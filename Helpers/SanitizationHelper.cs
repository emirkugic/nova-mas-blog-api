using System.Text.RegularExpressions;

public static class SanitizationHelper
{
    public static string SanitizeInput(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        string sanitizedInput = Regex.Replace(input, @"<script.*?>.*?</script>", string.Empty, RegexOptions.IgnoreCase);
        sanitizedInput = Regex.Replace(sanitizedInput, @"[^\w\s\-@.]", string.Empty);

        return sanitizedInput;
    }
}
