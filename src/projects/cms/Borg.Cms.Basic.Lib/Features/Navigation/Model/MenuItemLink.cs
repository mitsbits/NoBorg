using Borg.Infra.DTO;
using System.IO;
using System.Text.RegularExpressions;

namespace Borg.Cms.Basic.Lib.Features.Navigation.Model
{
    public class MenuItemLink : MenuItemBase
    {
        private static Regex httprgx = new Regex(@"^(http|https)://.*$");

        public MenuItemLink(Tiding source, string siteRoot) : base(source)
        {
            CalculateHref(siteRoot);
        }

        public override bool IsLink => true;
        private string _href = string.Empty;
        public override string Href => _href;

        private void CalculateHref(string siteRoot)
        {
            if (httprgx.IsMatch(_source.HumanKey)) { _href = _source.HumanKey; return; }
            _href = Path.Combine(siteRoot, _source.HumanKey.TrimStart("~/".ToCharArray()));
        }
    }
}