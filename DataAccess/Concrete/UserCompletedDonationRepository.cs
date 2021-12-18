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

namespace DataAccess.Concrete
{
    public class UserCompletedDonationRepository : GenericRepository<UserCompletedDonation>, IUserCompletedDonationRepository
    {
        public UserCompletedDonationRepository(HealthcareContext context) : base(context)
        {
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

       
    }
}
