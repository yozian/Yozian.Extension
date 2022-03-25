using System.Text;

namespace Yozian.Extension
{
    public static class StringBuilderExtension
    {
        public static StringBuilder AppendWhen(this StringBuilder @this, bool matched, string text)
        {
            if (matched)
            {
                @this.Append(text);
            }

            return @this;
        }

        public static StringBuilder AppendWhen(
            this StringBuilder @this,
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

        public static StringBuilder AppendLineWhen(
            this StringBuilder @this,
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

        public static StringBuilder AppendLineWhen(
            this StringBuilder @this,
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
