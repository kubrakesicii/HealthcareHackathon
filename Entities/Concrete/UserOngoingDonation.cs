using System;
namespace Entities.Concrete
{
    public class UserOngoingDonation
    {
        public int Id { get; set; }

        // eger user rolü Donor -> urdaki donation onun bagılayabilecegi seydir
        // eger donee ise ihtiyac duydugu seydir
        public int UserId { get; set; }
        public User User { get; set; }

        public int DonationId { get; set; }
        public Donation Donation { get; set; }

        public int IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
