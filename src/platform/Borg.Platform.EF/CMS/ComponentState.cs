using Borg.CMS.Components.Contracts;
using Borg.Infra.DDD.Contracts;
using Borg.Platform.EF.CMS.Data;
using Microsoft.EntityFrameworkCore;

namespace Borg.Platform.EF.CMS
{
    public partial class ComponentState : IEntity<int>, ICanBeDeleted, ICanBePublished, ICanDeletedAndRecovered
    {
        public ComponentState()
        {
            
        }
        public ComponentState(int id):this()
        {
            Id = id;
        }
        public ComponentState(int id, bool isPublished) : this(id)
        {
            IsPublished = isPublished;
        }
        public ComponentState( bool isPublished) : this()
        {
            IsPublished = isPublished;
        }
        public int Id { get; protected set; }
        public bool IsDeleted { get;protected set; }


        public bool IsPublished { get; protected set; }
        public void Publish()
        {
            if (!IsPublished) IsPublished = true;
        }

        public void Suspend()
        {
            if (IsPublished) IsPublished = false;
        }

        public void Delete()
        {
          if(!IsDeleted)  IsDeleted = true;
        }
        public void Recover()
        {
            if (IsDeleted) IsDeleted = false;
        }
        public bool OkToDisplay() => !IsDeleted && IsPublished;

    }

    public class ComponentStateMap : EntityMap<ComponentState, CmsDbContext>
    {
        public override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasSequence<int>("ComponentStatesSQC", "cms")
                .StartsAt(1)
                .IncrementsBy(1);

            builder.Entity<ComponentState>().HasKey(x => x.Id).ForSqlServerIsClustered();
            builder.Entity<ComponentState>().Property(x => x.Id).HasDefaultValueSql("NEXT VALUE FOR cms.ComponentStatesSQC");
        }
    }
}