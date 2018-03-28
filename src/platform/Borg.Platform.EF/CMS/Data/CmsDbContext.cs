using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Borg.Platform.EF.CMS.Data
{
    public class CmsDbContext : DbContext
    {
        private readonly Dictionary<Type, object> _cache;

        public CmsDbContext(DbContextOptions<CmsDbContext> option) : base(option)
        {
            _cache = new Dictionary<Type, object>();
        }

        public DbSet<ComponentState> ComponentStates { get; set; }
        public DbSet<TagState> TagStates { get; set; }
        public DbSet<HtmlSnippetState> HtmlSnippetStates { get; set; }
        public DbSet<ArticleState> ArticleStates { get; set; }
        public DbSet<ArticleTagState> ArticleTagStates { get; set; }
        public DbSet<TaxonomyState> TaxonomyStates { get; set; }
        public DbSet<NavigationItemState> NavigationItemStates { get; set; }
        public DbSet<DeviceState> DeviceStates { get; set; }
        public DbSet<SectionState> SectionStates { get; set; }
        public DbSet<ComponentDeviceState> ComponentDeviceStates { get; set; }
        public DbSet<PageMetadataState> PageMetadataStates { get; set; }
        public DbSet<SlotState> SlotStates { get; set; }
        public DbSet<ComponentDocumentAssociationState> ComponentDocumentAssociationStates { get; set; }
        public DbSet<CategoryGroupingState> CategoryGroupingStates { get; set; }
        public DbSet<CategoryState> CategoryStates { get; set; }
        public DbSet<CategoryComponentAssociationState> CategoryComponentAssociationStates { get; set; }
        public DbSet<InstanceBlockState> ConfigurationBlockStates { get; set; }

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
                entityType.Relational().Schema = "cms";
            }
        }
    }

    public class CmsDbContextFactory : IDesignTimeDbContextFactory<CmsDbContext>
    {
        private readonly string _dbConnKey = "db";

        public CmsDbContextFactory()
        {
        }

        CmsDbContext IDesignTimeDbContextFactory<CmsDbContext>.CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CmsDbContext>();
            optionsBuilder.UseSqlServer("Server=.\\d2016;Database=db;Trusted_Connection=True;MultipleActiveResultSets=true;", x => x.MigrationsHistoryTable("__MigrationsHistory", "cms"));

            return new CmsDbContext(optionsBuilder.Options);
        }
    }
}