namespace Borg.Infra.Messaging
{
    public interface ICorrelatedResponse : ICorrelated
    {
        void Corralate(ICorrelated message);
    }
}