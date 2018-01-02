using Borg.Infra.DTO;
using Borg.Infra.ExtensionMethods;
using System.Collections.Generic;
using System.ComponentModel;

namespace Borg.Cms.Basic.Lib.Features.Navigation.ViewModels
{
    public class MenusDashboardViewModel
    {
        public string[] Groups { get; set; }
    }

    public class MenuViewModel
    {
        [DisplayName("Froup")]
        public string Group { get; set; }

        [DisplayName("Menu items")]
        public IEnumerable<NavigationItemRecord> Records { get; set; }

        [DisplayName("Menu item")]
        public NavigationItemRecord SelectedRecord { get; set; }

        public IEnumerable<NavigationItemType> NavigaionTypeOptions => EnumUtil.GetValues<NavigationItemType>();

        public Tidings Trees() => Records.Trees();

        public IDictionary<(int, int), Tiding> TreeDictionary() => Trees().TreeDictionary();

        public IDictionary<(int, int), Tiding> TreeDictionaryExcludingCurrentAndChildren() =>
            Records.TreeDictionaryExcludingCurrentAndChildren(SelectedRecord);
    }
}