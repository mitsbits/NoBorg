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
                { @"γγ", "g"},
                { @"γκ", "g"},
                { @"αυτ", "aft"}
            };
        }

        private static IDictionary<char, string> GetCache()
        {
            return new Dictionary<char, string>
            {
                {'α', "a"},
                {'β', "v"},
                {'γ', "g"},
                {'δ', "d"},
                {'ε', "e"},
                {'ζ', "z"},
                {'η', "i"},
                {'θ', "th"},
                {'ι', "i"},
                {'κ', "k"},
                {'λ', "l"},
                {'μ', "m"},
                {'ν', "n"},
                {'ξ', "x"},
                {'ο', "o"},
                {'π', "p"},
                {'ρ', "r"},
                {'σ', "s"},
                {'τ', "t"},
                {'υ', "u"},
                {'φ', "f"},
                {'χ', "ch"},
                {'ψ', "ps"},
                {'ω', "o"},
                {'Α', "A"},
                {'Β', "V"},
                {'Γ', "G"},
                {'Δ', "D"},
                {'Ε', "E"},
                {'Ζ', "Z"},
                {'Η', "H"},
                {'Θ', "TH"},
                {'Ι', "I"},
                {'Κ', "K"},
                {'Λ', "L"},
                {'Μ', "M"},
                {'Ν', "N"},
                {'Ξ', "X"},
                {'Ο', "O"},
                {'Π', "P"},
                {'Ρ', "R"},
                {'Σ', "S"},
                {'Τ', "T"},
                {'Υ', "Y"},
                {'Φ', "F"},
                {'Χ', "CH"},
                {'Ψ', "PS"},
                {'Ω', "O"},
                {'ς', "s"},
                {'Ά', "A"},
                {'ά', "a"},
                {'Έ', "e"},
                {'έ', "e"},
                {'Ί', "I"},
                {'ί', "i"},
                {'ΐ', "i"},
                {'Ή', "H"},
                {'ή', "h"},
                {'Ό', "O"},
                {'ό', "o"},
                {'Ύ', "Y"},
                {'ύ', "y"},
                {'ΰ', "y"},
                {'Ώ', "O"},
                {'ώ', "o"}
            };
        }
    }
}