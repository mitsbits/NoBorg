using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using Borg.Infra.DDD.Contracts;

namespace Borg.Platform.EF.CMS
{

    public class ComponentState : IEntity<int>
    {

        public int Id { get; protected set; }
        public bool IsDeleted { get; protected set; }
        public bool IsPublished { get; protected set; }
    }

    public class TagState : ComponentState
    {
        public string Tag { get; protected set; }
        public string TagNormalized { get; protected set; }
        public string TagSlug { get; protected set; }
        private ICollection<ArticleTagState> ArticleTags { get; } = new List<ArticleTagState>();

        [NotMapped]
        public IEnumerable<ArticleState> Articles => ArticleTags.Select(e => e.Article);
    }


    public class HtmlSnippetState : ComponentState
    {
        protected HtmlSnippetState() : base() { }

        public HtmlSnippetState(string code, string snippet, int id = 0) : this()
        {
            Code = code;
            HtmlSnippet = snippet;
            Id = id;
        }

        public string HtmlSnippet { get;  set; }
        public string Code { get;  set; }
    }


    public class ArticleState : ComponentState
    {
        public string Title { get; protected set; }
        public string Slug { get; protected set; }
        public string Body { get; protected set; }
        internal virtual TaxonomyState Taxonomy { get;  set; } 
        private  ICollection<ArticleTagState> ArticleTags { get; } = new List<ArticleTagState>();

        [NotMapped]
        public virtual IEnumerable<TagState> Tags => ArticleTags.Select(e => e.Tag);
    }

    public class ArticleTagState
    {
        public int ArticleId { get; protected set; }
        public virtual ArticleState Article { get; protected set; }

        public int TagId { get; protected set; }
        public virtual TagState Tag { get; protected set; }
    }

    public class TaxonomyState : ComponentState
    {
        public int ParentId { get; protected set; } = 0;
        public int ArticleId { get; protected set; }
        public virtual ArticleState Article { get; protected set; }

    }

}
