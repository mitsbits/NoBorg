using Domain.Messages.Contracts;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace WebApi.Controllers
{
    [Route("v1/[controller]")]
    public class TopicsController : Controller
    {
        private IBus _bus;

        public TopicsController(IBus bus)
        {
            _bus = bus;
        }

        // GET api/values/5
        [HttpGet("")]
        public string[] Get()
        {
            var list = new List<string>();
            for (int i = 0; i < 100; i++)
            {
                list.Add($"topic-{i:000}");
            }
            return list.ToArray();
        }

        [HttpPost("")]
        public IActionResult Create([FromBody] CreateTopic topic)
        {
            return BadRequest();
        }
    }
}