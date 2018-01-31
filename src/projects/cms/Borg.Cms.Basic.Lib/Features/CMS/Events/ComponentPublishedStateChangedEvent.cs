namespace Borg.Cms.Basic.Lib.Features.CMS.Events
{
    public class ComponentPublishedStateChangedEvent : ComponentStateChangedEvent
    {
        public ComponentPublishedStateChangedEvent(int id, bool previous, bool current) : base(id)
        {
            Previous = previous;
            Current = current;
        }

        public bool Previous { get; }
        public bool Current { get; }
    }
}