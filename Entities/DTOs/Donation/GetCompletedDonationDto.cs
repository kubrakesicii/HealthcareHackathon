using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs.Donation
{
    public class GetCompletedDonationDto
    {
        public int Id { get; set; }

        // Bagıs alınan kisi veya bagıs yapılan kisi
        public int UserId { get; set; }
        public int CreatedAt { get; set; }
        public int IsActive { get; set; }
    }
}
