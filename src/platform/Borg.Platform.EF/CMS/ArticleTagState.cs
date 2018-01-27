namespace Borg.Platform.EF.CMS
{
    public class ArticleTagState
    {
        public int ArticleId { get; protected set; }
        public virtual ArticleState Article { get; protected set; }

        public int TagId { get; protected set; }
        public virtual TagState Tag { get; protected set; }
    }
}