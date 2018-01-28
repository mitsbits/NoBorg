using Borg.Infra.DTO;

namespace Borg.CMS.BuildingBlocks
{
    public class ModuleRenderer 
    {
        public string FriendlyName { get; set; }
        public string Summary { get; set; }
        public string ModuleGroup { get; set; }
        public Tiding[] Parameters { get; set; }
        public  string ModuleGender { get; set; }
    }
}