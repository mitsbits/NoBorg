using Borg.Infra.DDD.Contracts;
using Borg.Platform.EF.CMS.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Borg.Platform.EF.CMS
{
    public class ComponentState : IEntity<int>
    {
        public int Id { get; protected set; }
        public bool IsDeleted { get; set; }
        public bool IsPublished { get; set; }

        internal virtual TagState Tag { get; set; }
        internal virtual HtmlSnippetState HtmlSnippet { get; set; }
        internal virtual ArticleState Article { get; set; }
        internal virtual TaxonomyState Taxonomy { get; set; }
        internal virtual ComponentDeviceState ComponentDevice { get; set; }
        internal virtual PageMetadataState PageMetadata { get; set; }
        internal virtual CategoryGroupingState CategoryGrouping { get; set; }
        internal virtual CategoryState Category { get; set; }

        internal virtual ICollection<ComponentDocumentAssociationState> ComponentDocumentAssociations { get; set; } = new HashSet<ComponentDocumentAssociationState>();
        internal virtual ICollection<CategoryComponentAssociationState> CategoryComponentAssociations { get; set; } = new HashSet<CategoryComponentAssociationState>();

        public bool OkToDisplay() => !IsDeleted && IsPublished;
    }

    public class ComponentStateMap : EntityMap<ComponentState, CmsDbContext>
    {
        public override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasSequence<int>("ComponentStatesSQC", "cms")
                .StartsAt(1)
                .IncrementsBy(1);

            builder.Entity<ComponentState>().HasKey(x => x.Id).ForSqlServerIsClustered();
            builder.Entity<ComponentState>().Property(x => x.Id).HasDefaultValueSql("NEXT VALUE FOR cms.ComponentStatesSQC");
        }
    }
}