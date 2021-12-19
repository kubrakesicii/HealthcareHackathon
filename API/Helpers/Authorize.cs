using Entities.DTOs.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Helpers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]

    public class Authorize : Attribute, IAuthorizationFilter
    {
        private readonly IList<int> _roles;

        /*
        public Authorize(params Role[] Roles) : base()
        {
            _roles = Roles.Select(x => (int)x).ToList();
        }
        */
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
            if (allowAnonymous)
                return;

            // authorization
            var user = (GetUserDto)context.HttpContext.Items["User"];
            if (user == null)
            {
                // user authorize edilmemiş. UnAuth dön
                context.Result = new JsonResult(new { data = "", message = "UNAUTHORIZED", statusCode = StatusCodes.Status401Unauthorized }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
            else
            {
                // User Auth edilmiş ama rolünün isteğe yetkisi yok. Forbidden
                /*
                if (_roles.Any() && !_roles.Contains(user.RoleId))
                {
                    context.Result = new JsonResult(new { data = "", message = "FORBIDDEN", statusCode = StatusCodes.Status403Forbidden }) { StatusCode = StatusCodes.Status403Forbidden };
                }
                */
            }
        }
    }
}
