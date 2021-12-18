using System;
using System.Collections.Generic;
using Entities.Concrete;
using Entities.DTOs.Document;
using Entities.DTOs.Donation;

namespace Entities.DTOs.User
{
    public class GetUserDetailDto
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
        public string District { get; set; }
        public string City { get; set; }

        public List<GetDonationDto> Donations { get; set; }
        public List<GetDonationDto> DontionHistory { get; set; }
        public List<GetDocumentDto> Documents { get; set; }
    }
}
