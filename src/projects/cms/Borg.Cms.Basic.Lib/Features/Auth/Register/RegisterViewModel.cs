using System.ComponentModel.DataAnnotations;

namespace Borg.Cms.Basic.Lib.Features.Auth.Register
{
    public class RegisterViewModel
    {
        static RegisterViewModel()
        {
        }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        //[Required]
        //[StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        //[DataType(DataType.Password)]
        //[Display(Name = "Password")]
        //public string Password { get; set; }

        //[DataType(DataType.Password)]
        //[Display(Name = "Confirm password")]
        //[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        //public string ConfirmPassword { get; set; }

        //[Required]
        //[Display(Name = "First Name")]
        //public string FirstName { get; set; }

        //[Required]
        //[Display(Name = "Last Name")]
        //public string LastName { get; set; }

        //[Required]
        //[Display(Name = "Timezone")]
        //public string Timezone { get; set; }

        [Required]
        [Display(Name = "Verification Code")]
        public string VerificationCode { get; set; }

        //private IEnumerable<SelectListItem> _timeZoneOptions = new SelectListItem[0];

        //public IEnumerable<SelectListItem> TimeZoneOptions
        //{
        //    get
        //    {
        //        if (_timeZoneOptions.Any()) return _timeZoneOptions;
        //        _timeZoneOptions = TimeZoneInfo.GetSystemTimeZones().OrderBy(x => x.BaseUtcOffset)
        //            .Select(x => new SelectListItem() { Value = x.Id, Text = x.DisplayName, Selected = TimeZoneInfo.Local.Id == x.Id });

        //        return _timeZoneOptions;
        //    }
        //}
    }
}