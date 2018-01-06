using Borg.Infra.Collections.Hierarchy;
using Borg.Infra.DDD;
using Borg.Infra.DTO;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Borg.Cms.Basic.Lib.Features.Content;

namespace Borg.Cms.Basic.Lib.Features.Navigation
{
    public class NavigationItemRecord : IHasParent<int>, IPublishable, IEntity<int>, IWeighted
    {
        public int Id { get; set; }
        public int Depth { get; protected set; }

        [DefaultValue(0)]
        [DisplayName("Parent")]
        [Required]
        public int ParentId { get; set; }

        [DisplayName("Display")]
        [Required]
        public string Display { get; set; }

        [DefaultValue("BSE")]
        [DisplayName("Group")]
        [Required]
        public string Group { get; set; }

        [DefaultValue("/")]
        [DisplayName("Path")]
        public string Path { get; set; }

        [DisplayName("Item Type")]
        [Required]
        public NavigationItemType ItemType { get; set; } = NavigationItemType.Label;

        [DefaultValue(true)]
        [DisplayName("Active")]
        [Required]
        public bool IsPublished { get; set; }

        public void Publish()
        {
            IsPublished = true;
        }

        public void Suspend()
        {
            IsPublished = false;
        }

        [DefaultValue(0)]
        [DisplayName("Weight")]
        public double Weight { get;  set; }
        [DisplayName("Content Item")]
        public int? ContentItemRecordId { get; set; }
        public virtual ContentItemRecord ContentItemRecord { get; set; }
    }
}