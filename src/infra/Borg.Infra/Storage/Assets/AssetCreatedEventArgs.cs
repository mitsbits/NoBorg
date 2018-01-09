using System;

namespace Borg.Infra.Storage.Assets
{
    public class AssetCreatedEventArgs<TKey> : EventArgs where TKey : IEquatable<TKey>
    {
        public AssetCreatedEventArgs(TKey recordId)
        {
            RecordId = recordId;
        }

        public TKey RecordId { get; }
    }
}