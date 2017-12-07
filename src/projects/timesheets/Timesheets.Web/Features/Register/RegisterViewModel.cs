using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Timesheets.Web.Domain;

namespace Timesheets.Web.Features.Register
{
    public class RegisterViewModel
    {

        private static readonly IDictionary<string, TimeZoneInfo> _timeZoneInfos;

        static RegisterViewModel()
        {
            _timeZoneInfos = TimeZoneInfo.GetSystemTimeZones().ToDictionary(x => x.Id, x => x);
        }


        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required][Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required]
        [Display(Name = "Team")]
        public TeamCoutries Team { get; set; }

        public string ReturnUrl { get; set; }


        public IEnumerable<SelectListItem> TimeZoneOptions
        {
            get
            {
               return _timeZoneInfos.Keys.Select(x => new SelectListItem() {Value = x, Text = _timeZoneInfos[x].DisplayName, Selected = TimeZoneInfo.Local.Id == _timeZoneInfos[x].Id }).OrderBy(x=>x.Value).ToList();
            }
        }
    }
}
