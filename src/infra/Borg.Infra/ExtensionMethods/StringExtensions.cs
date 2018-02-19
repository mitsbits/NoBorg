using Borg.Infra.Comparers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Borg
{
    public static class StringExtensions
    {
        public static bool IsValidRegex(this string pattern)
        {
            if (string.IsNullOrEmpty(pattern)) return false;

            try
            {
                Regex.Match("", pattern);
            }
            catch (ArgumentException)
            {
                return false;
            }

            return true;
        }

        public static string RemoveWhitespace(this string str)
        {
            return string.Join("", str.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
        }

        public static string PadToStartAndEndOnceOnly(this string str, char padChar)
        {
            var padString = new string(new[] { padChar });

            var copy = string.Copy(str);

            while (copy.StartsWith(padString) || copy.EndsWith(padString))
                copy = copy.Trim(padChar);

            return $"\"{copy}\"";
        }

        public static string MakeValidFileName(this string name)
        {
            string invalidChars = Regex.Escape(new string(System.IO.Path.GetInvalidFileNameChars()));
            string invalidRegStr = $@"([{invalidChars}]*\.+$)|([{invalidChars}]+)";

            return Regex.Replace(name, invalidRegStr, "_");
        }

        public static string Repeat(this string c, int times)
        {
            var repeatedStrArray = Enumerable.Repeat(c.ToCharArray(), times).SelectMany(x => x);
            return new string(repeatedStrArray.ToArray());
        }

        public static bool IsNullOrWhiteSpace(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        public static string RemoveDiacritics(this string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        public static string EnsureCorrectFilenameFromUpload(this string filename)
        {
            if (filename.Contains("\\"))
                filename = filename.Substring(filename.LastIndexOf("\\") + 1);

            return filename;
        }

        public static int IndexOfWhitespaceAgnostic(this IEnumerable<string> source, string check)
        {
            var result = -1;
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (!source.Any()) return result;
            var collection = source.ToArray();
            var comparer = new WhitespaceAgnosticComparer();
            for (int i = 0; i < collection.Length; i++)
            {
                if (comparer.Equals(collection[i], check)) result = i;
                if (result >= 0) break;
            }
            return result;
        }

        public static bool EqualsWhitespaceAgnostic(this string source, string check)
        {
            var comparer = new WhitespaceAgnosticComparer();
            return comparer.Equals(source, check);
        }

        public static bool ContainsWhitespaceAgnostic(this IEnumerable<string> source, string check)
        {
            return source.IndexOfWhitespaceAgnostic(check) >= 0;
        }

        public static IEnumerable<string> DistincetWhitespaceAgnostic(this IEnumerable<string> source, string check)
        {
            return source.Distinct(new WhitespaceAgnosticComparer());
        }
    }
}