using Borg.CMS.Components.Contracts;
using Borg.Platform.EF.Instructions;
using Borg.Platform.EStore.Abstraction;
using Borg.Platform.EStore.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using EntityFrameworkCore.Triggers;

namespace Borg.Platform.EStore.Data.Entities
{
    public class PriceListState : PriceList<int>
    {
        public override IComponent<int> Component => ComponentState;
        public override IEnumerable<IPrice> Prices => PriceStates;

        public ComponentState ComponentState { get; set; }

        public ICollection<PriceState> PriceStates { get; set; }
    }

    public partial class ComponentState
    {
        internal virtual PriceListState PriceListState { get; set; }
    }

    public class PriceListMap : EntityMap<PriceListState, EStoreDbContext>
    {
        public override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<PriceListState>().HasKey(x =>  x.Id ).ForSqlServerIsClustered();
            builder.Entity<PriceListState>().HasOne(x => x.ComponentState).WithOne(x => x.PriceListState).HasForeignKey<PriceListState>(x => x.Id);

        }
    }
}