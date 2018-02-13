using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Borg.Infra.Services.Slugs
{
    public class GreekCharacterToAsciiService : InternationalCharacterToASCIIService
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
                source = Regex.Replace(source, (string)key, (string)Specials[key]);
            }
            return source;
        }

        private static IDictionary<string, string> GetSpecials()
        {
            return new Dictionary<string, string>
            {
                { @"γκ", "g"},
                { @"γγ", "g"},
                { @"αυτ", "aft"},
                { @"αυγ", "avg"}
            };
        }

        private static IDictionary<char, string> GetCache()
        {
            return new Dictionary<char, string>
            {
                {'α', "a"},
                {'ά', "a"},
                {'β', "v"},
                {'γ', "g"},
                {'δ', "d"},
                {'ε', "e"},
                {'έ', "e"},
                {'ζ', "z"},
                {'η', "i"},
                {'ή', "i"},
                {'θ', "th"},
                {'ι', "i"},
                {'ί', "i"},
                {'ϊ', "i"},
                {'κ', "k"},
                {'λ', "l"},
                {'μ', "m"},
                {'ν', "n"},
                {'ξ', "x"},
                {'ο', "o"},
                {'ό', "o"},
                {'π', "p"},
                {'ρ', "r"},
                {'σ', "s"},
                {'τ', "t"},
                {'υ', "u"},
                {'ύ', "u"},
                {'ϋ', "u"},
                {'φ', "f"},
                {'χ', "ch"},
                {'ψ', "ps"},
                {'ω', "o"},
                {'ώ', "o"},
                {'Α', "A"},
                {'Ά', "A"},
                {'Β', "V"},
                {'Γ', "G"},
                {'Δ', "D"},
                {'Ε', "E"},
                {'Έ', "E"},
                {'Ζ', "Z"},
                {'Η', "H"},
                {'Ή', "H"},
                {'Θ', "TH"},
                {'Ι', "I"},
                {'Ί', "I"},
                {'Ϊ', "I"},
                {'Κ', "K"},
                {'Λ', "L"},
                {'Μ', "M"},
                {'Ν', "N"},
                {'Ξ', "X"},
                {'Ο', "O"},
                {'Ό', "O"},
                {'Π', "P"},
                {'Ρ', "R"},
                {'Σ', "S"},
                {'Τ', "T"},
                {'Υ', "Y"},
                {'Ύ', "Y"},
                {'Ϋ', "Y"},
                {'Φ', "F"},
                {'Χ', "CH"},
                {'Ψ', "PS"},
                {'Ω', "O"},
                {'Ώ', "O"},
                {'ς', "s"},
            };
        }
    }
}