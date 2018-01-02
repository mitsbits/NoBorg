using Borg.Infra.DTO;

namespace Borg.MVC.BuildingBlocks
{
    public class ModuleRenderer 
    {
        public string FriendlyName { get; set; }
        public string Summary { get; set; }
        public string ModuleGroup { get; set; }
        public Tiding[] Parameters { get; set; }
    }
}