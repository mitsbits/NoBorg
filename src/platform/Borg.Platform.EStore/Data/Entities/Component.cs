using Borg.CMS;
using Borg.Platform.EF.Instructions;
using Borg.Platform.EF.Instructions.Attributes;
using Microsoft.EntityFrameworkCore;

namespace Borg.Platform.EStore.Data.Entities
{
    [TableSchemaDefinition("borg")]
    public partial class ComponentState : ComponentBase<int>
    {
        public override int Id { get; protected set; }
    }

    public class ComponentMap : EntityMap<ComponentState, EStoreDbContext>
    {
        public override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ComponentState>().HasKey(x => x.Id).ForSqlServerIsClustered();
        }
    }
}