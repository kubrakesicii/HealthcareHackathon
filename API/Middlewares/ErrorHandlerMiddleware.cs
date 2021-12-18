﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace API.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, IWebHostEnvironment env)
        {

            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex, env);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception, IWebHostEnvironment env)
        {
            HttpStatusCode status;
            string message;
            var stackTrace = string.Empty;
            int statusCode = 200;
            string data = null;
            bool success = false;

            var exceptionType = exception.GetType();
            if (exceptionType == typeof(BadRequestException))
            {
                message = exception.InnerException?.Message ?? exception.Message;
                status = HttpStatusCode.BadRequest;
                statusCode = (int)HttpStatusCode.BadRequest;
            }
            else if (exceptionType == typeof(ValidationException))
            {
                message = exception.InnerException?.Message ?? exception.Message;
                status = HttpStatusCode.BadRequest;
                statusCode = (int)HttpStatusCode.BadRequest;
            }
            else if (exceptionType == typeof(NotFoundException))
            {
                message = exception.InnerException?.Message ?? exception.Message;
                status = HttpStatusCode.NotFound;
                statusCode = (int)HttpStatusCode.NotFound;
            }
            else if (exceptionType == typeof(ForbiddenException))
            {
                message = exception.InnerException?.Message ?? exception.Message;
                status = HttpStatusCode.Forbidden;
                statusCode = (int)HttpStatusCode.Forbidden;
            }
            else if (exceptionType == typeof(Core.Exceptions.AuthenticationException))
            {
                message = exception.InnerException?.Message ?? exception.Message;
                status = HttpStatusCode.Unauthorized;
                statusCode = (int)HttpStatusCode.Unauthorized;
            }
            else if (exceptionType == typeof(TokenExpireException))
            {
                message = exception.InnerException?.Message ?? exception.Message;
                status = HttpStatusCode.Unauthorized;
                statusCode = (int)HttpStatusCode.Unauthorized;
            }
            else
            {
                Console.WriteLine("Internal Server hatasına geldi!!");

                status = HttpStatusCode.InternalServerError;
                message = exception.InnerException?.Message ?? exception.Message;
                statusCode = (int)HttpStatusCode.InternalServerError;
                //if (env.IsEnvironment("Development"))
                stackTrace = exception.StackTrace;
                Console.WriteLine(message);
                //throw new InternalServerException("INTERNAL SERVER ERROR");
            }

            var result = JsonSerializer.Serialize(new { data = data, success = false, message = message });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)status;
            return context.Response.WriteAsync(result);
        }
    }


    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ErrorHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseErrorHandlerMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlerMiddleware>();
        }
    }

}
