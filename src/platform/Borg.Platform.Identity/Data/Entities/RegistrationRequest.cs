using Borg.Infra.DDD.Contracts;
using Borg.Platform.EF;
using Microsoft.EntityFrameworkCore;
using System;

namespace Borg.Platform.Identity.Data.Entities
{
    public class RegistrationRequest : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Email { get; set; }
        public DateTimeOffset SubmitedOn { get; set; }
    }

    public class RegistrationRequestMap : EntityMap<RegistrationRequest, AuthDbContext>
    {
        public override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<RegistrationRequest>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
            builder.Entity<RegistrationRequest>().HasKey(x => x.Id).ForSqlServerIsClustered();
            builder.Entity<RegistrationRequest>().Property(x => x.Email).HasMaxLength(128).IsRequired();
            builder.Entity<RegistrationRequest>().HasIndex(x => x.Email).HasName("IX_RegistrationRequest_Email");
            builder.Entity<RegistrationRequest>().Property(x => x.Code).HasMaxLength(128).IsRequired();
            builder.Entity<RegistrationRequest>().HasIndex(x => x.Code).HasName("IX_RegistrationRequest_Code");
        }
    }
}