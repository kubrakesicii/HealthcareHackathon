using System;
namespace Entities.DTOs.User
{
    public class GetUserDto
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public int Age { get; set; }
        public decimal Weight { get; set; }
        public int Height { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public int BloodType { get; set; }

        //Enum -> Donee-Donor
        public int RoleId { get; set; }

        public string Address { get; set; }

        public int DistrictId { get; set; }
    }
}
