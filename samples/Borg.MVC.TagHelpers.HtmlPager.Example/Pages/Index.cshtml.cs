using Borg.Infra.Collections;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;

namespace Borg.MVC.TagHelpers.HtmlPager.Example.Pages
{
    public class IndexModel : PageModel
    {
        private readonly PersonsService _service;
        public IndexModel(PersonsService service)
        {
            _service = service;
        }
        public IPagedResult<Person> Data { get; private set; }
        public void OnGet(int p = 1, int r = 10)
        {
            var data = _service.Persons();
            Data = new PagedResult<Person>(data.Skip(p - 1).Take(r), p, r, data.Count());

        }
    }
}
