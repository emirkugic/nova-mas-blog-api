using System.Text.RegularExpressions;

public static class SanitizationHelper
{
    public static string SanitizeInput(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        string sanitizedInput = Regex.Replace(input, @"<script\b[^<]*(?:(?!<\/script>)<[^<]*)*<\/script>", string.Empty, RegexOptions.IgnoreCase | RegexOptions.Singleline);
        sanitizedInput = Regex.Replace(sanitizedInput, @"<[^>]+>", string.Empty);
        sanitizedInput = Regex.Replace(sanitizedInput, @"[^\p{L}\p{Nd}\s\-.@]", string.Empty);

        return sanitizedInput;
    }
}
