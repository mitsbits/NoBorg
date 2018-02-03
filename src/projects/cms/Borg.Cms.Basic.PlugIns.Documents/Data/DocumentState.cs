using Borg.Infra.DDD.Contracts;
using Borg.Platform.EF;
using Microsoft.EntityFrameworkCore;

namespace Borg.Cms.Basic.PlugIns.Documents.Data
{
    public class DocumentState : IEntity<int>
    {
        public int Id { get; set; }
    }

    public class DocumentStateMap : EntityMap<DocumentState, DocumentsDbContext>
    {
        public override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<DocumentState>().HasKey(x => x.Id).ForSqlServerIsClustered();
            builder.Entity<DocumentState>().Property(x => x.Id).ValueGeneratedNever();
        }
    }
}