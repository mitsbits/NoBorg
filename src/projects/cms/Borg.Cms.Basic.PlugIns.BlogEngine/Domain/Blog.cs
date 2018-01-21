using Borg.Cms.Basic.Lib.Discovery;
using Borg.Infra.DDD;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Borg.Cms.Basic.PlugIns.BlogEngine.Domain
{
    [Entity]
    public class Blog : IEntity<int>
    {
        public int Id { get; protected set; }
        public string Title { get; protected set; }
        public string Slug { get; protected set; }
        public virtual ICollection<BloggerBlog> BloggerBlogs { get; protected set; }
    }

    public class BloggerBlog
    {
        public int BloggerId { get; protected set; }
        public int BlogId { get; protected set; }
    }

    [Entity]
    public class Blogger : IEntity<int>
    {
        public int Id { get; protected set; }
        public string UserName { get; protected set; }
        public string Avatar { get; protected set; }
        public string DisplayName { get; protected set; }
        public string Slug { get; protected set; }
        public virtual ICollection<BloggerBlog> BloggerBlogs { get; protected set; }
    }

    [Entity]
    public class Blogpost : IEntity<int>
    {
        public int Id { get; protected set; }
        public int BlogId { get; protected set; }
        public int BloggerId { get; protected set; }
        public string Title { get; protected set; }
        public string Slug { get; protected set; }
        public string Body { get; protected set; }
        public virtual Blog Blog { get; protected set; }
        public virtual Blogger Blogger { get; protected set; }
    }
}