using System;
namespace Entities.Concrete
{
    public class UserCompletedDonation
    {
        public int Id { get; set; }

        public int DonorId { get; set; }
        public User Donor { get; set; }

        public int DoneeId { get; set; }
        public User Donee { get; set; }

        public int DonationId { get; set; }
        public Donation Donation { get; set; }

        public DateTime CreatedAt { get; set; }
        public int IsActive { get; set; }
    }
}
