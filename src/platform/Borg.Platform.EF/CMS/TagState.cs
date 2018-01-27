using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Borg.Infra.DDD.Contracts;

namespace Borg.Platform.EF.CMS
{
    public class TagState : IEntity<int>
    {
        public int Id { get; protected set; }
        public virtual ComponentState Component { get; protected set; }
        public string Tag { get; protected set; }
        public string TagNormalized { get; protected set; }
        public string TagSlug { get; protected set; }
        private ICollection<ArticleTagState> ArticleTags { get; } = new List<ArticleTagState>();

        [NotMapped]
        public IEnumerable<ArticleState> Articles => ArticleTags.Select(e => e.Article);
    }
}