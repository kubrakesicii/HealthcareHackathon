using Core.Enums;
using Core.Results;
using DataAccess.Abstract;
using DataAccess.Contexts;
using DataAccess.Repositories;
using Entities.Concrete;
using Entities.DTOs.Donation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Data;
using Entities.DTOs.User;
using System.Collections.Generic;

namespace DataAccess.Concrete
{
    public class UserCompletedDonationRepository : GenericRepository<UserCompletedDonation>, IUserCompletedDonationRepository
    {
        private readonly IAuthenticationRepository _authRepo;

        public UserCompletedDonationRepository(HealthcareContext context, IAuthenticationRepository authRepo) : base(context)
        {
            _authRepo = authRepo;
        }


        public async Task<Result> InsertCompletedDonation(CompletedDonationDto completedDonationDto)
        {
            // Bagis kaydi en basta akttif 0 kaydedilir, bagıs alan kisi onaylayınc aktif olur
            await InsertAsync(new UserCompletedDonation
            {
                DonorId = completedDonationDto.DonorId,
                DoneeId = completedDonationDto.DoneeId,
                DonationId = completedDonationDto.DonationId,
                IsActive = 0
            });

            return new Result(true);
        }


        public async Task<DataResult<List<GetDonationHistoryDto>>> GetUserDonationHistory(int userId)
        {
            Expression<Func<UserCompletedDonation, bool>> roleCond = x => true;
            var donations = _context.UserCompletedDonations.Include(x => x.Donation).Where(x => x.IsActive == 1);

            var role = await _context.Users.Where(x => x.Id == userId).Select(x => x.RoleId).FirstOrDefaultAsync();

            if (role == (int)UserType.Donor)
            {
                roleCond = x => x.DonorId == userId;
                donations = donations.Where(roleCond).Include(x => x.Donee);
                
            }
            else if (role == (int)UserType.Donee)
            {
                roleCond = x => x.DoneeId == userId;
                donations = donations.Where(roleCond).Include(x => x.DonorId);
            }

            var donationHistory = await donations
                   .Select(x => new GetDonationHistoryDto
                   {
                       Id = x.Id,
                       Donation = x.Donation.Name,
                       User = new GetInfoUserDto
                       {
                           Id = x.Donor != null ? x.Donor.Id : x.Donee.Id,
                           Name = x.Donor != null ? x.Donor.Firstname + x.Donor.Lastname : x.Donee.Firstname + x.Donee.Lastname
                       },
                       IsActive = x.IsActive,
                       CreatedAt = x.CreatedAt
                   }).ToListAsync();


            return new DataResult<List<GetDonationHistoryDto>>(donationHistory, true);


        }

        public async Task<DataResult<List<GetDonationHistoryDto>>> GetUnconfirmedDonations(int userId)
        {
            // Kullanıcıya eklenmis bagıs sonucları kullanıcı onaylamalı
            return new DataResult<List<GetDonationHistoryDto>>(await _context.UserCompletedDonations.Where(x => x.DoneeId == userId && x.IsActive == 0)
                   .Select(x => new GetDonationHistoryDto
                   {
                       Id = x.Id,
                       Donation = x.Donation.Name,
                       User = new GetInfoUserDto
                       {
                           Id = x.Donor.Id,
                           Name = x.Donor.Firstname + x.Donor.Lastname
                       },
                       IsActive = x.IsActive,
                       CreatedAt = x.CreatedAt
                   }).ToListAsync(), true);
        }

        public async Task<Result> ConfirmDonation(int donationId)
        {
            var userDonation = await _context.UserCompletedDonations.Where(x => x.Id == donationId).FirstOrDefaultAsync();
            userDonation.IsActive = 1;
            await UpdateAsync(userDonation);

            return new Result(true);
        }
    }
}
