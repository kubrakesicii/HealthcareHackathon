using Business.Abstract;
using Core.Results;
using DataAccess.UnitOfWork;
using Entities.Concrete;
using Entities.DTOs.Donation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class UserOngoingDonationService : IUserOngoingDonationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserOngoingDonationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> DeleteUserDonation(int id)
        {
            return await _unitOfWork.OngoingDonations.DeleteUserDonation(id);
        }

        public async Task<DataResult<List<GetDonationDto>>> GetUserDonations(int userId)
        {
            return await _unitOfWork.OngoingDonations.GetUserDonations(userId);
        }

        public async Task<Result> InsertUserDonation(OngoingDonationDto ongoingDonation)
        {
            return await _unitOfWork.OngoingDonations.InsertUserDonation(ongoingDonation);
        }
    }
}
