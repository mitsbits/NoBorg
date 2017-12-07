using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Borg.MVC.TagHelpers.HtmlPager;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Timesheets.Web.Auth;
using Timesheets.Web.Domain;
using Web.Domain;

namespace Web.Features.Workers
{
    public class WorkerViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "Roles")]
        public string[] Roles { get; set; }
        [Display(Name = "Locked")]
        public bool Locked { get; set; }
        [Display(Name = "Lockout End")]
        [DataType(DataType.DateTime)]
        public DateTime LockoutEnd { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LaststName { get; set; }

        [Display(Name = "Team")]
        public string TeamId { get; set; }
    }


    public class WorkerRolesViewModel
    {
        public WorkerRolesViewModel()
        {
            RoleOptions = new Option[0];
        }
        public WorkerRolesViewModel(WorkerViewModel outer)
        {
            Email = outer.Email;
            RoleOptions =
                Enum.GetValues(typeof(Roles))
                    .Cast<Roles>()
                    .Where(x => x != Timesheets.Web.Auth.Roles.Employee)
                    .Select(x => new Option() { Key = x.ToString(), Selected = outer.Roles.Contains(x.ToString()) })
                    .ToArray();
        }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "Roles")]
        public string[] Roles { get; set; }
        [Display(Name = "Roles")]
        public Option[] RoleOptions { get; set; }

    }

    public class WorkerNameAndTeamViewModel
    {
        public WorkerNameAndTeamViewModel() { }

        public WorkerNameAndTeamViewModel(WorkerViewModel outer)
        {
            Email = outer.Email;
            FirstName = outer.FirstName;
            LaststName = outer.LaststName;
            Team = (TeamCoutries)Enum.Parse(typeof(TeamCoutries), outer.TeamId);
        }



        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string LaststName { get; set; }
        [Required]
        [Display(Name = "Team")]
        public TeamCoutries Team { get; set; }

        public IEnumerable<SelectListItem> TeamOptions()
        {
            return Enum.GetValues(typeof(TeamCoutries)).Cast<TeamCoutries>().Select(result => new SelectListItem() { Text = result.ToString(), Value = result.ToString(), Selected = result.Equals(Team) });
        }
    }


    public class Option
    {
        public string Key { get; set; }
        public bool Selected { get; set; }
    }
}
