﻿using Borg.Infra.Storage.Contracts;
using System;
using System.IO;

namespace Borg.Infra.Storage
{
    public class FileSpecDefinition<TKey> : FileSpecDefinition, IFileSpec<TKey> where TKey : IEquatable<TKey>
    {
        public FileSpecDefinition(TKey id, string fullPath, string name, DateTime creationDate, DateTime lastWrite,
            DateTime? lastRead, long sizeInBytes, string mimeType) : base(fullPath, name, creationDate, lastWrite,
            lastRead, sizeInBytes, mimeType)
        {
            Id = id;
        }

        internal FileSpecDefinition(TKey id)
        {
            Id = id;
        }

        public TKey Id { get; internal set; }

        public IFileSpec<TKey> Clone()
        {
            return (IFileSpec<TKey>) this.MemberwiseClone();
        }
    }

    public class FileSpecDefinition : IFileSpec
    {
        internal FileSpecDefinition()
        {
        }

        public FileSpecDefinition(string fullPath, string name, DateTime creationDate, DateTime lastWrite, DateTime? lastRead,
            long sizeInBytes, string mimeType = "")
        {
            FullPath = fullPath;
            Name = name;
            CreationDate = creationDate;
            LastWrite = lastWrite;
            LastRead = lastRead;
            SizeInBytes = sizeInBytes;
            MimeType = string.IsNullOrWhiteSpace(mimeType) ? fullPath.GetMimeType() : mimeType;
            Extension = Path.GetExtension(fullPath);
        }

        public string FullPath { get; protected set; }
        public string Name { get; }
        public DateTime CreationDate { get; }
        public DateTime LastWrite { get; protected set; }
        public DateTime? LastRead { get; protected set; }
        public long SizeInBytes { get; }
        public string MimeType { get; }
        public string Extension { get; }

        [Obsolete("not a good idea, copy instead", false)]
        public void ModifyPath(string newPath)
        {
            FullPath = newPath;
            LastWrite = DateTime.UtcNow;
        }
    }
}