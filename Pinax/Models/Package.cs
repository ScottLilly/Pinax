namespace Pinax.Models;

public class Package
{
    public string Name { get; set; }
    public Version Version { get; set; }
    public Version LatestNuGetVersion { get; set; }

    public bool IsOutdated(Enums.WarningLevel warningLevel)
    {
        if (warningLevel == Enums.WarningLevel.None)
        {
            return false;
        }

        if (warningLevel
            is Enums.WarningLevel.Major
            or Enums.WarningLevel.Minor
            or Enums.WarningLevel.Build
            or Enums.WarningLevel.Revision)
        {
            if (Version.Major < LatestNuGetVersion.Major)
            {
                return true;
            }
        }

        if (warningLevel
            is Enums.WarningLevel.Minor
            or Enums.WarningLevel.Build
            or Enums.WarningLevel.Revision)
        {
            if (Version.Major == LatestNuGetVersion.Major &&
                Version.Minor < LatestNuGetVersion.Minor)
            {
                return true;
            }
        }

        if (warningLevel
            is Enums.WarningLevel.Build
            or Enums.WarningLevel.Revision)
        {
            if (Version.Major == LatestNuGetVersion.Major &&
                Version.Minor == LatestNuGetVersion.Minor &&
                Version.Build < LatestNuGetVersion.Build)
            {
                return true;
            }
        }

        if (warningLevel
            is Enums.WarningLevel.Revision)
        {
            if (Version.Major == LatestNuGetVersion.Major &&
                Version.Minor == LatestNuGetVersion.Minor &&
                Version.Build == LatestNuGetVersion.Build &&
                Version.Revision < LatestNuGetVersion.Revision)
            {
                return true;
            }
        }

        return false;
    }

    public override string ToString() =>
        $"{Name} [In project: {Version}] [Latest: {LatestNuGetVersion}]";
}