﻿using Business.Abstract;
using Core.Exceptions;
using Core.Token;
using DataAccess.Abstract;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace API.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly JwtOption _jwtOptions;
        public AuthMiddleware(RequestDelegate next, JwtOption jwtOptions)
        {
            _next = next;
            _jwtOptions = jwtOptions;
        }

        public async Task Invoke(HttpContext httpContext, IUserService userService)
        {
            var token = httpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
            {
                try
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes(_jwtOptions.SecurityKey);
                    tokenHandler.ValidateToken(token, new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                        ClockSkew = TimeSpan.Zero
                    }, out SecurityToken validatedToken);

                    if (validatedToken != null)
                    {
                        var jwtToken = (JwtSecurityToken)validatedToken;
                        Console.WriteLine("Expire : " + jwtToken.ValidTo);
                        Console.WriteLine("Now : " + DateTime.Now);

                        if (DateTime.Now > jwtToken.ValidTo.AddHours(3))
                        {
                            var result = JsonSerializer.Serialize(new { data = "", message = "TOKENEXPIRED", statusCode = StatusCodes.Status401Unauthorized });

                            httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            httpContext.Response.ContentType = "application/json";

                            await httpContext.Response.WriteAsync(result);
                        }

                        var userId = int.Parse(jwtToken.Claims.First(x => x.Type.Equals("id")).Value);

                        // attach user to context on successful jwt validation
                        var user = await userService.GetUser(userId);

                        httpContext.Items["User"] = user;
                    }
                    else
                    {
                        throw new AuthenticationException("UNAUTHORIZED");
                        /*
                        var result = JsonSerializer.Serialize(new { data = "", message = "UNAUTHORIZED", statusCode = StatusCodes.Status401Unauthorized });
                        httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        httpContext.Response.ContentType = "application/json";
                        await httpContext.Response.WriteAsync(result);
                        */
                    }
                }
                catch (Exception e)
                {
                    var ex = e;
                    // Eger token süresi dolmuşsa Validate metodunda direk exception atar ve catch bloguna düşer.
                    throw new TokenExpireException("TOKENEXPIRED");
                }
            }

            await _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class AuthMiddlewareExtensions
    {
        public static IApplicationBuilder UseAuthMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthMiddleware>();
        }
    }
}
