namespace Blog.Domain.Helpers;

public static class ValidationsHelper
{
    public static bool IsFile(string image)
    {
        try
        {
            return Uri.IsWellFormedUriString(image, UriKind.Absolute)
                   && new Uri(image).AbsolutePath.Split('/').Last().Contains('.');
        }
        catch
        {
            return false;
        }
    }
}