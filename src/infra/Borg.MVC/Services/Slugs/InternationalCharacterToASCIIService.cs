namespace Borg.MVC.Services.Slugs
{
    public interface InternationalCharacterToASCIIService
    {
        /// <summary>
        /// Transforms a non ASCII char to an ASCII char array.
        /// </summary>
        /// <param name="c">The non ASCII char.</param>
        /// <returns>Should return empty array when no match is found.</returns>
        char[] Transform(char c);
    }
}