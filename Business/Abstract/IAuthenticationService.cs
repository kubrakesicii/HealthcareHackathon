using System;
namespace Business.Abstract
{
    public class IAuthenticationService
    {
        bool IsAuthenticated { get; }
        int Id { get; }
        string FullName { get; }
        int RoleId { get; }
    }
}
