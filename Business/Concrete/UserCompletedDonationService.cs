using Business.Abstract;
using DataAccess.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class UserCompletedDonationService : IUserCompletedDonationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserCompletedDonationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    }
}
