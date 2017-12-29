using System.Collections.Generic;

namespace Borg.MVC.Services.Breadcrumbs
{
    public class Breadcrumbs : List<BreadcrumbItem>
    {
        public Breadcrumbs()
        {
        }

        public Breadcrumbs(BreadcrumbItem root)
        {
            Add(root);
        }
    }
}