using Borg.Cms.Basic.Lib.Features.Auth.Register;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Borg.Cms.Basic.Lib.Features.Auth.Data
{
    public class AuthDbContext : IdentityDbContext<CmsUser>
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options)
            : base(options)
        {
        }

        public DbSet<RegistrationRequest> RegistrationRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<RegistrationRequest>().HasKey(x => x.SK).ForSqlServerIsClustered();
            builder.Entity<RegistrationRequest>().Ignore(x => x.CompositeKey);
            builder.Entity<RegistrationRequest>().Property(x => x.Email).HasMaxLength(128).IsRequired();
            builder.Entity<RegistrationRequest>().Property(x => x.Id).HasMaxLength(128).IsRequired();
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                entityType.Relational().Schema = "auth";
            }
        }
    }
}