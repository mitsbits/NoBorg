using Borg.Infra.DDD.Contracts;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Borg.Platform.EF.CMS
{
    public class ArticleState : IEntity<int>
    {
        public int Id { get; protected set; }
        public virtual ComponentState Component { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Body { get; set; }
        internal virtual TaxonomyState Taxonomy { get; set; }
        private ICollection<ArticleTagState> ArticleTags { get; } = new List<ArticleTagState>();

        [NotMapped]
        public virtual IEnumerable<TagState> Tags => ArticleTags.Select(e => e.Tag);
    }
}