using System;
using System.ComponentModel.DataAnnotations;

namespace Core
{
    public enum UserType
    {
        [Display(Name = "Donor")] Donor = 1,  // Bağış yapan kişi
        [Display(Name = "Donee")] Donee = 1   // Bağış alan kişi
    }
}
