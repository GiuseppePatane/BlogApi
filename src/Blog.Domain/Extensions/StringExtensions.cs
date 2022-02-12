using System.Text.RegularExpressions;

namespace Blog.Domain.Extensions;

public static  class StringExtensions
{
    public static string? TrimAndTruncateHtml(this string? html, int maxCharacters, string trailingText = "&hellip;")
    {
        if (string.IsNullOrEmpty(html) || maxCharacters <= 0)
        {
            return string.Empty;
        }
        if (html.Length <= maxCharacters)
            return RemoveHtml(html);

        var textReplace = RemoveHtml(html);
        var newText = textReplace.Length <= maxCharacters ? textReplace : textReplace.Substring(0, maxCharacters);

        newText = newText.Remove(newText.LastIndexOf(" ", StringComparison.Ordinal));

        newText = $"{newText.RemovePunctuationEndChar()}{trailingText}";
        return newText;
    }
    private static string RemoveHtml(string html)
    {
        var regex = @"<[^>]+>";
        var spaceRegex = @"\s{2,}";
        var textReplace = Regex.Replace(html, regex, string.Empty);
        textReplace = Regex.Replace(textReplace, spaceRegex, " ");
        return textReplace;
    }
    private static string RemovePunctuationEndChar(this string html)
    {
        if (string.IsNullOrWhiteSpace(html)) return string.Empty;
        var last = html.Last();
        return char.IsPunctuation(last) ? html.TrimEnd(last) : html;
    }
}