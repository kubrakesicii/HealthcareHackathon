using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs.Donation
{
    public class CompletedDonationDto
    {
        public int DonorId { get; set; }
        public int DoneeId { get; set; }
        public int DonationId { get; set; }
    }
}
