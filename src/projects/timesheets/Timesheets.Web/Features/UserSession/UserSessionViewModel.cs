using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Features.UserSession
{
    public class UserSessionViewModel
    {
        public string UserIdentifier { get; set; }
        public string UserName { get; set; }
        public DateTimeOffset SessionStart { get; set; }
        [Display(Name = "Menu collapsed")]
        public bool MenuIsCollapsed { get; set; }
        [Display(Name = "Rows Per Page")]
        public int RowsPerPage { get; set; }
        public string RedirectUrl { get; set; }
    }
}
