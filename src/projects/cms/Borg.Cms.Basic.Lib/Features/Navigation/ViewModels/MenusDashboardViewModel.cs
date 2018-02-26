using Borg.Cms.Basic.Lib.Features.CMS.Commands;
using Borg.CMS;
using Borg.Infra.DTO;
using Borg.Infra.Services;
using Borg.Platform.EF.CMS;
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
        public IEnumerable<NavigationItemState> Records { get; set; }

        [DisplayName("Menu item")]
        public NavigationItemState SelectedState { get; set; }

        public IEnumerable<NavigationItemType> NavigaionTypeOptions => EnumUtil.GetValues<NavigationItemType>();

        public Tidings Trees() => Records.Trees();

        public IDictionary<(int, int), Tiding> TreeDictionary() => Trees().TreeDictionary();

        public IDictionary<(int, int), Tiding> TreeDictionaryExcludingCurrentAndChildren() =>
            Records.TreeDictionaryExcludingCurrentAndChildren(SelectedState);

        public NavigationItemStateCreateOrUpdateCommand GetCommand()
        {
            //if (SelectedState == null) SelectedState = new NavigationItemState();
            var result = new NavigationItemStateCreateOrUpdateCommand()
            {
                RecordId = SelectedState?.Id ?? default(int),
                Path = SelectedState?.Path ?? "",
                Display = SelectedState?.Display,
                Group = Group,
                IsPublished = SelectedState?.Component.IsPublished ?? false,
                IsDeleted = SelectedState?.Component.IsDeleted ?? false,
                ItemType = SelectedState?.NavigationItemType ?? NavigationItemType.Label,
                ParentId = SelectedState?.Taxonomy.ParentId ?? 0,
                ParentOptions = TreeDictionaryExcludingCurrentAndChildren(),
                Weight = SelectedState?.Taxonomy.Weight ?? 0,
            };
            return result;
        }
    }
}