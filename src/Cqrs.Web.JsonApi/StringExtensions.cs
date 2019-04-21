namespace Cqrs.Web.JsonApi
{
    using System;

    public static class StringExtension
    {
        public static string ToPascalCase(this string str)
        {
            if (!string.IsNullOrEmpty(str) && str.Length > 1)
            {
                return Char.ToUpperInvariant(str[0]) + str.Substring(1);
            }

            return str;
        }
    }
}
