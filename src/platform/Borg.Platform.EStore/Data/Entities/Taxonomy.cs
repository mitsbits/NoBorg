using Borg.CMS;
using Borg.CMS.Components.Contracts;
using Borg.Platform.EF.Instructions;
using Borg.Platform.EF.Instructions.Attributes;
using Microsoft.EntityFrameworkCore;

namespace Borg.Platform.EStore.Data.Entities
{
    [TableSchemaDefinition("borg")]
    public partial class TaxonomyState : TaxonomyBase<int>
    {
        public override int Id { get; protected set; }
        public override int ParentId { get; protected set; }
        public override int[] HierarchyKeys { get; }
        public override IComponent<int> Component => ComponentState;
        public override string LanguageCode { get; protected set; }

        public  ComponentState ComponentState { get; protected set; }
    }

    public partial class ComponentState
    {
        internal virtual TaxonomyState TaxonomyState { get; set; }
    }

    public class TaxonomyMap : EntityMap<PriceListState, EStoreDbContext>
    {
        public override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<TaxonomyState>().HasKey(x => new { x.Id, x.LanguageCode }).ForSqlServerIsClustered();
            builder.Entity<TaxonomyState>().HasOne(x => x.ComponentState).WithOne(x => x.TaxonomyState).HasForeignKey<TaxonomyState>(x => x.Id);
        }
    }
}