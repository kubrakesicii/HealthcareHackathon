using System;
namespace Entities.DTOs.Donation
{
    public class GetDonationDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
