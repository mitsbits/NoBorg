using Borg.CMS.Components.Contracts;
using System;
using System.Threading.Tasks;

namespace Borg.CMS.Components
{
    #region Not Usefull

    public delegate Task ComponentDeletedStateChangeEventHandler<TKey>(ComponentDeletedStateChangeEventArgs<TKey> args) where TKey : IEquatable<TKey>;

    public delegate Task ComponentPublishedStateChangeEventHandler<TKey>(ComponentPublishedStateChangeEventArgs<TKey> args) where TKey : IEquatable<TKey>;

    public class ComponentDeletedStateChangeEventArgs<TKey> : EventArgs where TKey : IEquatable<TKey>
    {
        public ComponentDeletedStateChangeEventArgs(TKey id, bool previousDeletedState, bool currentDeletedState)
        {
            Id = id;
            PreviousDeletedState = previousDeletedState;
            CurrentDeletedState = currentDeletedState;
        }

        public TKey Id { get; }
        public bool PreviousDeletedState { get; }
        public bool CurrentDeletedState { get; }
    }

    public class ComponentPublishedStateChangeEventArgs<TKey> : EventArgs where TKey : IEquatable<TKey>
    {
        public ComponentPublishedStateChangeEventArgs(TKey id, bool previousPublishedState, bool currentPublishedState)
        {
            Id = id;
            PreviousPublishedState = previousPublishedState;
            CurrentPublishedState = currentPublishedState;
        }

        public TKey Id { get; }
        public bool PreviousPublishedState { get; }
        public bool CurrentPublishedState { get; }
    }

    #endregion Not Usefull

    public abstract class Component<TKey> : IComponent<TKey> where TKey : IEquatable<TKey>
    {
        public abstract TKey Id { get; }

        public void Delete()
        {
            IsDeleted = true;
        }

        public bool IsDeleted { get; protected set; }
        public bool IsPublished { get; protected set; }

        public void Publish()
        {
            IsPublished = true;
        }

        public void Suspend()
        {
            IsPublished = false;
        }
    }
}