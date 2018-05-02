using System;

namespace Borg.CMS.Components.Contracts
{
    public interface IComponentPage<out TKey> : IComponent<TKey>, IHaveTitle, IHaveASlug, IHaveARelativePath, IHaveHtmlMetas, IHaveTags, IHaveSubtitleTitle, IHaveMainContent, IHavePagedContent, IHavePrimaryImage, IHaveComponentKey where TKey : IEquatable<TKey>
    {
    }
}