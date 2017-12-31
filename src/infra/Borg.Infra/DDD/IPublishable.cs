namespace Borg.Infra.DDD
{
    public interface IPublishable
    {
        bool IsPublished { get; }

        void Publish();

        void Suspend();
    }
}