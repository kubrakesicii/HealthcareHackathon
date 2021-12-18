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
        Task<GetUserDto> RegisterUser(InsertUserDto insertUserDto);
        Task<GetLoginDto> Login(LoginDto loginDto);
        Task<GetUserDetailDto> GetUserDetail(int id);
        Task<List<GetUserDetailDto>> GetAllUsersByFilter(FilterUserDto filterUser);
    }
}
