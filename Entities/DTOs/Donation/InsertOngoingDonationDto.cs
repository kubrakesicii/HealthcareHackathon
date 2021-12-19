using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs.Donation
{
    public class InsertOngoingDonationDto
    {
        public List<OngoingDonationDto> OngoingDonations { get; set; }
    }
}
