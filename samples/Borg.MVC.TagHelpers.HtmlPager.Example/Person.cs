using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace Borg.MVC.TagHelpers.HtmlPager.Example
{
    public class Person
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Birthday { get; set; }
        public string Phone { get; set; }
    }





    public class PersonsService
    {
        private readonly IHostingEnvironment _env;
        public PersonsService(IHostingEnvironment env)
        {
            _env = env;
        }


        public IEnumerable<Person> Persons()
        {
            var path = Path.Combine(_env.WebRootPath, "persons.json");
            var context = File.ReadAllText(path);
            var datum = JsonConvert.DeserializeObject<List<Person>>(context);

            return datum as IEnumerable<Person>;
        }
    }
}
