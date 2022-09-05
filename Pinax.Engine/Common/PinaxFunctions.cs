namespace Pinax.Engine.Common;

public static class PinaxFunctions
{
    public static Version GetVersionFromString(string versionString)
    {
        string cleanedVersion =
            versionString.Where(c => char.IsDigit(c) || c == '.')
                .Aggregate("", (current, c) => current + c);

        Version version = Version.Parse(cleanedVersion);

        return new Version(
            version.Major == -1 ? 0 : version.Major,
            version.Minor == -1 ? 0 : version.Minor,
            version.Build == -1 ? 0 : version.Build,
            version.Revision == -1 ? 0 : version.Revision);
    }
}