using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using Borg.CMS.Services.Contracts;
using Microsoft.Extensions.Localization;

namespace Borg.CMS.Services
{
    public abstract class LocalizationServiceBase : ILocalizationService
    {
        private static readonly Lazy<IReadOnlyDictionary<string, CultureInfo>> _options = new Lazy<IReadOnlyDictionary<string, CultureInfo>>(
            () =>
            {
                return new ReadOnlyDictionary<string, CultureInfo>(CultureInfo.GetCultures(CultureTypes.NeutralCultures).ToDictionary(x => x.TwoLetterISOLanguageName, x => x));
            });

        protected LocalizationServiceBase(string fallbacklanguage)
        {
            FallbackLanguage = fallbacklanguage;
        }

        protected LocalizationServiceBase(string fallbacklanguage, string language):this(fallbacklanguage)
        {
            Language = language;
        }

        public string FallbackLanguage { get; }
        public virtual IReadOnlyDictionary<string, CultureInfo> LanguageOptions => _options.Value;
        public abstract CultureLocalizedString Resource(string language, string key, string defaultValue = "");
        public abstract IEnumerable<CultureLocalizedString> AllResources(string key = "");
        public abstract IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures);

        private string _language;
        public string Language
        {
            get => _language.IsNullOrWhiteSpace() ? FallbackLanguage : _language;
            set => _language = value;
        }

        public abstract IStringLocalizer WithCulture(CultureInfo culture);

        LocalizedString IStringLocalizer.this[string name] => Resource(Language, name);


        LocalizedString IStringLocalizer.this[string name, params object[] arguments] => new CultureLocalizedString(Language, name, string.Format(Resource(Language, name).Value, arguments));


    }
}
