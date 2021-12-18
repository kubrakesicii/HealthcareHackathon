using Core.Results;
using DataAccess.Abstract;
using DataAccess.Contexts;
using DataAccess.Repositories;
using Entities.Concrete;
using Entities.DTOs.Donation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete
{
    public class UserOngoingDonationRepository : GenericRepository<UserOngoingDonation> , IUserOngoingDonationRepository
    {
        public UserOngoingDonationRepository(HealthcareContext context) : base(context)
        {
        }

        public async Task<DataResult<List<GetDonationDto>>> GetUserDonations(int userId)
        {
            // Kullanıcıya baglı bagıs bilgisi burada tutulur,
            // Kullanıcı Donor ise buradakiler onun bagıslayabilecegi seylerdir
            // Kullanıcı Donee ise buradakiler onun agıs olmak istedigi seylerdir

            return new DataResult<List<GetDonationDto>>(await _context.UserOngoingDonations.Where(x => x.IsActive == 1 && x.UserId == userId).Include(x => x.Donation)
                .Select(x => new GetDonationDto
                {
                    Id = x.Id,
                    Name = x.Donation.Name,
                    Description = x.Donation.Description,
                    CreatedAt = x.CreatedAt
                }).ToListAsync(), true);
        }

        public async Task<Result> DeleteUserDonation(int id)
        {
            var userdon = await GetAsync(x => x.Id == id);
            userdon.IsActive = 0;
            await UpdateAsync(userdon);

            return new Result(true);
        }

        public async Task<Result> InsertUserDonation(OngoingDonationDto ongoingdonation)
        {
            return await InsertAsync(new UserOngoingDonation
            {
                UserId = ongoingdonation.UserId,
                DonationId = ongoingdonation.DonationId
            });
        }
    }
}
