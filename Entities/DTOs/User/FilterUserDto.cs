using System;
using Entities.DTOs.Filter;

namespace Entities.DTOs.User
{
    public class FilterUserDto
    {
        // Aranacak kullanıcı türü : Donor or donee
        public int RoleId { get; set; }

        public IntervalFilterDto AgeFilter { get; set; }
        public IntervalFilterDto HeightFilter { get; set; }
        public IntervalFilterDto WeightFilter { get; set; }

        public int BloodType { get; set; }

        // "1;2" -> 1 veya 2 donationa sahip
        public string Donations { get; set; }
    }
}
