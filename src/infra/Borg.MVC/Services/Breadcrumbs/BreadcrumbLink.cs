namespace Borg.MVC.Services.Breadcrumbs
{
    public sealed class BreadcrumbLink : BreadcrumbItem
    {
        public BreadcrumbLink(string display, string url) : base(display, url)
        {
        }

        public override BreadcrumbItemType BreadcrumbType => BreadcrumbItemType.Link;
    }
}