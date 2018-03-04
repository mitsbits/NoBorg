using Borg.Infra.DTO;
using System.Collections.Generic;
using Borg.Platform.EF.CMS;

namespace Borg.Cms.Basic.Lib.Features.CMS.Categories.ViewModels
{
    public class CategoryEditViewModel
    {
        public int RecordId { get; set; } = 0;
        public string FriendlyName { get; set; }
        public string Slug { get; set; }
        public int GroupingId { get; set; } = 0;
        public int ParentId { get; set; } = 0;
        public double Weight { get; set; } = 0;
        public bool IsTransient => RecordId == default(int);
        public IDictionary<(int, int), Tiding> ParentOptions { get; set; }
        public bool AlsoSetSlug { get; set; } = false;
        public ComponentState Component { get; set; }
    }
}