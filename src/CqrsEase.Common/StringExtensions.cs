namespace CqrsEase.Common
{
    using System;

    public static class StringExtension
    {
        public static string ToPascalCase(this string str)
        {
            if (!string.IsNullOrEmpty(str) && str.Length > 1 && !Char.IsUpper(str[0]))
            {
                return Char.ToUpperInvariant(str[0]) + str.Substring(1);
            }

            return str;
        }

        public static string ToCamelCase(this string str)
        {
            if (!string.IsNullOrEmpty(str) && str.Length > 1 && !Char.IsLower(str[0]))
            {
                return Char.ToLowerInvariant(str[0]) + str.Substring(1);
            }

            return str;
        }
    } 
}
