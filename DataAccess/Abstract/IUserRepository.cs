using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Results;
using DataAccess.Repositories;
using Entities.Concrete;
using Entities.DTOs.User;

namespace DataAccess.Abstract
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<DataResult<GetUserDto>> RegisterUser(InsertUserDto insertUserDto);
        Task<DataResult<GetLoginDto>> Login(LoginDto loginDto);
        Task<DataResult<GetUserDetailDto>> GetUserDetail(int id);
        Task<DataResult<List<GetUserDto>>> GetAllUsers();
        Task<DataResult<List<GetUserDetailDto>>> GetAllUsersByFilter(FilterUserDto filterUser);
        Task<Result> UpdateUser(int id, UpdateUserDto updateUserDto);

    }
}
