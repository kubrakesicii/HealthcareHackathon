﻿using DataAccess.Abstract;
using DataAccess.Contexts;
using DataAccess.Repositories;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete
{
    public class UserCompletedDonationRepository : GenericRepository<UserCompletedDonation>, IUserCompletedDonationRepository
    {
        public UserCompletedDonationRepository(HealthcareContext context) : base(context)
        {
        }
    }
}
