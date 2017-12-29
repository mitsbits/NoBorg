namespace Borg.MVC.Services.Breadcrumbs
{
    public abstract class BreadcrumbItem
    {
        protected BreadcrumbItem(string display, string url)
        {
            Display = display;
            Url = url;
        }

        public virtual string Display { get; }
        public virtual string Url { get; }
        public abstract BreadcrumbItemType BreadcrumbType { get; }
    }
}