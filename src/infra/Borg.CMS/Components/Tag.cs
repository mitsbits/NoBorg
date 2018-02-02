using System;

namespace Borg.CMS.Components
{
    public class Tag<TKey> : Component<TKey> where TKey : IEquatable<TKey>
    {
        public override TKey Id { get; }
        public string TagDisplay { get; }
    }
}