using Borg.CMS.Components.Contracts;
using Borg.Platform.EF.Instructions;
using Microsoft.EntityFrameworkCore;

namespace Borg.Platform.EStore.Data.Entities
{
    public partial class ComponentState : IComponent<int>
    {
        public int Id { get; set; }

        public void Delete()
        {
            if (IsDeleted) return;
            IsDeleted = true;
        }

        public bool IsDeleted { get; protected set; }
        public bool IsPublished { get; protected set; }

        public void Publish()
        {
            if (IsPublished) return;
            IsPublished = true;
        }

        public void Suspend()
        {
            if (!IsPublished) return;
            IsPublished = false;
        }
    }

    public class ComponentMap : EntityMap<ComponentState, EStoreDbContext>
    {
        public override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ComponentState>().HasKey(x => x.Id).ForSqlServerIsClustered();
        }
    }
}