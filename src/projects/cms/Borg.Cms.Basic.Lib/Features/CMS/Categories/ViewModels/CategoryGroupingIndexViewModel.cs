using Borg.Cms.Basic.Lib.Features.CMS.Categories.Commands;
using Borg.Infra.Collections;
using Borg.Platform.EF.CMS;

namespace Borg.Cms.Basic.Lib.Features.CMS.Categories.ViewModels
{
    public class CategoryGroupingIndexViewModel
    {
        public AddOrUpdateCategoryGroupingCommand CreateCommand => new AddOrUpdateCategoryGroupingCommand();
        public IPagedResult<CategoryGroupingState> Index { get; set; }
    }
}