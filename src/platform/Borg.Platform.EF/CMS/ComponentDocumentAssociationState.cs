using System;
using System.Collections.Generic;
using System.Text;
using Borg.Platform.EF.CMS.Data;
using Microsoft.EntityFrameworkCore;

namespace Borg.Platform.EF.CMS
{
   public class ComponentDocumentAssociationState
    {
        public int ComponentId { get; set; }
        public int DocumentId { get; set; }
        public int FileId { get; set; }
        public int Version { get; set; }
        public virtual ComponentState Component { get; set; }
    }


    public class ComponentDocumentAssociationStateMap : EntityMap<ComponentDocumentAssociationState, CmsDbContext>
    {
        public override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ComponentDocumentAssociationState>().HasKey(t => new { t.ComponentId, t.DocumentId }).ForSqlServerIsClustered();
            builder.Entity<ComponentDocumentAssociationState>().Property(x => x.FileId).IsRequired();
            builder.Entity<ComponentDocumentAssociationState>().Property(x => x.Version).IsRequired();
            builder.Entity<ComponentDocumentAssociationState>().HasIndex(x => x.FileId);
            builder.Entity<ComponentDocumentAssociationState>().HasIndex(x => x.Version);
            builder.Entity<ComponentDocumentAssociationState>().HasOne(x => x.Component)
                .WithMany(x => x.ComponentDocumentAssociations).HasForeignKey(x => x.ComponentId)
                .HasConstraintName("FK_Components_Documents");
        }
    }
}
