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
        private readonly IAuthenticationService _authService;

        public UserCompletedDonationService(IUnitOfWork unitOfWork, IAuthenticationService authService)
        {
            _unitOfWork = unitOfWork;
            _authService = authService;
        }
    }
}
