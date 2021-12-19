using API.Middlewares;
using Business.Abstract;
using Business.Concrete;
using Core.Token;
using DataAccess.Abstract;
using DataAccess.Concrete;
using DataAccess.Contexts;
using DataAccess.Repositories;
using DataAccess.UnitOfWork;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Text;

namespace API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddMvc();
            // CORS
            services.AddCors(options => options.AddDefaultPolicy(builder =>
                builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader())
            );


            //SWAGGER
            services.AddSwaggerGen(c =>
            {

                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Healthcare API",
                    Version = "v1",
                    Description = ".NET Core 6.0.100"
                });

                var securitySchema = new OpenApiSecurityScheme
                {
                    Description =
                        "JWT Authorization by using Bearer Scheme. For example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                };
                c.AddSecurityDefinition("Bearer", securitySchema);
                var securityRequirement = new OpenApiSecurityRequirement { { securitySchema, new[] { "Bearer" } } };
                c.AddSecurityRequirement(securityRequirement);
            });

            //Db context
            services.AddDbContext<HealthcareContext>(options =>
                options.UseSqlServer("Server=127.0.0.1,1433; Database=HealthcareDb; User=sa; Password = AyremX.123", x => x.MigrationsAssembly("DataAccess")));

            
            //JWT IMP
            var jwtOptions = new JwtOption
            {
                Issuer = "healthcare.net",
                Audience = "healthcare.net",
                SecurityKey = "healthcarehackathon2021!",
                AccessTokenExpiration = 60 * 24 * 7,
                RefreshTokenExpiration = 60 * 24 * 10
            };
            services.AddSingleton(jwtOptions);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecurityKey)),
                    ClockSkew = TimeSpan.Zero
                };
            });


            //services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IDocumentService, DocumentService>();
            services.AddScoped<IUserOngoingDonationService, UserOngoingDonationService>();
            services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //SWAGGER
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Api Service");
                c.DefaultModelsExpandDepth(-1);
                c.DocExpansion(DocExpansion.None);
            });

            //CORS
            app.UseCors();

            app.UseRouting();
            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseHttpsRedirection();


            // ERROR MID
            app.UseErrorHandlerMiddleware();


            // File Middleware
            app.UseWhen(
               httpContext => (httpContext.Request.Path.StartsWithSegments("/Users/Register") && httpContext.Request.Method.Equals("POST")
               ),
               subApp => subApp.UseFileHandlerMiddleware()
            );


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
