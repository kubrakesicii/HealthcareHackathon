using System;
namespace Entities.Concrete
{
    public class Badget
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public int DonationId { get; set; }
        public Donation Donation { get; set; }

        public int Count { get; set; }
        public int IsActive { get; set; }
    }
}
