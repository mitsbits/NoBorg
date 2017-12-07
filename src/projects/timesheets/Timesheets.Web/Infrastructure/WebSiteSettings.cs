namespace Timesheets.Web.Infrastructure
{
    public class WebSiteSettings
    {
        public ConnectionStrings ConnectionStrings { get; set; }
    }

    public class ConnectionStrings
    {
        public string AuthSql { get; set; }
        public string TmesheetsSql { get; set; }
    }
}