namespace Borg.Infra.DTO
{
    public interface ICloneable<out T> where T : class
    {
        T Clone();
    }
}