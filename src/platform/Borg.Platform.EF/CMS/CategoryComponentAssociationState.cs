using Borg.Platform.EF.CMS.Data;
using Microsoft.EntityFrameworkCore;

namespace Borg.Platform.EF.CMS
{
    public class CategoryComponentAssociationState
    {
        public int CategoryId { get; set; }
        public int ComponentId { get; set; }
        public bool IsPrimary { get; set; }
        internal virtual ComponentState Component { get; set; }
        internal virtual CategoryState Category { get; set; }
    }

    public class CategoryComponentAssociationStateMap : EntityMap<CategoryComponentAssociationState, CmsDbContext>
    {
        public override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<CategoryComponentAssociationState>().HasKey(t => new { t.CategoryId, t.ComponentId }).ForSqlServerIsClustered();
            builder.Entity<CategoryComponentAssociationState>().HasOne(pt => pt.Component).WithMany(x => x.CategoryComponentAssociations).HasForeignKey(x => x.ComponentId).HasConstraintName("FK_Components_CategoryComponentAssociation");
            builder.Entity<CategoryComponentAssociationState>().HasOne(pt => pt.Category).WithMany(x => x.CategoryComponentAssociations).HasForeignKey(x => x.CategoryId).HasConstraintName("FK_Categories_CategoryComponentAssociation");
        }
    }
}