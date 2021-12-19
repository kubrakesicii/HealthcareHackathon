using System;
using System.Linq;
using DataAccess.Abstract;
using DataAccess.Contexts;
using Microsoft.AspNetCore.Http;

namespace DataAccess.Concrete
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HealthcareContext _context;

        public AuthenticationService(IHttpContextAccessor httpContextAccessor, HealthcareContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        public bool IsAuthenticated => _httpContextAccessor?.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

        public int Id => Convert.ToInt32(_httpContextAccessor?.HttpContext?.User?.Claims.FirstOrDefault(x => x.Type.Equals("id"))?.Value);


        public string FullName => _httpContextAccessor?.HttpContext?.User?.Claims.FirstOrDefault(x => x.Type.Equals("fullName"))?.Value;

        public int RoleId => Convert.ToInt32(_httpContextAccessor?.HttpContext?.User?.Claims.FirstOrDefault(x => x.Type.Equals("roleId"))?.Value);

    }
}