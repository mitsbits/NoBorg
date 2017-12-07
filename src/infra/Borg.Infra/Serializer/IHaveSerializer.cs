namespace Borg.Infra
{
    public interface IHaveSerializer
    {
        ISerializer Serializer { get; }
    }
}