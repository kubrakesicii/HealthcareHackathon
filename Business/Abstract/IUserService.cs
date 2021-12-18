using System;
using System.Threading.Tasks;
using Core.Results;
using Entities.DTOs.User;

namespace Business.Abstract
{
    public interface IUserService
    {
        Task<Result> RegisterUser(InsertUserDto insertUserDto);
        Task<GetLoginDto> Login(LoginDto loginDto);
        Task<GetUserDetailDto> GetUserDetail(int id);
    }
}
