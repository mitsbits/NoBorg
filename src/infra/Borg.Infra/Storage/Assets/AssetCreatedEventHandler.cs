using System;
using System.Threading.Tasks;

public delegate Task AssetCreatedEventHandler<TKey>(AssetCreatedEventArgs<TKey> args) where TKey : IEquatable<TKey>;