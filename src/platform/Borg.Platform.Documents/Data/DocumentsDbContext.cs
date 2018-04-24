using Borg.Platform.Documents.Data.Entities;
using Borg.Platform.EF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Borg.Platform.Documents.Data
{
    public class DocumentsDbContext : DbContext
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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            var maptype = typeof(EntityMap<,>);

            var maps = GetType().Assembly.GetTypes().Where(t => t.IsSubclassOfRawGeneric(maptype) && !t.IsAbstract && t.BaseType.GenericTypeArguments[1] == GetType());

            foreach (var map in maps)
            {
                if (!_cache.ContainsKey(map))
                {
                    _cache.Add(map, Activator.CreateInstance(map) as IEntityMap);
                }
                ((IEntityMap)_cache[map]).OnModelCreating(builder);
            }

            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                entityType.Relational().Schema = "documents";
            }
        }
    }
}