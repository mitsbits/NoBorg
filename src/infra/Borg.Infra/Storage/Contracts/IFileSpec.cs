﻿using Borg.Infra.DTO;
using System;

namespace Borg.Infra.Storage.Contracts
{
    public interface IFileSpec<out TKey> : IFileSpec, ICloneable<IFileSpec<TKey>> where TKey : IEquatable<TKey>
    {
        TKey Id { get; }
    }

    public interface IFileSpec
    {
        string FullPath { get; }
        string Name { get; }
        DateTime CreationDate { get; }
        DateTime LastWrite { get; }
        DateTime? LastRead { get; }
        long SizeInBytes { get; }
        string MimeType { get; }
        string Extension { get; }
    }
}