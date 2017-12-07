namespace Borg.Infra.DTO
{
    internal interface ICatalogued
    {
        string Key { get; }

        string HumanKey { get; }

        string Value { get; }

        string Hint { get; }

        string Flag { get; }
    }
}