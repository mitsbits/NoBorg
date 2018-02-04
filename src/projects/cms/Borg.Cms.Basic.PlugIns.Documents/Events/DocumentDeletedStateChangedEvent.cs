namespace Borg.Cms.Basic.PlugIns.Documents.Events
{
    public class DocumentDeletedStateChangedEvent : DocumentStateChangedEvent
    {
        public DocumentDeletedStateChangedEvent(int id, bool previous, bool current) : base(id)
        {
            Previous = previous;
            Current = current;
        }

        public bool Previous { get; }
        public bool Current { get; }
    }
}