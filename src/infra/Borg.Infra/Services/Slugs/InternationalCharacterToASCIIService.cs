using System.Runtime.CompilerServices;
using Borg.Infra.Services.Factory;

namespace Borg.Infra.Services.Slugs
{
    public interface InternationalCharacterToASCIIService
    {
        /// <summary>
        /// Transforms a non ASCII char to an ASCII char array.
        /// </summary>
        /// <param name="c">The non ASCII char.</param>
        /// <returns>Should return empty array when no match is found.</returns>
        char[] Transform(char c);

        string Special(string source, int length = -1);
    }

    public class NullInternationalCharacterToASCIIService : InternationalCharacterToASCIIService
    {
        private static readonly InternationalCharacterToASCIIService _instance;
        static NullInternationalCharacterToASCIIService()
        {
            _instance =
                New.Creator(typeof(NullInternationalCharacterToASCIIService)) as InternationalCharacterToASCIIService;
        }

        public static InternationalCharacterToASCIIService Instance => _instance;
        public char[] Transform(char c)
        {
            return new[] {c};
        }

        public string Special(string source, int length = -1)
        {
            return source;
        }
    }
}