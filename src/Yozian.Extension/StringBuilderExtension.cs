using System.Text;

namespace Yozian.Extension;

public static class StringBuilderExtension
{
    extension(StringBuilder @this)
    {
        public StringBuilder AppendWhen(bool matched, string text)
        {
            if (matched)
            {
                @this.Append(text);
            }

            return @this;
        }

        public StringBuilder AppendWhen(
            bool matched,
            string text,
            params string[] values
        )
        {
            if (matched)
            {
                @this.Append(string.Format(text, values));
            }

            return @this;
        }

        public StringBuilder AppendLineWhen(
            bool matched,
            string text
        )
        {
            if (matched)
            {
                @this.AppendLine(text);
            }

            return @this;
        }

        public StringBuilder AppendLineWhen(
            bool matched,
            string text,
            params string[] values
        )
        {
            if (matched)
            {
                @this.AppendLine(string.Format(text, values));
            }

            return @this;
        }
    }
}