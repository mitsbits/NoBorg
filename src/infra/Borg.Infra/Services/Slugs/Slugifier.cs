using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Borg.Infra.Services.Slugs
{
    public delegate string SlugifierExecutingInterceptor(string source);

    public class Slugifier : ISlugifierService
    {
        private readonly ILogger _logger;
        private readonly IEnumerable<InternationalCharacterToASCIIService> _internationalCharacterMappers;
        private static ISlugifierService _default;

        public Slugifier(ILoggerFactory loggerFactory, IEnumerable<InternationalCharacterToASCIIService> internationalCharacterMappers)
        {
            _logger = (loggerFactory == null) ? NullLogger.Instance : loggerFactory.CreateLogger(GetType());
            _internationalCharacterMappers = internationalCharacterMappers ??
                                             new[] {NullInternationalCharacterToASCIIService.Instance};
        }

        private Slugifier()
        {
            _logger = NullLogger.Instance;
            _internationalCharacterMappers = new[] { NullInternationalCharacterToASCIIService.Instance };
        }

        public static ISlugifierService CreateDefault()
        {
            return _default ?? (_default = new Slugifier());
        }

        #region IStringSlugifierService

        protected SlugifierExecutingInterceptor SlugifierExecuting;

        private static readonly Regex WordDelimiters = new Regex(@"[\s—–_]", RegexOptions.Compiled);

        private static readonly Regex MultipleHyphens = new Regex(@"-{2,}", RegexOptions.Compiled);

        public virtual string Slugify(string source, int maxlength = 42)
        {
            if (source == null) return string.Empty;

            source = InterceptSource(source);
            source = source.RemoveDiacritics();

            if (maxlength == -1)
            {
                //whaaaaaat?
            }
            else
            {
                foreach (var internationalCharacterToAsciiService in _internationalCharacterMappers)
                {
                    source = internationalCharacterToAsciiService.Special(source, maxlength);
                }
            }


            int len = source.Length;
            bool prevdash = false;
            var sb = new StringBuilder(len);
            char c;

            for (int i = 0; i < len; i++)
            {
                c = source[i];
                if ((c >= 'a' && c <= 'z') || (c >= '0' && c <= '9'))
                {
                    sb.Append(c);
                    prevdash = false;
                }
                else if (c >= 'A' && c <= 'Z')
                {
                    // tricky way to convert to lowercase
                    sb.Append((char)(c | 32));
                    prevdash = false;
                }
                else if (c == ' ' || c == ',' || c == '.' || c == '/' ||
                         c == '\\' || c == '-' || c == '_' || c == '=')
                {
                    if (!prevdash && sb.Length > 0)
                    {
                        sb.Append('-');
                        prevdash = true;
                    }
                }
                else if (c >= 128)
                {
                    int prevlen = sb.Length;
                    var replacement = new char[0];
                    foreach (var mapper in _internationalCharacterMappers)
                    {
                        replacement = mapper.Transform(c);
                        if (replacement.Length == 0) continue;
                        sb.Append(replacement);
                        break;
                    }
                    if (replacement.Length == 0) sb.Append(c);
                    if (prevlen != sb.Length) prevdash = false;
                }
                if (maxlength > 0 && i == maxlength) break;
            }

            return prevdash ? sb.ToString().Substring(0, sb.Length - 1) : sb.ToString();
        }

        private string InterceptSource(string source)
        {
            if (SlugifierExecuting != null)
            {
                source = SlugifierExecuting.Invoke(source);
            }

            source = MultipleHyphens.Replace(WordDelimiters.Replace(source, "-"), "-");
            return source.ToLowerInvariant();
        }

        #endregion IStringSlugifierService

        #region IDateSlugifierService

        public string Slugify(DateTime source)
        {
            Preconditions.NotEmpty(source, nameof(source));
            var sb = new StringBuilder(source.ToString("YYYYMMDD"));
            if (source.TimeOfDay != TimeSpan.Zero)
            {
                sb.Append(source.TimeOfDay.Seconds.ToString());
            }
            return sb.ToString();
        }

        public bool TryDeSlugify(string slug, out DateTime date)
        {
            try
            {
                var datePart = slug.Substring(0, 8);
                date = new DateTime(int.Parse(datePart.Substring(0, 4)), int.Parse(datePart.Substring(4, 2)), int.Parse(datePart.Substring(6, 2)));
                if (slug.Length > 8)
                {
                    var mins = int.Parse(slug.Substring(8));
                    date = date.Add(TimeSpan.FromSeconds(mins));
                }
                return true;
            }
            catch (Exception e)
            {
                date = default(DateTime);
                _logger.Warn("faied to convert {slug} to date - {@exception}", slug, e);
                return false;
            }
        }

        #endregion IDateSlugifierService
    }
}