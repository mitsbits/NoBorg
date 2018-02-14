namespace Borg.Infra.DTO
{
    public interface IPropertyBag
    {
        void SetValue<TValue>(string propName, TValue propValue);

        TValue GetValue<TValue>(string propName);

        void SetValueRaw(string propName, string propValue);

        string GetValueRaw(string propName);
    }
}