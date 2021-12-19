using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Core;
using Core.Results;
using Core.Token;
using DataAccess.Abstract;
using DataAccess.UnitOfWork;
using Entities.DTOs.Donation;
using Entities.DTOs.User;
using Microsoft.AspNetCore.Http;

namespace Business.Concrete
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;
        private readonly IAuthenticationService _authRepo;

        public UserService(IUnitOfWork unitOfWork, ITokenService tokenService, IAuthenticationService authRepo)
        {
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
            _authRepo = authRepo;
        }

        public async Task<Result> RegisterUser(InsertUserDto insertUserDto)
        {
            // Kullanıcı eklendi ve ona ait raporlar eklendi
            var user = await _unitOfWork.Users.RegisterUser(insertUserDto);
            await _unitOfWork.Documents.InsertDocuments(user.Data.Id);
            if(insertUserDto.Donations.Count() > 0)
            {
                foreach (var d in insertUserDto.Donations)
                {
                    await _unitOfWork.OngoingDonations.InsertUserDonation(new OngoingDonationDto
                    {
                        UserId = _authRepo.Id,
                        DonationId = d
                    });
                }
            }


            return new Result(true);
        }

        public async Task<DataResult<GetLoginDto>> Login(LoginDto loginDto)
        {
            var user = await _unitOfWork.Users.Login(loginDto);
            if (user.Data != null)
                user.Data.Token = _tokenService.CreateToken(user.Data.Id, $"{user.Data.Firstname} {user.Data.Lastname}", user.Data.RoleId);
            return user;
        }

        public async Task<DataResult<GetUserDetailDto>> GetUserDetail(int id)
        {
            var user = await _unitOfWork.Users.GetUserDetail(id);
            user.Data.Documents = (await _unitOfWork.Documents.GetUserDocuments(id)).Data;

            // Donor icin bagılayabilecegi seyler - donee icin bagıs almak istedigi seyler
            user.Data.Donations = (await _unitOfWork.OngoingDonations.GetUserDonations(id)).Data;
            //user.Data.DontionHistory = (await _unitOfWork.Donations.GetDonationHistory(id)).Data;

            return user;
        }

        public async Task<DataResult<dynamic>> GetAllUsers()
        {
            return await _unitOfWork.Users.GetAllUsers(_authRepo.Id);
        }

        public async Task<DataResult<List<GetUserDetailDto>>> GetAllUsersByFilter(FilterUserDto filterUser)
        {
            return await _unitOfWork.Users.GetAllUsersByFilter(filterUser);
        }

        public async Task<Result> UpdateUser(int id, UpdateUserDto updateUserDto)
        {
            return await _unitOfWork.Users.UpdateUser(id, updateUserDto);
        }

        public async Task<DataResult<GetUserDto>> GetUser(int id)
        {
            return await _unitOfWork.Users.GetUser(id);
        }
    }
}
