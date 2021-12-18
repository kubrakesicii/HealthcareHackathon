using API.Middlewares;
using Business.Abstract;
using Business.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete;
using DataAccess.Contexts;
using DataAccess.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;

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

            //SWAGGER
            services.AddSwaggerGen(c =>
            {

                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "YatBu API",
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
                options.UseSqlServer("Server=localhost,2000; Database=HealthcareDb; User=root; Password = ", x => x.MigrationsAssembly("DataAccess")));


            //services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRepository, UserRepository>();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Api Service");
                c.DefaultModelsExpandDepth(-1);
                c.DocExpansion(DocExpansion.None);
            });
        

            app.UseRouting();

            app.UseAuthorization();

            // ERROR MID
            app.UseErrorHandlerMiddleware();


            // File Middleware
            app.UseWhen(
               httpContext => (httpContext.Request.Path.StartsWithSegments("/Users") || httpContext.Request.Method.Equals("POST")
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
