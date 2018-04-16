namespace Borg.Infra.Configuration.Contracts
{
    public interface ISettingsProvider<out TSettings> where TSettings : ISettings
    {
        TSettings Config { get; }
    }
}