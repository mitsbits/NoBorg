using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Borg.Infra.DDD.Contracts;
using Borg.Platform.EF.CMS.Data;
using Microsoft.EntityFrameworkCore;

namespace Borg.Platform.EF.CMS
{
    public class CategoryGroupingState : IEntity<int>
    {
        public int Id { get; set; }
        public virtual ComponentState Component { get; set; }
        public string FriendlyName { get; set; }
        public virtual ICollection<CategoryState> Categories { get; set; } = new HashSet<CategoryState>();
    }


    public class CategoryGroupingStateMap : EntityMap<CategoryGroupingState, CmsDbContext>
    {
        public override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<CategoryGroupingState>().HasKey(x =>x.Id).ForSqlServerIsClustered();
            builder.Entity<CategoryGroupingState>().Property(x => x.Id).ValueGeneratedNever();
            builder.Entity<CategoryGroupingState>().Property(x => x.FriendlyName).IsRequired().HasMaxLength(512)
                .IsUnicode().IsRequired().HasDefaultValue("");
            builder.Entity<CategoryGroupingState>().HasOne(x => x.Component).WithOne(x => x.CategoryGrouping).HasForeignKey<CategoryGroupingState>(x => x.Id);
        }
    }
}
