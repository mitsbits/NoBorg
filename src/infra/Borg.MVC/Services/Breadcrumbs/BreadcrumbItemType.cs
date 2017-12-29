namespace Borg.MVC.Services.Breadcrumbs
{
    //Example
    //Breadcrumbs.Add(new BreadcrumbLabel("Network"));
    //Breadcrumbs.Add(new BreadcrumbLink("Recipients", Url.Action("index")));
    //

    public enum BreadcrumbItemType
    {
        Link = 1,
        Label = 2
    }
}