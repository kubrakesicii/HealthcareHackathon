using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace API.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class FileHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public FileHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            List<string> docPaths = new List<string>();
            var imagePath = "/Images/default.jpg";

            if (httpContext.Request.Form.Count() > 0)
            {
                try
                {
                    //httpContext.Request.Form.Files -> IFormFile türünde
                    foreach (var formFile in httpContext.Request.Form.Files)
                    {
                        var extension = Path.GetExtension(formFile.FileName);
                        var name = Guid.NewGuid() + extension;
                        var path = "";
                        var location = "";

                        if(formFile.Name.Equals("Documents"))
                        {
                            path = Path.Combine("/Documents/", name);
                            location = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Documents/", name);

                            docPaths.Add(path);
                        }
                        else if (formFile.Name.Equals("Image"))
                        {
                            path = Path.Combine("/Images/", name);
                            location = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/", name);

                            imagePath = path;
                        }


                        using (var fileStream = new FileStream(location, FileMode.Create))
                        {
                            await formFile.CopyToAsync(fileStream);
                        }
                    }

                    httpContext.Items["DocPaths"] = docPaths;
                    httpContext.Items["ImagePath"] = imagePath;

                    await _next(httpContext);
                }
                catch (Exception e)
                {

                }
            }

            await _next(httpContext);
        }
    }
    

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class FileHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseFileHandlerMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<FileHandlerMiddleware>();
        }
    }
}
