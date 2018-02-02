namespace Borg.Infra.Conventions.Contracts
{
    public interface IConvention<out TOutput>
    {
        TOutput Invoke(object[] args);
    }
}