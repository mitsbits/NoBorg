namespace Borg.Infra
{
    public enum ResponseStatus
    {
        Undefined = 0,
        Verbose = 1,
        Warning = 2,
        Success = 8,
        Info = 16,
        Error = 32,
        Critical = 64
    }
}