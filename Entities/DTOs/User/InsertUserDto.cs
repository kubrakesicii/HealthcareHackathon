using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Entities.DTOs.User
{
    public class InsertUserDto
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public int Age { get; set; }
        public decimal Weight { get; set; }
        public int Height { get; set; }
        public string Description { get; set; }

        public int BloodType { get; set; }

        //Enum -> Donee-Donor
        public int RoleId { get; set; }

        public string Address { get; set; }

        public int DistrictId { get; set; }

        public List<int> Donations { get; set; }
        public IFormFile Image { get; set; }
        public List<IFormFile> Documents { get; set; }
    }
}
