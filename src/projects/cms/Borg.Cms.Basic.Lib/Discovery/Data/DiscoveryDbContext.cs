using Borg.Cms.Basic.Lib.Discovery.Contracts;
using Borg.MVC;
using Borg.MVC.PlugIns.Contracts;
using Borg.Platform.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Borg.Cms.Basic.Lib.Discovery.Data
{
    public class DiscoveryDbContext : DbContext
    {
        public DiscoveryDbContext(DbContextOptions<DiscoveryDbContext> options) : base(options)
        {
        }

        internal IEnumerable<Type> EntityTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            var scope = BorgStartUp.ServiceProvider.CreateScope();
            var provider = scope.ServiceProvider;
            var host = provider.GetService<IPlugInHost>();
            var plugins = host.SpecifyPlugins<IPlugInEfEntityRegistration>();
            foreach (var plugInEfRegistration in plugins)
            {
                foreach (var entity in plugInEfRegistration.Entities.Keys)
                {
                    var action = plugInEfRegistration.Entities[entity];
                    if (!action.Invoke(builder))
                    {
                        builder.Entity(entity);
                    }
                }
            }
            scope.Dispose();
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                entityType.Relational().Schema = "discovery";
            }
        }

        private void BuildAction(EntityTypeBuilder entityTypeBuilder)
        {
            throw new NotImplementedException();
        }
    }

    public class DiscoveryDbSeed
    {
        private readonly DiscoveryDbContext _db;

        public DiscoveryDbSeed(DiscoveryDbContext db)
        {
            _db = db;
        }

        public async Task EnsureUp()
        {
            await _db.Database.MigrateAsync();
        }
    }

    public class DiscoveryDbContextFactory : BorgDbContextFactory<DiscoveryDbContext>
    {
    }
}