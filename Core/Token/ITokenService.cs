using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Core.Token
{
    public interface ITokenService
    {
        // Bu metodun parametrelerine, token payload içine gömülmek istene tüm datalar verilebilir.
        string CreateToken(int id, string fullName, int roleId);
        Task<List<Claim>> GetUserClaims();
    }
}
