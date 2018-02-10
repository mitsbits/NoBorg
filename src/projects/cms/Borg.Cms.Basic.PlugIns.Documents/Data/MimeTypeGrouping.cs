using Borg.Infra.DDD.Contracts;
using Borg.Platform.EF;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Borg.Cms.Basic.PlugIns.Documents.Data
{
    public class MimeTypeGroupingState : IEntity<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<MimeTypeGroupingExtensionState> Extensions { get; set; } = new HashSet<MimeTypeGroupingExtensionState>();
    }

    public class MimeTypeGroupingStateMap : EntityMap<MimeTypeGroupingState, DocumentsDbContext>
    {
        public override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<MimeTypeGroupingState>().HasKey(x => x.Id).ForSqlServerIsClustered();
            builder.Entity<MimeTypeGroupingState>().Property(x => x.Name).IsRequired().HasMaxLength(256).IsUnicode(true);
            builder.Entity<MimeTypeGroupingState>().Property(x => x.Description).IsRequired(false).HasDefaultValue("");
            builder.Entity<MimeTypeGroupingState>().HasIndex(x => x.Name).IsUnique(true).HasName("IX_MimeTypeGrouping_Name");
        }
    }
}