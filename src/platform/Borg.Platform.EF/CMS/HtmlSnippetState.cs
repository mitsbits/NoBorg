using Borg.Infra.DDD.Contracts;
using Borg.Platform.EF.CMS.Data;
using Borg.Platform.EF.Instructions;
using Microsoft.EntityFrameworkCore;

namespace Borg.Platform.EF.CMS
{
    public class HtmlSnippetState : IEntity<int>
    {
        public int Id { get; protected set; }
        public virtual ComponentState Component { get; set; }

        protected HtmlSnippetState() : base()
        {
        }

        public HtmlSnippetState(string code, string snippet, int id = 0) : this()
        {
            Code = code;
            HtmlSnippet = snippet;
            Id = id;
        }

        public string HtmlSnippet { get; set; }
        public string Code { get; set; }
    }

    public partial class ComponentState
    {
        internal HtmlSnippetState HtmlSnippet { get; set; }
    }

    public class HtmlSnippetStateMap : EntityMap<HtmlSnippetState, CmsDbContext>
    {
        public override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<HtmlSnippetState>().HasKey(x => x.Id).ForSqlServerIsClustered();
            builder.Entity<HtmlSnippetState>().Property(x => x.Id).ValueGeneratedNever();
            builder.Entity<HtmlSnippetState>().Property(x => x.HtmlSnippet).IsRequired().HasDefaultValue("").IsUnicode();
            builder.Entity<HtmlSnippetState>().Property(x => x.Code).HasMaxLength(1024).IsRequired().HasDefaultValue("").IsUnicode(false);
            builder.Entity<HtmlSnippetState>().HasIndex(x => x.Code).IsUnique(true).HasName("IX_HtmlSnippet_Code");
            builder.Entity<HtmlSnippetState>().HasOne(x => x.Component).WithOne(x => x.HtmlSnippet).HasForeignKey<HtmlSnippetState>(x => x.Id);
        }
    }
}