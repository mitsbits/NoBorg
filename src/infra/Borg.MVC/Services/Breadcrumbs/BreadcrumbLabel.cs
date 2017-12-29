namespace Borg.MVC.Services.Breadcrumbs
{
    public sealed class BreadcrumbLabel : BreadcrumbItem
    {
        public BreadcrumbLabel(string display) : base(display, string.Empty)
        {
        }

        public override BreadcrumbItemType BreadcrumbType => BreadcrumbItemType.Label;
    }
}