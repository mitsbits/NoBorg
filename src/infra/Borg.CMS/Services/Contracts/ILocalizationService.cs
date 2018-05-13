using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Microsoft.Extensions.Localization;

namespace Borg.CMS.Services.Contracts
{
    public interface ILocalizationService : ILanguageService
    {
        string FallbackLanguage { get; }
        IReadOnlyDictionary<string, CultureInfo> LanguageOptions { get; }
        CultureLocalizedString Resource(string language, string key, string defaultValue = "");
        IEnumerable<CultureLocalizedString> AllResources(string key = "");

    }

    public interface ILanguageService : IStringLocalizer
    {
        string Language { get; }
    }
}
