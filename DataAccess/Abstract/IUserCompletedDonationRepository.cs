using Core.Results;
using DataAccess.Repositories;
using Entities.Concrete;
using Entities.DTOs.Donation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface IUserCompletedDonationRepository : IGenericRepository<UserCompletedDonation>
    {
        Task<Result> InsertCompletedDonation(CompletedDonationDto completedDonationDto);
        //Task<DataResult<GetCompletedDonationDto>> GetUserDonationHistory(int userId);

    }
}
