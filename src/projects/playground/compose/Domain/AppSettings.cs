using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{


    public class AppSettings
    {
        public IDictionary<string, string> ConnectionStrings { get; set; }
        public IDictionary<string, string> EndPoints { get; set; }
    }

}
