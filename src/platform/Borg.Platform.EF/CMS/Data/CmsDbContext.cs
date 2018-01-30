using Borg.CMS.BuildingBlocks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Borg.Platform.EF.CMS.Data
{
    public class CmsDbContext : DbContext
    {
        public CmsDbContext(DbContextOptions<CmsDbContext> option) : base(option)
        {
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
        
        public DbSet<SlotState> SlotStates { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasSequence<int>("ComponentStatesSQC", "cms")
                .StartsAt(1)
                .IncrementsBy(1);

            builder.Entity<ComponentState>().HasKey(x => x.Id).ForSqlServerIsClustered();
            builder.Entity<ComponentState>().Property(x => x.Id).HasDefaultValueSql("NEXT VALUE FOR cms.ComponentStatesSQC");

            builder.Entity<TagState>().HasKey(x => x.Id).ForSqlServerIsClustered();
            builder.Entity<TagState>().Property(x => x.Id).ValueGeneratedNever();
            builder.Entity<TagState>().Property(x => x.Tag).HasMaxLength(1024).IsRequired().HasDefaultValue("").IsUnicode();
            builder.Entity<TagState>().Property(x => x.TagNormalized).HasMaxLength(1024).IsRequired().HasDefaultValue("").IsUnicode();
            builder.Entity<TagState>().Property(x => x.TagSlug).HasMaxLength(1024).IsRequired().HasDefaultValue("").IsUnicode(false);
            builder.Entity<TagState>().HasIndex(x => x.Tag).IsUnique(true).HasName("IX_Tag_Tag");
            builder.Entity<TagState>().HasIndex(x => x.TagNormalized).IsUnique(true).HasName("IX_Tag_TagNormalized");
            builder.Entity<TagState>().HasOne(x => x.Component).WithOne(x => x.Tag).HasForeignKey<TagState>(x => x.Id);

            builder.Entity<HtmlSnippetState>().HasKey(x => x.Id).ForSqlServerIsClustered();
            builder.Entity<HtmlSnippetState>().Property(x => x.Id).ValueGeneratedNever();
            builder.Entity<HtmlSnippetState>().Property(x => x.HtmlSnippet).IsRequired().HasDefaultValue("").IsUnicode();
            builder.Entity<HtmlSnippetState>().Property(x => x.Code).HasMaxLength(1024).IsRequired().HasDefaultValue("").IsUnicode(false);
            builder.Entity<HtmlSnippetState>().HasIndex(x => x.Code).IsUnique(true).HasName("IX_HtmlSnippet_Code");
            builder.Entity<HtmlSnippetState>().HasOne(x => x.Component).WithOne(x => x.HtmlSnippet).HasForeignKey<HtmlSnippetState>(x => x.Id);

            builder.Entity<ArticleState>().HasKey(x => x.Id).ForSqlServerIsClustered();
            builder.Entity<ArticleState>().Property(x => x.Id).ValueGeneratedNever();
            builder.Entity<ArticleState>().Property(x => x.Title).IsRequired().HasMaxLength(1024).HasDefaultValue("").IsUnicode();
            builder.Entity<ArticleState>().Property(x => x.Slug).IsRequired().HasMaxLength(1024).HasDefaultValue("").IsUnicode(false);
            builder.Entity<ArticleState>().Property(x => x.Slug).IsRequired().HasDefaultValue("").IsUnicode();
            builder.Entity<ArticleState>().HasIndex(x => x.Title).IsUnique(false).HasName("IX_Article_Title");
            builder.Entity<ArticleState>().HasOne(x => x.Component).WithOne(x => x.Article).HasForeignKey<ArticleState>(x => x.Id);

            builder.Entity<ArticleTagState>().HasKey(t => new { t.ArticleId, t.TagId }).ForSqlServerIsClustered();
            builder.Entity<ArticleTagState>().HasOne(pt => pt.Article).WithMany("ArticleTags").OnDelete(DeleteBehavior.ClientSetNull);
            builder.Entity<ArticleTagState>().HasOne(pt => pt.Tag).WithMany("ArticleTags").OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<TaxonomyState>().HasKey(x => x.Id).ForSqlServerIsClustered();
            builder.Entity<TaxonomyState>().Property(x => x.Id).ValueGeneratedNever();
            builder.Entity<TaxonomyState>().Property(x => x.ParentId).IsRequired().HasDefaultValue(0);
            builder.Entity<TaxonomyState>().HasOne(x => x.Article).WithOne(a => a.Taxonomy);
            builder.Entity<TaxonomyState>().HasIndex(x => x.ArticleId).IsUnique(true).HasName("IX_Taxonomy_ArticleId");
            builder.Entity<TaxonomyState>().HasIndex(x => x.ParentId).IsUnique(false).HasName("IX_Taxonomy_ParentId");
            builder.Entity<TaxonomyState>().HasOne(x => x.Component).WithOne(x => x.Taxonomy).HasForeignKey<TaxonomyState>(x => x.Id);

            builder.Entity<NavigationItemState>().HasKey(x => x.Id).ForSqlServerIsClustered();
            builder.Entity<NavigationItemState>().Property(x => x.Id).ValueGeneratedNever();
            builder.Entity<NavigationItemState>().Property(x => x.GroupCode).IsRequired().HasMaxLength(256).HasDefaultValue("");
            builder.Entity<NavigationItemState>().HasIndex(x => x.GroupCode).IsUnique(false).HasName("IX_Navigation_GroupCode");
            builder.Entity<NavigationItemState>().HasOne(x => x.Taxonomy).WithOne(x => x.NavigationItem).HasForeignKey<NavigationItemState>(x => x.Id);
            builder.Entity<NavigationItemState>().Ignore(x => x.Component);
            builder.Entity<NavigationItemState>().Ignore(x => x.Article);

            builder.Entity<DeviceState>().HasKey(x => x.Id).ForSqlServerIsClustered();
            builder.Entity<DeviceState>().Property(x => x.FriendlyName).HasMaxLength(512).IsRequired().HasDefaultValue("");
            builder.Entity<DeviceState>().Property(x => x.RenderScheme).HasMaxLength(512).IsRequired().HasDefaultValue(DeviceRenderScheme.UnSet);
            builder.Entity<DeviceState>().Property(x => x.Layout).HasMaxLength(512).IsRequired().HasDefaultValue("");
            builder.Entity<DeviceState>().Property(x => x.Theme).HasMaxLength(256).IsUnicode(false).IsRequired(false).HasDefaultValue("");

            builder.Entity<SectionState>().HasKey(x => x.Id).ForSqlServerIsClustered();
            builder.Entity<SectionState>().HasIndex(x => x.DeviceId).HasName("IX_Section_DeviceId");
            builder.Entity<SectionState>().HasIndex(x => x.Identifier).HasName("IX_Section_Identifier");
            builder.Entity<SectionState>().HasOne(p => p.Device).WithMany(b => b.Sections)
                .HasForeignKey(p => p.DeviceId).HasConstraintName("FK_Device_Section");
            builder.Entity<SectionState>().Property(x => x.FriendlyName).HasMaxLength(512).IsRequired().HasDefaultValue("");
            builder.Entity<SectionState>().Property(x => x.Identifier).HasMaxLength(512).IsRequired().HasDefaultValue("");

            builder.Entity<SlotState>().HasKey(x => x.Id).ForSqlServerIsClustered();
            builder.Entity<SlotState>().HasIndex(x => x.SectionId).HasName("IX_Slot_SectionId");
            builder.Entity<SlotState>().HasIndex(x => x.Ordinal).HasName("IX_Slot_Ordinal");
            builder.Entity<SlotState>().HasIndex(x => x.ModuleTypeName).HasName("IX_Slot_ModuleTypeName");
            builder.Entity<SlotState>().HasIndex(x => x.ModuleGender).HasName("IX_Slot_ModuleGender");
            builder.Entity<SlotState>().HasOne(p => p.Section).WithMany(b => b.Slots)
                .HasForeignKey(p => p.SectionId).HasConstraintName("FK_Section_Slot");
            builder.Entity<SlotState>().Property(x => x.ModuleDecriptorJson).IsRequired().HasDefaultValue("");
            builder.Entity<SlotState>().Property(x => x.ModuleGender).IsRequired().HasMaxLength(64).HasDefaultValue("");
            builder.Entity<SlotState>().Property(x => x.ModuleTypeName).IsRequired().HasMaxLength(1024).HasDefaultValue("");

            builder.Entity<ComponentDeviceState>().HasKey(t => new { t.ComponentId, t.DeviceId }).ForSqlServerIsClustered();
            builder.Entity<ComponentDeviceState>().HasOne(pt => pt.Component).WithOne(x => x.ComponentDevice);
            builder.Entity<ComponentDeviceState>().HasOne(pt => pt.Device).WithOne(x => x.ComponentDevice);

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