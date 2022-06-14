namespace Pinax.Models;

public class Package
{
    public string Name { get; set; }
    public Version VersionInUse { get; set; }
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
            if (VersionInUse.Major < LatestNuGetVersion.Major)
            {
                return true;
            }
        }

        if (warningLevel
            is Enums.WarningLevel.Minor
            or Enums.WarningLevel.Build
            or Enums.WarningLevel.Revision)
        {
            if (VersionInUse.Major == LatestNuGetVersion.Major &&
                VersionInUse.Minor < LatestNuGetVersion.Minor)
            {
                return true;
            }
        }

        if (warningLevel
            is Enums.WarningLevel.Build
            or Enums.WarningLevel.Revision)
        {
            if (VersionInUse.Major == LatestNuGetVersion.Major &&
                VersionInUse.Minor == LatestNuGetVersion.Minor &&
                VersionInUse.Build < LatestNuGetVersion.Build)
            {
                return true;
            }
        }

        if (warningLevel
            is Enums.WarningLevel.Revision)
        {
            if (VersionInUse.Major == LatestNuGetVersion.Major &&
                VersionInUse.Minor == LatestNuGetVersion.Minor &&
                VersionInUse.Build == LatestNuGetVersion.Build &&
                VersionInUse.Revision < LatestNuGetVersion.Revision)
            {
                return true;
            }
        }

        return false;
    }

    public override string ToString() =>
        $"{Name} [In project: {VersionInUse}] [Latest: {LatestNuGetVersion}]";
}