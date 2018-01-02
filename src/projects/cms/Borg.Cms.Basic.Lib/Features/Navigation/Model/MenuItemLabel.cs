using Borg.Infra.DTO;

namespace Borg.Cms.Basic.Lib.Features.Navigation.Model
{
    public class MenuItemLabel : MenuItemBase
    {
        public MenuItemLabel(Tiding source) : base(source)
        {
        }

        public override bool IsLink => false;
        public override string Href => string.Empty;
    }
}