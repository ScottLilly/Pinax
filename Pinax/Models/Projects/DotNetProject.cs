using Pinax.Models.ProjectTypes;

namespace Pinax.Models.Projects;

public class DotNetProject : IProject<DotNetProjectType>
{
    private readonly string _fileName;
    private readonly DotNetVersions _latestVersions;

    public string Path { get; set; }
    public string ShortName =>
        string.Join('\\', _fileName.SplitPath().Skip(2));
    public string ProjectFileName =>
        _fileName.SplitPath().Last();

    public List<DotNetProjectType> ProjectTypes { get; } = new();
    public List<Package> Packages { get; } = new();

    public DotNetProject(string fileName, DotNetVersions latestVersions)
    {
        _fileName = fileName;
        _latestVersions = latestVersions;
    }

    public bool IsOutdated(Enums.WarningLevel warningLevel)
    {
        if (HasOutdatedProjectForType(Enums.DotNetType.Standard,
                warningLevel, _latestVersions.Standard))
        {
            return true;
        }

        if (HasOutdatedProjectForType(Enums.DotNetType.Core,
                warningLevel, _latestVersions.Core))
        {
            return true;
        }

        if (HasOutdatedProjectForType(Enums.DotNetType.Framework,
                warningLevel, _latestVersions.Framework))
        {
            return true;
        }

        if (HasOutdatedProjectForType(Enums.DotNetType.DotNet,
                warningLevel, _latestVersions.DotNet))
        {
            return true;
        }

        return false;
    }

    #region Private methods

    private bool HasOutdatedProjectForType(Enums.DotNetType type,
        Enums.WarningLevel warningLevel, Version version)
    {
        if (warningLevel == Enums.WarningLevel.None)
        {
            return false;
        }

        var projects =
            ProjectTypes.Where(p => p.Type.Equals(type));

        foreach (DotNetProjectType project in projects)
        {
            if (warningLevel
                is Enums.WarningLevel.Major
                or Enums.WarningLevel.Minor
                or Enums.WarningLevel.Build
                or Enums.WarningLevel.Revision)
            {
                if (project.Version.Major < version.Major)
                {
                    return true;
                }
            }

            if (warningLevel
                is Enums.WarningLevel.Minor
                or Enums.WarningLevel.Build
                or Enums.WarningLevel.Revision)
            {
                if (project.Version.Major == version.Major &&
                    project.Version.Minor < version.Minor)
                {
                    return true;
                }
            }

            if (warningLevel
                is Enums.WarningLevel.Build
                or Enums.WarningLevel.Revision)
            {
                if (project.Version.Major == version.Major &&
                    project.Version.Minor == version.Minor &&
                    project.Version.Build < version.Build)
                {
                    return true;
                }
            }

            if (warningLevel
                is Enums.WarningLevel.Revision)
            {
                if (project.Version.Major == version.Major &&
                    project.Version.Minor == version.Minor &&
                    project.Version.Build == version.Build &&
                    project.Version.Revision < version.Revision)
                {
                    return true;
                }
            }

        }

        return false;
    }

    #endregion
}