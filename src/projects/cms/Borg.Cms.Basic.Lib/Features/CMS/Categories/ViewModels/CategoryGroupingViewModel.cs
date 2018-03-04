using System.Collections.Generic;
using Borg.Platform.EF.CMS;
using System.Linq;
using Borg.Infra.DTO;

namespace Borg.Cms.Basic.Lib.Features.CMS.Categories.ViewModels
{
    public class CategoryGroupingViewModel
    {
        public CategoryGroupingState AggregateRoot { get; set; }
        public int SelectedCategoryId { get; set; } = default(int);

        public CategoryEditViewModel CategoryEditViewModel()
        {
            if (HasCategorySelected)
            {
                var hit = AggregateRoot.Categories.Single(x => x.Id == SelectedCategoryId);
                return new CategoryEditViewModel()
                {
                    ParentId = hit.Taxonomy.ParentId,
                    FriendlyName = hit.FriendlyName,
                    Slug = hit.Slug,
                    RecordId = hit.Id,
                    GroupingId = hit.GroupingId,
                    Weight = hit.Taxonomy.Weight,
                    ParentOptions = TreeDictionaryExcludingCurrentAndChildren(),
                    Component = hit.Component
                };
            }

            return new CategoryEditViewModel()
            {
                GroupingId = AggregateRoot.Id,
                ParentOptions =  TreeDictionary()
            };
        }

        private bool HasCategorySelected => SelectedCategoryId > 0;

        public Tidings Trees() => AggregateRoot.Categories.Trees();

        public IDictionary<(int, int), Tiding> TreeDictionary() => Trees().TreeDictionary();

        public IDictionary<(int, int), Tiding> TreeDictionaryExcludingCurrentAndChildren() =>
            AggregateRoot.Categories.TreeDictionaryExcludingCurrentAndChildren(SelectedState);


        public CategoryState SelectedState => AggregateRoot.Categories.FirstOrDefault(x => x.Id == SelectedCategoryId);
    }
}