using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Borg.MVC.Services.Slugs
{
    internal class GreekCharacterToAsciiService : InternationalCharacterToASCIIService
    {
        private static readonly Lazy<IDictionary<char, string>> _cache = new Lazy<IDictionary<char, string>>(GetCache);

        private static readonly Lazy<IDictionary<string, string>> _specials = new Lazy<IDictionary<string, string>>(GetSpecials);

        private static IDictionary<char, string> Cache => _cache.Value;

        private static IDictionary<string, string> Specials => _specials.Value;

        public char[] Transform(char c)
        {
            return Cache.ContainsKey(c) ? Cache[c].ToCharArray() : new char[0];
        }

        public string Special(string source, int length = -1)
        {
            foreach (var key in Specials.Keys)
            {
                source = Regex.Replace(source, key, Specials[key]);
            }
            return source;
        }

        private static IDictionary<string, string> GetSpecials()
        {
            return new Dictionary<string, string>
            {
                { @"��", "g"},
                { @"��", "g"},
                { @"���", "aft"}
            };
        }

        private static IDictionary<char, string> GetCache()
        {
            return new Dictionary<char, string>
            {
                {'�', "a"},
                {'�', "v"},
                {'�', "g"},
                {'�', "d"},
                {'�', "e"},
                {'�', "z"},
                {'�', "i"},
                {'�', "th"},
                {'�', "i"},
                {'�', "k"},
                {'�', "l"},
                {'�', "m"},
                {'�', "n"},
                {'�', "x"},
                {'�', "o"},
                {'�', "p"},
                {'�', "r"},
                {'�', "s"},
                {'�', "t"},
                {'�', "u"},
                {'�', "f"},
                {'�', "ch"},
                {'�', "ps"},
                {'�', "o"},
                {'�', "A"},
                {'�', "V"},
                {'�', "G"},
                {'�', "D"},
                {'�', "E"},
                {'�', "Z"},
                {'�', "H"},
                {'�', "TH"},
                {'�', "I"},
                {'�', "K"},
                {'�', "L"},
                {'�', "M"},
                {'�', "N"},
                {'�', "X"},
                {'�', "O"},
                {'�', "P"},
                {'�', "R"},
                {'�', "S"},
                {'�', "T"},
                {'�', "Y"},
                {'�', "F"},
                {'�', "CH"},
                {'�', "PS"},
                {'�', "O"},
                {'�', "s"},
                {'�', "A"},
                {'�', "a"},
                {'�', "e"},
                {'�', "e"},
                {'�', "I"},
                {'�', "i"},
                {'�', "i"},
                {'�', "H"},
                {'�', "h"},
                {'�', "O"},
                {'�', "o"},
                {'�', "Y"},
                {'�', "y"},
                {'�', "y"},
                {'�', "O"},
                {'�', "o"}
            };
        }
    }
}