using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Timesheets.Web.ViewComponents
{
    public class BreadcrumpsViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {

            return View();
        }
    }
}
