using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
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

namespace DataAccess.Concrete
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        //Generic Repoyu user ile extend edersem yazılı metotlar direk bu sınıf uzerinden kullanılabilir
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITokenService _tokenService;

        public UserRepository(IHttpContextAccessor httpContextAccessor, ITokenService tokenService)
        {
            _httpContextAccessor = httpContextAccessor;
            _tokenService = tokenService;
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


        public async Task<GetUserDto> RegisterUser(InsertUserDto insertUserDto)
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

            return new GetUserDto
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
            };
        }


        public async Task<GetLoginDto> Login(LoginDto loginDto)
        {
            var user = await GetAsync(x => x.Username == loginDto.Username);
            if(user == null)
            {
                throw new NotFoundException("USER NOT FOUND");
            }

            var verify = VerifyPassword(loginDto.Password, user.Password);
            if (!verify)
            {
                throw new BadRequestException("WRONG PASSWORD");
            }

            var token = _tokenService.CreateToken(user.Id, $"{user.Firstname} {user.Lastname}", user.RoleId);

            return new GetLoginDto
            {
                User = new GetUserDto
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
                },
                Token = token
            };
        }

        public async Task<GetUserDetailDto> GetUserDetail(int id)
        {
            var user = await GetAsync(x => x.Id == id);
            return new GetUserDetailDto
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
            };
        }

        public async Task<List<GetUserDetailDto>> GetAllUsersByFilter(FilterUserDto filterUser)
        {
            //Tablo geldikten sonra searchkey verip arama yapabilir.

            var users = _context.Users.Where(x => x.IsActive == 1);

            if(filterUser.RoleId != 0)
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

                    foreach (var d in searchDonations)
                    {
                        donationCond = donationCond.Or(x => x.DonationId == d);
                    }

                    var donations = await _context.UserOngoingDonations.Where(donationCond).Select(x => x.UserId).Distinct().ToListAsync();
                    filteredIds.AddRange(donations);
                }
            }
            else
            {
                filteredIds.AddRange(filteredRes);
            }


            List<GetUserDetailDto> filteredUsers = new List<GetUserDetailDto>();
            foreach (var user in filteredIds)
            {
                filteredUsers.Add(await GetUserDetail(user));
            }


            return filteredUsers;
        }
    }
}
