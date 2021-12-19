using System;
namespace DataAccess.Abstract
{
    public class IAuthenticationRepository
    {
        public bool IsAuthenticated { get; }
        public int Id { get; }
        public string FullName { get; }
        public int RoleId { get; }
    }
}
