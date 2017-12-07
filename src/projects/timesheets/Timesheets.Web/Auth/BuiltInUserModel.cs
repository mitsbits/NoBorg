namespace Timesheets.Web.Auth
{
    public class BuiltInUserModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool Locked { get; set; } = false;
    }
}
