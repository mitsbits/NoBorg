using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Borg.Platform.Identity
{
    public class AuthDbContext : IdentityDbContext<GenericUser>
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options)
            : base(options)
        {
        }

        public DbSet<RegistrationRequest> RegistrationRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
           
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                entityType.Relational().Schema = "auth";
            }
        }
    }
}
