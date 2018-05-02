using Borg.Platform.EF.CMS.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Borg.Platform.EF.Instructions;

namespace Borg.Platform.EF.CMS
{
    public class ComponentDocumentAssociationState
    {
        public int ComponentId { get; set; }
        public int DocumentId { get; set; }
        public int FileId { get; set; }
        public int Version { get; set; }
        public string MimeType { get; set; }
        public string Uri { get; set; }
        public virtual ComponentState Component { get; set; }
    }

    public partial class ComponentState
    {
        internal ICollection<ComponentDocumentAssociationState> ComponentDocumentAssociations { get; set; } = new HashSet<ComponentDocumentAssociationState>();
    }

    public class ComponentDocumentAssociationStateMap : EntityMap<ComponentDocumentAssociationState, CmsDbContext>
    {
        public override void OnModelCreating(ModelBuilder builder) //.HasMaxLength(1024).IsRequired().HasDefaultValue("");
        {
            builder.Entity<ComponentDocumentAssociationState>().HasKey(t => new { t.ComponentId, t.DocumentId }).ForSqlServerIsClustered();
            builder.Entity<ComponentDocumentAssociationState>().Property(x => x.FileId).IsRequired();
            builder.Entity<ComponentDocumentAssociationState>().Property(x => x.Version).IsRequired();
            builder.Entity<ComponentDocumentAssociationState>().Property(x => x.MimeType).IsRequired().HasMaxLength(256).IsUnicode(true).HasDefaultValue("application/octet-stream");
            builder.Entity<ComponentDocumentAssociationState>().Property(x => x.Uri).HasMaxLength(1024).IsRequired().IsUnicode(true).HasDefaultValue("");

            builder.Entity<ComponentDocumentAssociationState>().HasIndex(x => x.FileId);
            builder.Entity<ComponentDocumentAssociationState>().HasIndex(x => x.Version);

            builder.Entity<ComponentDocumentAssociationState>().HasOne(x => x.Component)
                .WithMany(x => x.ComponentDocumentAssociations).HasForeignKey(x => x.ComponentId)
                .HasConstraintName("FK_Components_Documents");
        }
    }
}