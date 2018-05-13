using Microsoft.Extensions.Localization;

namespace Borg.CMS.Services
{
    public class CultureLocalizedString : LocalizedString
    {
        public string Language { get; }
        public CultureLocalizedString(string language,string name, string value) : base(name, value)
        {
            Language = language;
        }

        public CultureLocalizedString(string language, string name, string value, bool resourceNotFound) : base(name, value, resourceNotFound)
        {
            Language = language;
        }

        public CultureLocalizedString(string language, string name, string value, bool resourceNotFound, string searchedLocation) : base(name, value, resourceNotFound, searchedLocation)
        {
            Language = language;
        }
    }
}