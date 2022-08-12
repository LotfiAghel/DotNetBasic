using System;
using System.Text;
using System.Text.RegularExpressions;

namespace SGSStandalone.Core
{

    public static class StringExtensions
{
    public static string ToSnakeCase(this string input)
    {
        if (string.IsNullOrEmpty(input)) { return input; }

        var startUnderscores = Regex.Match(input, @"^_+");
        return startUnderscores + Regex.Replace(input, @"([a-z0-9])([A-Z])", "$1_$2").ToLower();
    }

    public static string ToHex(this byte[] data)
    {
        return BitConverter.ToString(data).Replace("-", "");
    }

    public static string ToBase64(this byte[] data)
    {
        return Convert.ToBase64String(data);
    }

    public static string EncodeUtf8(this byte[] data)
    {
        return Encoding.UTF8.GetString(data);
    }

    public static byte[] DecodeUtf8(this string data)
    {
        return Encoding.UTF8.GetBytes(data);
    }

    /// <summary>
    /// Compares the string against a given pattern.
    /// </summary>
    /// <param name="str">The string.</param>
    /// <param name="pattern">The pattern to match, where "*" means any sequence of characters, and "?" means any single character.</param>
    /// <returns><c>true</c> if the string matches the given pattern; otherwise <c>false</c>.</returns>
    public static bool Like(this string str, string pattern)
    {
        return new Regex(
            "^" + Regex.Escape(pattern).Replace(@"\*", ".*").Replace(@"\?", ".") + "$",
            RegexOptions.IgnoreCase | RegexOptions.Singleline
        ).IsMatch(str);
    }

    public static string SubstringTo(this string str, char c)
    {
        var idx = str.IndexOf(c, StringComparison.Ordinal);
        if (idx < 0)
            return str;
        return str.Remove(idx);
    }

    public static string SubstringTo(this string str, string s)
    {
        var idx = str.IndexOf(s, StringComparison.Ordinal);
        if (idx < 0)
            return str;
        return str.Remove(idx);
    }

    public static string StripEnd(this string str, string part)
    {
        while (str.EndsWith(part, StringComparison.Ordinal))
        {
            str = str.Remove(str.LastIndexOf(part, StringComparison.Ordinal));
        }
        return str;
    }

    public static string StripStart(this string str, string part)
    {
        while (str.StartsWith(part, StringComparison.Ordinal))
        {
            str = str.Remove(0, part.Length);
        }
        return str;
    }
}

}
