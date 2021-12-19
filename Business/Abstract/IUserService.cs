using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Results;
using Entities.DTOs.User;

namespace Business.Abstract
{
    public interface IUserService
    {
        Task<Result> RegisterUser(InsertUserDto insertUserDto);
        Task<DataResult<GetLoginDto>> Login(LoginDto loginDto);
        Task<DataResult<GetUserDetailDto>> GetUserDetail(int id);
        Task<DataResult<List<GetUserDto>>> GetAllUsers();
        Task<DataResult<GetUserDto>> GetUser(int id);
        Task<DataResult<List<GetUserDetailDto>>> GetAllUsersByFilter(FilterUserDto filterUser);
        Task<Result> UpdateUser(int id, UpdateUserDto updateUserDto);
    }
}
