using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccess.Repositories;
using Entities.Concrete;
using Entities.DTOs.Donation;

namespace DataAccess.Abstract
{
    public interface IDonationRepository : IGenericRepository<Donation>
    {
        Task<List<GetDonationDto>> GetUserOngoingDonations(int userId);
        Task<List<GetDonationDto>> GetDonationHistory(int userId);


    }
}
