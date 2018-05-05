using Borg.Platform.EF.Instructions;
using Borg.Platform.EStore.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace Borg.Platform.EStore.Data.Entities
{
    public class PriceState : Price<int>
    {
        public override int Id { get; protected set; }
        public int PriceListId { get; protected set; }
        public virtual PriceListState PriceListState { get; set; }
    }

    public class PriceMap : EntityMap<PriceState, EStoreDbContext>
    {
        public override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<PriceState>().HasKey(x => x.Id).ForSqlServerIsClustered();
            builder.Entity<PriceState>().HasOne(x => x.PriceListState).WithMany(x => x.PriceStates).HasForeignKey(x => x.PriceListId);
        }
    }
}