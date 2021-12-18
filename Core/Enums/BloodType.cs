using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Enums
{
    public enum BloodType
    {
        [Display(Name = "0 Rh+")] ZeroRhP = 1,
        [Display(Name = "0 Rh-")] ZeroRhN = 1,
        [Display(Name = "A Rh+")] ARhP = 1,
        [Display(Name = "A Rh-")] ARhN = 1,
        [Display(Name = "B Rh+")] BRhP = 1,
        [Display(Name = "B Rh-")] BRhN = 1,
        [Display(Name = "AB Rh+")] ABRhP = 1,
        [Display(Name = "AB Rh-")] ABRhN = 1,
    }
}
