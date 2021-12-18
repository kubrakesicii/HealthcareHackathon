using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Enums
{
    public enum BloodType
    {
        [Display(Name = "0 Rh+")] ZeroRhP = 1,
        [Display(Name = "0 Rh-")] ZeroRhN = 2,
        [Display(Name = "A Rh+")] ARhP = 3,
        [Display(Name = "A Rh-")] ARhN = 4,
        [Display(Name = "B Rh+")] BRhP = 5,
        [Display(Name = "B Rh-")] BRhN = 6,
        [Display(Name = "AB Rh+")] ABRhP = 7,
        [Display(Name = "AB Rh-")] ABRhN = 8,
    }
}
