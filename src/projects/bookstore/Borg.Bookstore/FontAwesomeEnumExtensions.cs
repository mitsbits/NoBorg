using System;
using System.Text;
using Borg.Bookstore;
using Borg.Cms.Basic.Lib;

namespace Borg
{
    public static class FontAwesomeEnumExtensions
    {
        public static string ToCssClass(this FontAwesomeEnum selection)
        {
            var input = selection.ToString().ToCharArray();

            var builder = new StringBuilder();
            for (int i = 0; i < input.Length; i++)
            {
                var c = input[i];
                if (i == 0)
                {
                    builder.Append(char.ToLower(c));
                }
                else
                {
                    if (char.IsUpper(c))
                    {
                        builder.Append('-');
                        builder.Append(char.ToLower(c));
                    }
                    else
                    {
                        builder.Append(c);
                    }
                }
            }
            return $"fa fa-{builder}";
        }


        public static string ParseClass(this FontAwesomeEnum selection, string source)
        {
            var test = selection;
            var transformed = Enum.TryParse(source, out test);
            if (transformed) selection = test;

            var input = selection.ToString().ToCharArray();

            var builder = new StringBuilder();
            for (int i = 0; i < input.Length; i++)
            {
                var c = input[i];
                if (i == 0)
                {
                    builder.Append(char.ToLower(c));
                }
                else
                {
                    if (char.IsUpper(c))
                    {
                        builder.Append('-');
                        builder.Append(char.ToLower(c));
                    }
                    else
                    {
                        builder.Append(c);
                    }
                }
            }
            return $"fa fa-{builder}";
        }
    }
}