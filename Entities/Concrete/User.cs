using System;
namespace Entities.Concrete
{
    public class User
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public byte[] Password { get; set; }
        public string Phone { get; set; }
        public int Age { get; set; }
        public decimal Weight { get; set; }
        public int Height { get; set; }
        public string ImagePath { get; set; }
        public string Description { get; set; }

        // Enum
        public int BloodType { get; set; }

        //Enum -> Patient-Donor
        public int RoleId { get; set; }

        public string Address { get; set; }

        public int DistrictId { get; set; }

        public int IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdtatedAt { get; set; }
    }
}
