using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{


    public class AppSettings
    {
        public IDictionary<string, string> ConnectionStrings { get; set; }
        public IDictionary<string, string> EndPoints { get; set; }
        public RabbitMqSettings RabbitMq { get; set; }
    }


    public class RabbitMqSettings
    {
        public string Host { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

}
