using System.Collections.Generic;
using Borg.MVC.TagHelpers.HtmlPager;

namespace Borg.MVC
{
   public class BorgSettings
    {
        public IDictionary<string, string> ConnectionStrings { get; set; }
        public PaginationInfoStyle PaginationInfoStyle { get; set; }
        public StorageSettings Storage { get; set; }

        public TenantSettings Tenant { get; set; }
    }

    public class StorageSettings
    {
        public string Folder { get; set; }
    }

    public class TenantSettings
    {
        public string ServiceTag { get; set; }
        public string Endpoint { get; set; }
    }
}
