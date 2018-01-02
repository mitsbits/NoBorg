using Borg.Infra.DTO;

namespace Borg.Cms.Basic.Lib.Features.Navigation.Model
{
    public abstract class MenuItemBase
    {
        protected readonly Tiding _source;

        protected MenuItemBase(Tiding source)
        {
            _source = source;
            Display = source.Value;
        }

        public abstract bool IsLink { get; }
        public abstract string Href { get; }
        public string Display { get; set; }
    }
}