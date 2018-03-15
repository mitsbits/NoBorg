using Borg.Infra.DDD.Contracts;
using System;

namespace Borg.CMS.Components.Contracts
{
    public interface IComponent<out TKey> : IEntity<TKey>, ICanBeDeleted, ICanBePublished where TKey : IEquatable<TKey>
    {
    }

    public interface IComponentPage<out TKey> : IComponent<TKey>, IHaveTitle, IHaveASlug, IHaveARelativePath, IHaveHtmlMetas, IHaveTags, IHaveSubtitleTitle, IHaveMainContent, IHavePagedContent, IHavePrimaryImage, IHaveComponentKey where TKey : IEquatable<TKey>
    {
    }
}