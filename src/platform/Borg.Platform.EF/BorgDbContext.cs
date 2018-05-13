using Borg.Infra.Services.Factory;
using Borg.Platform.EF.Instructions;
using Borg.Platform.EF.Instructions.Attributes;
using EntityFrameworkCore.Triggers;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Borg.Platform.EF
{
    public abstract class BorgDbContext : DbContext
    {
        protected BorgDbContext([NotNull] DbContextOptions options) : base(options)
        {
        }

        protected virtual string SchemaName => GetType().Name.Replace("DbContext", string.Empty).Slugify();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            var maptype = typeof(EntityMap<,>);
            var maps = GetType().Assembly.GetTypes().Where(t => t.IsSubclassOfRawGeneric(maptype) && !t.IsAbstract && t.BaseType.GenericTypeArguments[1] == GetType());
            foreach (var map in maps)
            {
                ((IEntityMap)New.Creator(map)).OnModelCreating(builder);
            }
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                var t = entityType.ClrType;
                if (t != null)
                {
                    var attr = t.GetCustomAttribute<TableSchemaDefinitionAttribute>();
                    entityType.Relational().Schema = attr != null ? attr.Schema.Slugify() : SchemaName;
                }
                else
                {
                    entityType.Relational().Schema = SchemaName;
                }
            }
        }

        #region If you're targeting EF Core

        public override Int32 SaveChanges()
        {
            return this.SaveChangesWithTriggers(base.SaveChanges, acceptAllChangesOnSuccess: true);
        }

        public override Int32 SaveChanges(Boolean acceptAllChangesOnSuccess)
        {
            return this.SaveChangesWithTriggers(base.SaveChanges, acceptAllChangesOnSuccess);
        }

        public override Task<Int32> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.SaveChangesWithTriggersAsync(base.SaveChangesAsync, acceptAllChangesOnSuccess: true, cancellationToken: cancellationToken);
        }

        public override Task<Int32> SaveChangesAsync(Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.SaveChangesWithTriggersAsync(base.SaveChangesAsync, acceptAllChangesOnSuccess, cancellationToken);
        }

        #endregion If you're targeting EF Core
    }
}