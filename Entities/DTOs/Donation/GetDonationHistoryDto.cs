using System;
using Entities.DTOs.User;

namespace Entities.DTOs.Donation
{
    public class GetDonationHistoryDto
    {
        public int Id { get; set; }
        public string Donation { get; set; }
        public GetInfoUserDto User { get; set; }
        public DateTime CreatedAt { get; set; }
        public int IsActive { get; set; }
    }
}
