using System;

namespace Borg.CMS.Components.Contracts
{
    public interface ITag<out TKey> where TKey : IEquatable<TKey>
    {
        TKey Id { get; }
    }
    public interface ITag
    {
        string TagDisplay { get; }
        string TagSlug { get; }
    }
}