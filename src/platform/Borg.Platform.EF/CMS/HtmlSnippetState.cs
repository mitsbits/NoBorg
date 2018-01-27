using Borg.Infra.DDD.Contracts;

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
}