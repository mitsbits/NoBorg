using System;

namespace Borg.CMS.Components.Contracts
{
    public interface IAmComponent<out TKey> where TKey : IEquatable<TKey>
    {
        IComponent<TKey> Component { get; }
    }
}