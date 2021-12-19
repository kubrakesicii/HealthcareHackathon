using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Core.Enums;
using Core.Exceptions;
using Core.Results;
using Core.Token;
using DataAccess.Abstract;
using DataAccess.Contexts;
using DataAccess.Repositories;
using Entities.Concrete;
using Entities.DTOs.User;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        //Generic Repoyu user ile extend edersem yazılı metotlar direk bu sınıf uzerinden kullanılabilir
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserRepository(HealthcareContext context,IHttpContextAccessor httpContextAccessor) : base(context)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        //Helper methods
        public static byte[] CryptoPassword(string password)
        {
            if (password == null)
                return null;
            MD5 md5 = new MD5CryptoServiceProvider();
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(password));
            return md5.Hash;
        }

        public static bool VerifyPassword(string password, byte[] userPass)
        {
            byte[] crypted = CryptoPassword(password);
            var verify = !crypted.Where((t, i) => t != userPass[i]).Any();
            return verify;
        }


        public async Task<DataResult<GetUserDto>> RegisterUser(InsertUserDto insertUserDto)
        {
            var imagePath = _httpContextAccessor.HttpContext.Items["ImagePath"].ToString();

            var user = new User
            {
                Firstname = insertUserDto.Firstname,
                Lastname = insertUserDto.Lastname,
                Email = insertUserDto.Email,
                Username = insertUserDto.Username,
                Password = CryptoPassword(insertUserDto.Password),
                ImagePath = imagePath,
                Age = insertUserDto.Age,
                Height = insertUserDto.Height,
                Weight = insertUserDto.Weight,
                Description = insertUserDto.Description,
                BloodType = insertUserDto.BloodType,
                RoleId = insertUserDto.RoleId,
                Address = insertUserDto.Address,
                DistrictId = insertUserDto.DistrictId
            };

            await InsertAsync(user);

            return new DataResult<GetUserDto>(new GetUserDto
            {
                Id = user.Id,
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                Email = user.Email,
                Username = user.Username,
                ImagePath = user.ImagePath,
                Age = user.Age,
                Height = user.Height,
                Weight = user.Weight,
                Description = user.Description,
                BloodType = user.BloodType,
                RoleId = user.RoleId,
                Address = user.Address,
            }, true);
        }


        public async Task<DataResult<GetLoginDto>> Login(LoginDto loginDto)
        {
            var user = await GetAsync(x => x.Email == loginDto.Email);
            if(user == null)
            {
                return new ErrorDataResult<GetLoginDto>("USER NOT FOUND");
            }

            var verify = VerifyPassword(loginDto.Password, user.Password);
            if (!verify)
            {
                return new ErrorDataResult<GetLoginDto>("WRONG PASSWORD");

            }

            return new DataResult<GetLoginDto>(new GetLoginDto
            {
                Id = user.Id,
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                Email = user.Email,
                Username = user.Username,
                ImagePath = user.ImagePath,
                Age = user.Age,
                Height = user.Height,
                Weight = user.Weight,
                Description = user.Description,
                BloodType = user.BloodType,
                RoleId = user.RoleId,
                Address = user.Address,
                DistrictId = user.DistrictId
            }, true);
        }

        public async Task<DataResult<GetUserDetailDto>> GetUserDetail(int id)
        {
            var user = await GetAsync(x => x.Id == id);
            return new DataResult<GetUserDetailDto>(new GetUserDetailDto
            {
                Id = user.Id,
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                Email = user.Email,
                Username = user.Username,
                ImagePath = user.ImagePath,
                Age = user.Age,
                Height = user.Height,
                Weight = user.Weight,
                Description = user.Description,
                BloodType = user.BloodType,
                RoleId = user.RoleId,
                Address = user.Address,
                District = _context.Districts.Where(x => x.Id == user.DistrictId).Select(x => x.Name).FirstOrDefault(),
                City = _context.Districts.Where(x => x.Id == user.DistrictId).Include(x => x.City).Select(x => x.City.Name).FirstOrDefault()
            }, true);
        }

        public async Task<DataResult<List<GetUserDetailDto>>> GetAllUsersByFilter(FilterUserDto filterUser)
        {
            //Tablo geldikten sonra searchkey verip arama yapabilir.

            var users = _context.Users.Where(x => x.IsActive == 1);
            var role = 0;


            if (filterUser.RoleId != 0)
            {
                users = users.Where(x => x.RoleId == filterUser.RoleId);
               
            }

            if(filterUser.AgeFilter != null)
            {
                users = users.Where(x => x.Age >= filterUser.AgeFilter.MinValue && x.Age < filterUser.AgeFilter.MaxValue);
            }

            if (filterUser.HeightFilter != null)
            {
                users = users.Where(x => x.Height >= filterUser.HeightFilter.MinValue && x.Height < filterUser.HeightFilter.MaxValue);
            }


            if (filterUser.WeightFilter != null)
            {
                users = users.Where(x => x.Weight >= filterUser.WeightFilter.MinValue && x.Weight < filterUser.WeightFilter.MaxValue);
            }

            if(filterUser.BloodType != 0)
            {
                users = users.Where(x => x.BloodType == filterUser.BloodType);
            }

            var filteredRes = await users.Select(x => x.Id).ToListAsync();

            List<int> filteredIds = new List<int>();
            if (filterUser.Donations != "")
            {
                foreach(var u in filteredRes)
                {
                    Expression<Func<UserOngoingDonation, bool>> donationCond = x => x.IsActive == 1;

                    var searchDonations = filterUser.Donations.Contains(";")
                        ? filterUser.Donations.Split(";").Select(n => Convert.ToInt32(n)).ToArray()
                        : new int[1] { Convert.ToInt32(filterUser.Donations) };

                    var userDons = await _context.UserOngoingDonations.Where(x => x.IsActive == 1 && x.UserId == u).Select(x => x.DonationId).ToListAsync();
                    if (searchDonations.Any(x => userDons.Contains(x)))
                        filteredIds.Add(u);
                }
            }
            else
            {
                filteredIds.AddRange(filteredRes);
            }


            List<GetUserDetailDto> filteredUsers = new List<GetUserDetailDto>();
            foreach (var user in filteredIds)
            {
                filteredUsers.Add((await GetUserDetail(user)).Data);
            }


            return new DataResult<List<GetUserDetailDto>>(filteredUsers, true);
        }

        public async Task<DataResult<List<GetUserDto>>> GetAllUsers()
        {
            return new DataResult<List<GetUserDto>>(await _context.Users.Where(x => x.IsActive == 1).Select(x => new GetUserDto {
                Id = x.Id,
                Firstname = x.Firstname,
                Lastname = x.Lastname,
                Email = x.Email,
                Username = x.Username,
                ImagePath = x.ImagePath,
                Age = x.Age,
                Height = x.Height,
                Weight = x.Weight,
                Description = x.Description,
                BloodType = x.BloodType,
                RoleId = x.RoleId,
                Address = x.Address,
            }).ToListAsync(), true);
        }

        public async Task<Result> UpdateUser(int id, UpdateUserDto updateUserDto)
        {
            var imagePath = _httpContextAccessor.HttpContext.Items["ImagePath"].ToString();

            var user = await GetAsync(x => x.Id == id);

            if (user == null)
            {
                return new ErrorDataResult<GetUserDto>("USER NOTFOUND");
            }

            user.Firstname = updateUserDto.Firstname != null ? updateUserDto.Firstname : user.Firstname;
            user.Lastname = updateUserDto.Lastname != null ? updateUserDto.Lastname : user.Lastname;
            user.Email = updateUserDto.Email != null ? updateUserDto.Email : user.Email;
            user.Address = updateUserDto.Address != null ? updateUserDto.Address : user.Address;
            user.Age = updateUserDto.Age != 0 ? updateUserDto.Age : user.Age;
            user.Weight = updateUserDto.Weight != 0 ? updateUserDto.Weight : user.Weight;
            user.Height = updateUserDto.Height != 0 ? updateUserDto.Height : user.Height;
            user.Description = updateUserDto.Description != null ? updateUserDto.Description : user.Description;
            user.BloodType = updateUserDto.BloodType != 0 ? updateUserDto.BloodType : user.BloodType;
            user.DistrictId = updateUserDto.DistrictId != 0 ? updateUserDto.DistrictId : user.DistrictId;
            user.Password = updateUserDto.Password != null ? CryptoPassword(updateUserDto.Password) : user.Password;

            await UpdateAsync(user);

            return new Result(true);
        }

        public async Task<DataResult<GetUserDto>> GetUser(int id)
        {
            return new DataResult<GetUserDto>(await _context.Users.Where(x => x.Id == id).Select(x => new GetUserDto
            {
                Id = x.Id,
                Firstname = x.Firstname,
                Lastname = x.Lastname,
                Email = x.Email,
                Username = x.Username,
                ImagePath = x.ImagePath,
                Age = x.Age,
                Height = x.Height,
                Weight = x.Weight,
                Description = x.Description,
                BloodType = x.BloodType,
                RoleId = x.RoleId,
                Address = x.Address,
            }).FirstOrDefaultAsync(), true);
        }
    }
}
