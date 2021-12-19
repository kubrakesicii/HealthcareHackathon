using System;
namespace DataAccess.Abstract
{
    public class IAuthenticationService
    {
        public bool IsAuthenticated { get; }
        public int Id { get; }
        public string FullName { get; }
        public int RoleId { get; }
    }
}
