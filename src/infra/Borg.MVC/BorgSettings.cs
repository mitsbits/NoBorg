using Borg.MVC.TagHelpers.HtmlPager;

namespace Borg.MVC
{
   public class BorgSettings
    {
        public PaginationInfoStyle PaginationInfoStyle { get; set; }
        public StorageSettings Storage { get; set; }
    }

    public class StorageSettings
    {
        public string Folder { get; set; }
    }
}
