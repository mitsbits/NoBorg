using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;

namespace Timesheets.Web.Domain
{
    public class TimesheetsDbContext : DbContext
    {
        protected TimesheetsDbContext()
        {
        }

        public TimesheetsDbContext(DbContextOptions<TimesheetsDbContext> options)
            : base(options)
        {
        }

        public DbSet<Team> Teams { get; set; }
        public DbSet<Worker> Workers { get; set; }
        public DbSet<WorkingDay> WorkingDays { get; set; }
        public DbSet<BankHoliday> BankHolidays { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Taxonomy> Taxonomies { get; set; }
        public DbSet<TaxonomyTag> TaxonomiesTags { get; set; }
        public DbSet<AspUser> AspUsers { get; set; }
        public DbSet<AspUserRole> AspUserRoles { get; set; }
        public DbSet<AspRole> AspRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<WorkingDay>().ToTable("WorkingDays");
            builder.Entity<BankHoliday>().ToTable("BankHolidays");
            builder.Entity<Tag>().ToTable("Tags");
            builder.Entity<Taxonomy>().ToTable("Taxonomies").Ignore(e => e.IsRoot);
            builder.Entity<TaxonomyTag>().ToTable("TaxonomiesTags").HasKey(x => new { x.TaxonomyId, x.TagId });
            ConfigureTeams(builder);
            ConfigureAspEntities(builder);
        }

        private static void ConfigureAspEntities(ModelBuilder builder)
        {
            builder.Entity<AspUserRole>().HasKey(x => new { x.UserId, x.RoleId });
            builder.Entity<AspUser>().ToTable("AspNetUsers");
            builder.Entity<AspUserRole>().ToTable("AspNetUserRoles");
            builder.Entity<AspRole>().ToTable("AspNetRoles");
        }

        private static void ConfigureTeams(ModelBuilder builder)
        {
            builder.Entity<Team>().HasIndex(x => x.Id).IsUnique();
            builder.Entity<Team>().Property(x => x.Id).HasMaxLength(32);
        }
    }

    public class TimesheetsDbContextFactory : IDesignTimeDbContextFactory<TimesheetsDbContext>
    {
        ////////
        public TimesheetsDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<TimesheetsDbContext>();
            var path = AppDomain.CurrentDomain.BaseDirectory.Substring(0, AppDomain.CurrentDomain.BaseDirectory.IndexOf("bin", StringComparison.Ordinal));
            IConfigurationRoot configuration = new ConfigurationBuilder()
              .SetBasePath(path)
              .AddJsonFile("config.json")
              .Build();

            builder.UseSqlServer(configuration["ConnectionStrings:TmesheetsSql"]);
            return new TimesheetsDbContext(builder.Options);
        }
    }
}