using Borg.Infra.DDD.Contracts;
using Borg.Infra.Storage.Assets.Contracts;
using Borg.Platform.EF;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Borg.Platform.EF.Instructions;

namespace Borg.Platform.Documents.Data.Entities
{
    public class AssetState : IEntity<int>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<VersionState> Versions { get; set; } = new HashSet<VersionState>();

        public DocumentBehaviourState DocumentBehaviourState { get; set; }
        public int CurrentVersion { get; set; } = 0;
    }

    public class AssetStateMap : EntityMap<AssetState, DocumentsDbContext>
    {
        public override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasSequence<int>("AssetsSQC", "assets")
                .StartsAt(1)
                .IncrementsBy(1);

            builder.Entity<AssetState>().HasKey(x => x.Id).ForSqlServerIsClustered();
            builder.Entity<AssetState>().Property(x => x.Id).HasDefaultValueSql<int>("NEXT VALUE FOR assets.AssetsSQC");
            builder.Entity<AssetState>().Property(x => x.Name).HasMaxLength(512).IsRequired().HasDefaultValue<string>("");
            builder.Entity<AssetState>().Property(x => x.CurrentVersion).IsRequired().HasDefaultValueSql<int>("0");
            builder.Entity<AssetState>().Property(x => x.DocumentBehaviourState).IsRequired();
        }
    }
}