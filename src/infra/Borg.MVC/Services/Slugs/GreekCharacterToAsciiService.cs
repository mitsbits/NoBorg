using System;
using System.Collections.Generic;

namespace Borg.MVC.Services.Slugs
{
    internal class GreekCharacterToAsciiService : InternationalCharacterToASCIIService
    {
        private static readonly Lazy<IDictionary<char, string>> _cache = new Lazy<IDictionary<char, string>>(GetCache);

        private static IDictionary<char, string> Cache => _cache.Value;

        public char[] Transform(char c)
        {
            return Cache.ContainsKey(c) ? Cache[c].ToCharArray() : new char[0];
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