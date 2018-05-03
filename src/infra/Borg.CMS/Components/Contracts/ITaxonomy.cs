using Borg.Infra.Collections.Hierarchy;
using Borg.Infra.DTO;
using System;

namespace Borg.CMS.Components.Contracts
{
    public interface ITaxonomy<out TKey> : ITreeNode<TKey>, IAmComponent<TKey>, IHaveTitle, IHaveComponentKey, IWeighted, IHaveACode, IMultilibgualContent where TKey : IEquatable<TKey>
    {
    }
}