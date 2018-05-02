using System;
using System.Collections.Generic;
using System.Text;
using Borg.CMS.Components.Contracts;
using Borg.Platform.EF.Instructions;
using Borg.Platform.EStore.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Borg.Platform.EStore.Data.Entities
{
    public class PriceListState : PriceList<int>
    {
        public override IComponent<int> Component => ComponentState;

        public  ComponentState ComponentState { get; set; }

    }

    public partial class ComponentState
    {
        internal PriceListState PriceList { get; set; }
    }


    public class PriceListMap : EntityMap<PriceListState, EStoreDbContext>
    {
        public override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<PriceListState>().HasKey(x => new { x.Id, x.LanguageCode }).ForSqlServerIsClustered();
            builder.Entity<PriceListState>().HasOne(x => x.ComponentState).WithOne(x => x.PriceList).HasForeignKey<PriceListState>(x=>x.Id);
        }
    }
}
