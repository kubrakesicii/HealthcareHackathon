using System;
namespace Entities.DTOs.User
{
    public class GetLoginDto
    {
        public GetUserDto User { get; set; }
        public string Token { get; set; }
    }
}

