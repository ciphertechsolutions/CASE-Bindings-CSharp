using System.Text.RegularExpressions;

namespace CT.CASE.Generator
{
    public static class String_ToIdentifier
    {
        private static readonly Regex WORD_BREAK = new Regex(@"\W+(\w|$)");
        private static readonly Regex VERIFY_PATTERN = new Regex("^[A-Za-z_][A-Za-z_0-9]*$");

        public static string ToIdentifier(this string s)
        {
            var ident = WORD_BREAK.Replace(s, (match) => match.Groups[1].Value.ToUpper());
            if (char.IsDigit(ident[0]))
            {
                ident = "_" + ident;
            }
            if (!VERIFY_PATTERN.IsMatch(ident))
            {
                throw new System.ArgumentException(string.Format("{0} (munged as {1})", s, ident));
            }
            return ident;
        }

        /// <summary>
        /// Replaces the first letter of this string with its lowercase counterpart.
        /// </summary>
        public static string ToCamelCase(this string s)
        {
            return s.Substring(0, 1).ToLowerInvariant() + s.Substring(1);
        }

        /// <summary>
        /// Replaces the first letter of this string with its uppercase counterpart.
        /// </summary>
        public static string ToPascalCase(this string s)
        {
            return s.Substring(0, 1).ToUpperInvariant() + s.Substring(1);
        }
    }
}
