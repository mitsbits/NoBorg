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
                {'á', "a"},
                {'â', "v"},
                {'ã', "g"},
                {'ä', "d"},
                {'å', "e"},
                {'æ', "z"},
                {'ç', "i"},
                {'è', "th"},
                {'é', "i"},
                {'ê', "k"},
                {'ë', "l"},
                {'ì', "m"},
                {'í', "n"},
                {'î', "x"},
                {'ï', "o"},
                {'ð', "p"},
                {'ñ', "r"},
                {'ó', "s"},
                {'ô', "t"},
                {'õ', "u"},
                {'ö', "f"},
                {'÷', "ch"},
                {'ø', "ps"},
                {'ù', "o"},
                {'Á', "A"},
                {'Â', "V"},
                {'Ã', "G"},
                {'Ä', "D"},
                {'Å', "E"},
                {'Æ', "Z"},
                {'Ç', "H"},
                {'È', "TH"},
                {'É', "I"},
                {'Ê', "K"},
                {'Ë', "L"},
                {'Ì', "M"},
                {'Í', "N"},
                {'Î', "X"},
                {'Ï', "O"},
                {'Ð', "P"},
                {'Ñ', "R"},
                {'Ó', "S"},
                {'Ô', "T"},
                {'Õ', "Y"},
                {'Ö', "F"},
                {'×', "CH"},
                {'Ø', "PS"},
                {'Ù', "O"},
                {'ò', "s"},
                {'¢', "A"},
                {'Ü', "a"},
                {'¸', "e"},
                {'Ý', "e"},
                {'º', "I"},
                {'ß', "i"},
                {'À', "i"},
                {'¹', "H"},
                {'Þ', "h"},
                {'¼', "O"},
                {'ü', "o"},
                {'¾', "Y"},
                {'ý', "y"},
                {'à', "y"},
                {'¿', "O"},
                {'þ', "o"}
            };
        }
    }
}