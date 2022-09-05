using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Octokit;
using Pinax.Models;

namespace Pinax;

public static class ExtensionMethods
{
    public static bool IsNotNullEmptyOrWhiteSpace(this string text)
    {
        return !string.IsNullOrWhiteSpace(text);
    }

    public static bool Matches(this string text, string comparisonText)
    {
        return text.Trim().Equals(comparisonText.Trim(), 
            StringComparison.InvariantCultureIgnoreCase);
    }

    public static bool None<T>(this IEnumerable<T> elements, Func<T, bool>? func = null)
    {
        return func == null
            ? !elements.Any()
            : !elements.Any(func.Invoke);
    }

    public static string GetEnumDisplayName(this Enum enumType)
    {
        return enumType.GetType().GetMember(enumType.ToString())
            .First()
            .GetCustomAttribute<DisplayAttribute>()
            ?.Name ?? enumType.ToString();
    }

    public static IEnumerable<string> SplitPath(this string path)
    {
        return path.Split('/', '\\');
    }

    public static FileDetails ToFileDetails(this SearchCode searchItem)
    {
        return new FileDetails(searchItem.Repository.HtmlUrl, searchItem.Name);
    }

    public static FileDetails ToFileDetails(this FileInfo fileInfo)
    {
        return new FileDetails(fileInfo.DirectoryName ?? "", fileInfo.Name);
    }

    public static Version ToVersion(this string rawVersion)
    {
        try
        {
            rawVersion = rawVersion.Replace('-', '.');

            string cleanVersion = 
                rawVersion.Where(c => char.IsDigit(c) || c == '.')
                    .Aggregate("", (current, c) => current + c);

            if (cleanVersion.Substring(cleanVersion.Length - 1) == ".")
            {
                cleanVersion = cleanVersion.Substring(0, cleanVersion.Length - 1);
            }

            return Version.Parse(cleanVersion);
        }
        catch (Exception e)
        {
            return new Version(0, 0, 0, 0);
        }
    }
}