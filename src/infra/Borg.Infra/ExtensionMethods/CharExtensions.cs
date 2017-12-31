namespace Borg
{
    public static class CharExtensions
    {
        public static string Repeat(this char c, int times)
        {
            return new string(c, times);
        }
    }
}