using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("v1/[controller]")]
    public class TopicsController : Controller
    {
        // GET api/values/5
        [HttpGet("")]
        public string[] Get()
        {
            var list = new List<string>();
            for (int i = 0; i < 100; i++)
            {
                list.Add(  $"topic-{i:000}");

            }
            return list.ToArray();
        }
    }
}
