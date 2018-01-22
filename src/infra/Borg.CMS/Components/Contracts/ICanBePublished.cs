namespace Borg.CMS.Components.Contracts
{
    public interface ICanBePublished
    {
        bool IsPublished { get; }

        void Publish();

        void Suspend();
    }
}