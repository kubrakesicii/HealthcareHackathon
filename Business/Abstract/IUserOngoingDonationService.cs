using Core.Results;
using Entities.Concrete;
using Entities.DTOs.Donation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IUserOngoingDonationService
    {
        Task<DataResult<List<GetDonationDto>>> GetUserDonations(int userId);
        Task<Result> InsertUserDonation(OngoingDonationDto ongoingDonation);
        Task<Result> DeleteUserDonation(int id);
    }
}
