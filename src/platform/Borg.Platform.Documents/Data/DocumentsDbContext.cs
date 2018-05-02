using Borg.Platform.Documents.Data.Entities;
using Borg.Platform.EF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Borg.Platform.EF.Instructions;

namespace Borg.Platform.Documents.Data
{
    public class DocumentsDbContext : BorgDbContext
    {
        private readonly Dictionary<Type, object> _cache;

        public DocumentsDbContext(DbContextOptions<DocumentsDbContext> option) : base(option)
        {
            _cache = new Dictionary<Type, object>();
        }

        public DbSet<DocumentState> DocumentStates { get; set; }
        public DbSet<DocumentOwnerState> DocumentOwnerStates { get; set; }
        public DbSet<DocumentCheckOutState> DocumentCheckOutStates { get; set; }
        public DbSet<MimeTypeGroupingState> MimeTypeGroupingStates { get; set; }
        public DbSet<MimeTypeGroupingExtensionState> MimeTypeGroupingExtensionStates { get; set; }
        public DbSet<FileState> FileRecords { get; set; }
        public DbSet<VersionState> VersionRecords { get; set; }
        public DbSet<AssetState> AssetRecords { get; set; }
        public DbSet<MimeTypeState> MimeTypeRecords { get; set; }


    }
}