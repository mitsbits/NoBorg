using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Borg.Infra.Services.Slugs
{
    public delegate string SlugifierExecutingInterceptor(string source);

    public class Slugifier : ISlugifierService
    {
        private readonly IEnumerable<InternationalCharacterToASCIIService> _internationalCharacterMappers;

        public Slugifier(IEnumerable<InternationalCharacterToASCIIService> internationalCharacterMappers)
        {
            _internationalCharacterMappers = internationalCharacterMappers ?? throw new ArgumentNullException(nameof(internationalCharacterMappers));
        }

        protected SlugifierExecutingInterceptor SlugifierExecuting;

        private static readonly Regex WordDelimiters = new Regex(@"[\s—–_]", RegexOptions.Compiled);

        private static readonly Regex MultipleHyphens = new Regex(@"-{2,}", RegexOptions.Compiled);

        public virtual string Slugify(string source, int maxlength = 42)
        {
            if (source == null) return string.Empty;

            source = InterceptSource(source);

            foreach (var nternationalCharacterToAsciiService in _internationalCharacterMappers)
            {
                source = nternationalCharacterToAsciiService.Special(source, maxlength);
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
            return source;
        }
    }
}