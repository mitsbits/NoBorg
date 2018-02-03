using Borg.Platform.EF;
using Microsoft.EntityFrameworkCore;

namespace Borg.Cms.Basic.PlugIns.Documents.Data
{
    public class DocumentOwnerState
    {
        public int DocumentId { get; set; }
        public string Owner { get; set; }
    }

    public class DocumentOwnerStateMap : EntityMap<DocumentOwnerState, DocumentsDbContext>
    {
        public override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<DocumentOwnerState>().HasKey(x => new { DoecumentId = x.DocumentId, x.Owner }).ForSqlServerIsClustered();
            builder.Entity<DocumentOwnerState>().Property(x => x.DocumentId).ValueGeneratedNever();
            builder.Entity<DocumentOwnerState>().Property(x => x.Owner).HasMaxLength(256).IsUnicode(false);
        }
    }
}