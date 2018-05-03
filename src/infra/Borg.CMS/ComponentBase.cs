using Borg.CMS.Components.Contracts;
using System;

namespace Borg.CMS
{
    public abstract class ComponentBase<TKey> : IComponent<TKey> where TKey : IEquatable<TKey>
    {
        public abstract TKey Id { get; protected set; }

        public virtual void Delete()
        {
            if (IsDeleted) return;
            IsDeleted = true;
        }

        public virtual bool IsDeleted { get; protected set; }
        public virtual bool IsPublished { get; protected set; }

        public virtual void Publish()
        {
            if (IsPublished) return;
            IsPublished = true;
        }

        public virtual void Suspend()
        {
            if (!IsPublished) return;
            IsPublished = false;
        }
    }


    public abstract class TaxonomyBase<TKey> : ITaxonomy<TKey> where TKey : IEquatable<TKey>
    {
        public virtual int Depth { get; protected set; }
        public abstract TKey Id { get; protected set; }
        public abstract TKey ParentId { get; protected set; }
        public abstract TKey[] HierarchyKeys { get; }
        public abstract IComponent<TKey> Component { get; }
        public virtual string Title { get; protected set; }
        public virtual double Weight { get; protected set; }
        public virtual string ComponentKey => Component.Id.ToString();
        public abstract string LanguageCode { get; protected set; }
        public virtual string Code { get; protected set; }
    }
}