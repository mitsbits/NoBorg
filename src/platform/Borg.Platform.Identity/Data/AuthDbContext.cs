using Borg.Infra.Services.Factory;
using Borg.Platform.EF;
using Borg.Platform.Identity.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Borg.Platform.EF.Instructions;
using Borg.Platform.EF.Instructions.Attributes;

namespace Borg.Platform.Identity.Data
{
    public class AuthDbContext : IdentityDbContext<GenericUser>
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }
        protected virtual string SchemaName => GetType().Name.Replace("DbContext", string.Empty).Slugify();

        public DbSet<RegistrationRequest> RegistrationRequests { get; set; }

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
                    var atts = t.GetCustomAttributesData();
                    var attr = t.GetCustomAttribute<TableSchemaDefinitionAttribute>();
                    if (attr != null)
                    {
                        entityType.Relational().Schema = attr.Schema.Slugify();
                    }
                    else
                    {
                        entityType.Relational().Schema = SchemaName;
                    }
                }
                else
                {
                    entityType.Relational().Schema = SchemaName;
                }
               
            }
        }
    }
}