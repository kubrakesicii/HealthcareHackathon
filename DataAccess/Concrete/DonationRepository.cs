using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core;
using DataAccess.Abstract;
using DataAccess.Repositories;
using Entities.Concrete;
using Entities.DTOs.Donation;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete
{
    public class DonationRepository : GenericRepository<Donation>, IDonationRepository
    {
        public async Task<List<GetDonationDto>> GetDonationHistory(int userId)
        {
            Expression<Func<UserCompletedDonation , bool>> userCond = x => true;
            var user = await _context.Users.Where(x => x.Id == userId).FirstOrDefaultAsync();
            if(user.RoleId == (int)UserType.Donee)
            {
                userCond = x => x.DoneeId == userId;
            }
            else
            {
                userCond = x => x.DonorId == userId;
            }

            return await _context.UserCompletedDonations.Include(x => x.Donation).Where(userCond).Select(x => new GetDonationDto
            {
                Id = x.Id,
                Name = x.Donation.Name,
                CreatedAt = x.CreatedAt,
            }).ToListAsync();
        }

        public async Task<List<GetDonationDto>> GetUserOngoingDonations(int userId)
        {
            // Burada donorun bagıslayabildigi, doneenin bagıs alacagı seyler gelir
            return await _context.UserOngoingDonations.Include(x => x.Donation).Where(x => x.UserId == userId).Select(x => new GetDonationDto
            {
                Id = x.Id,
                Name = x.Donation.Name,
                CreatedAt = x.CreatedAt,
                Type = 2 // Bagıs bekleyen seyler
            }).ToListAsync();
        }
    }
}
