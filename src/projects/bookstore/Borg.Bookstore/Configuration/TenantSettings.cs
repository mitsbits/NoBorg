using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Borg.Bookstore.Configuration
{
    public class SiteSettings
    {
        public string ApplicationName { get; set; }
        public string ApplicationEndpoint { get; set; }
        public string SqlConnectionString { get; set; }
    }
}
