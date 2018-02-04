using Borg.Infra.DDD.Contracts;
using Borg.Platform.EF;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Borg.Cms.Basic.PlugIns.Documents.Data
{
    public class DocumentState : IEntity<int>
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsPublished { get; set; }
        internal virtual ICollection<DocumentOwnerState> Owners { get; set; }
        internal virtual ICollection<DocumentCheckOutState> CheckOuts { get; set; }
    }

    public class DocumentStateMap : EntityMap<DocumentState, DocumentsDbContext>
    {
        public override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<DocumentState>().HasKey(x => x.Id).ForSqlServerIsClustered();
            builder.Entity<DocumentState>().Property(x => x.Id).ValueGeneratedNever();
            builder.Entity<DocumentState>().Property(x => x.IsDeleted).IsRequired().HasDefaultValue(false);
            builder.Entity<DocumentState>().Property(x => x.IsPublished).IsRequired().HasDefaultValue(false);
        }
    }
}