using System;
using System.Linq;
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
    }
}