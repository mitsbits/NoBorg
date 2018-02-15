using System.Collections.Generic;

namespace Borg.Infra.Comparers
{
    public class WhitespaceAgnosticComparer : IEqualityComparer<string>
    {
        /// <summary>
        /// Has a good distribution.
        /// </summary>
        private const int Multiplier = 89;

        /// <summary>
        /// Whether the two strings are equal
        /// </summary>
        public bool Equals(string x, string y)
        {
            return x.Replace(" ", string.Empty).ToLower() == y.Replace(" ", string.Empty).ToLower();
        }

        /// <summary>
        /// Return the hash code for this string.
        /// </summary>
        public int GetHashCode(string str)
        {
            // Stores the result.
            var result = 0;

            // Don't compute hash code on null object.
            if (str == null)
            {
                return 0;
            }

            var obj = str.Replace(" ", string.Empty).ToLower();

            // Get length.
            var length = obj.Length;

            // Return default code for zero-length strings [valid, nothing to hash with].
            if (length > 0)
            {
                // Compute hash for strings with length greater than 1
                var let1 = obj[0];          // First char of string we use
                var let2 = obj[length - 1]; // Final char

                // Compute hash code from two characters
                var part1 = let1 + length;
                result = (Multiplier * part1) + let2 + length;
            }

            return result;
        }
    }
}