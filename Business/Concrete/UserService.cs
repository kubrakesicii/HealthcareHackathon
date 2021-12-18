using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Abstract;
using Core;
using Core.Results;
using Core.Token;
using DataAccess.Abstract;
using Entities.DTOs.User;
using Microsoft.AspNetCore.Http;

namespace Business.Concrete
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDocumentRepository _documentRepo;
        private readonly IDonationRepository _donationRepo;


        public UserService(IUserRepository userRepo, IHttpContextAccessor httpContextAccessor, IDocumentRepository documentRepo,
            IDonationRepository donationRepo)
        {
            _userRepo = userRepo;
            _httpContextAccessor = httpContextAccessor;
            _documentRepo = documentRepo;
            _donationRepo = donationRepo;
        }

        public async Task<Result> RegisterUser(InsertUserDto insertUserDto)
        {
            // Kullanıcı eklendi ve ona ait raporlar eklendi
            var user = await _userRepo.RegisterUser(insertUserDto);
            await _documentRepo.InsertDocuments(user.Id);

            return new Result(true);
        }

        public async Task<GetLoginDto> Login(LoginDto loginDto)
        {
            return await _userRepo.Login(loginDto);         
        }

        public async Task<GetUserDetailDto> GetUserDetail(int id)
        {
            var user = await _userRepo.GetUserDetail(id);
            user.Documents = await _documentRepo.GetUserDocuments(id);

            // Donor icin bagılayabilecegi seyler - donee icin bagıs almak istedigi seyler
            user.Donations = await _donationRepo.GetUserOngoingDonations(id);
            user.DontionHistory = await _donationRepo.GetDonationHistory(id);

            return user;
        }
    }
}
