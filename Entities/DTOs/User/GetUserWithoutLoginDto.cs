using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs.User
{
    public class GetUserWithoutLoginDto : IGetUser
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public int BloodType { get; set; }

        //Enum -> Donee-Donor
        public int RoleId { get; set; }
    }
}
