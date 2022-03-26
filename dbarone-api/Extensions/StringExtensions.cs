namespace dbarone_api.Extensions;
using System.Text;

public enum Justification
{
    LEFT,
    CENTRE,
    RIGHT
}

public static class StringExtensions
{
    /// <summary>
    /// Allows a short (TimeLow) guid or full guid to be converted to Guid
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static Guid ToGuid(this string s)
    {
        if (s.Length == 8)
            return new Guid(string.Format("{0}-0000-0000-0000-000000000000", s));
        else
            return new Guid(s);
    }

    public static string Justify(this string s, int length, Justification justification)
    {
        if (s.Length > length)
            s = s.Substring(0, length);

        if (justification == Justification.LEFT)
            return s.PadRight(length);
        else if (justification == Justification.CENTRE)
            return (" " + s.PadRight(length / 2).PadLeft(length / 2)).PadRight(length);
        else
            return s.PadLeft(length);
    }

    /// <summary>
    /// Parses a string for arguments. Arguments can be
    /// separated by whitespace. Single or double quotes
    /// can be used to delimit fields that contain space
    /// characters.
    /// </summary>
    public static string[] ParseArgs(this string s)
    {
        List<string> args = new List<string>();
        string currentArg = "";
        using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(s ?? "")))
        {
            using (var sr = new StreamReader(ms))
            {
                bool inWhiteSpace = false;
                char inQuoteChar = '\0';
                char nextChar;
                while (!sr.EndOfStream)
                {
                    nextChar = (char)sr.Read();
                    if (inQuoteChar == '\0' && (nextChar == '\'' || nextChar == '"'))
                    {
                        // Start of quoted field
                        inQuoteChar = nextChar;
                        currentArg = "";
                    }
                    else if (nextChar == inQuoteChar && nextChar != '\0')
                    {
                        // End of quoted field
                        // The end of quoted field MUST be followed by whitespace.
                        args.Add(currentArg);
                        inQuoteChar = '\0';
                    }
                    else if (!inWhiteSpace && inQuoteChar == '\0' && string.IsNullOrWhiteSpace(nextChar.ToString()))
                    {
                        // Start of whitespace, not in quoted field
                        args.Add(currentArg);
                        inWhiteSpace = true;
                    }
                    else if (inWhiteSpace && inQuoteChar == '\0' && !string.IsNullOrWhiteSpace(nextChar.ToString()))
                    {
                        // Start of new argument
                        currentArg = nextChar.ToString();
                        inWhiteSpace = false;
                    }
                    else
                    {
                        currentArg += nextChar.ToString();
                    }
                }
                if (!string.IsNullOrEmpty(currentArg))
                    args.Add(currentArg);
            }
        }
        return args.ToArray();
    }

    public static bool IsNullOrWhiteSpace(this string str)
    {
        return string.IsNullOrWhiteSpace(str);
    }

    public static bool IsNullOrEmpty(this string str)
    {
        return string.IsNullOrEmpty(str);
    }

    public static string RemoveRight(this string str, int length)
    {
        return str.Remove(str.Length - length);
    }

    public static string RemoveLeft(this string str, int length)
    {
        return str.Remove(0, length);
    }

    public static Stream ToStream(this string str)
    {
        MemoryStream stream = new MemoryStream();
        StreamWriter writer = new StreamWriter(stream);
        writer.Write(str);
        writer.Flush();
        stream.Position = 0;
        return stream;
    }

    /// <summary>
    /// Splits a string into chunks of [length] characters. Word breaks
    /// are avoided.
    /// </summary>
    /// <param name="s"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    public static IEnumerable<String> WordWrap(this string s, int length)
    {
        if (s == null)
            throw new ArgumentNullException("s");
        if (length <= 0)
            throw new ArgumentException("Part length has to be positive.", "partLength");

        var i = 0;
        while (i < s.Length)
        {
            // remove white space at start of line
            while (i < s.Length && char.IsWhiteSpace(s[i]))
                i++;

            var j = length;   // add extra character to check white space just after line.

            while (j >= 0)
            {
                if (i + j < s.Length && char.IsWhiteSpace(s[i + j]))
                    break;
                else if (i + j == s.Length)
                    break;
                j--;
            }
            if (j <= 0 || j > length)
                j = length;

            if (i + j >= s.Length)
                j = s.Length - i;

            var result = s.Substring(i, j);
            i += j;
            yield return result;
        }
    }
}