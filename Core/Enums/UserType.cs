using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Enums
{
    public enum UserType
    {
        [Display(Name = "Donor")] Donor = 1,  // Bağış yapan kişi
        [Display(Name = "Donee")] Donee = 2   // Bağış alan kişi
    }
}
