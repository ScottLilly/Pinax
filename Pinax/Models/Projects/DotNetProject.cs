using Pinax.Models.ProjectTypes;

namespace Pinax.Models.Projects;

public class DotNetProject : IProject<DotNetProjectType>
{
    private readonly FileDetails _fileDetails;

    public List<DotNetProjectType> ProjectTypes { get; } = new();
    public List<Package> Packages { get; } = new();

    public string Path => _fileDetails.Path;
    public string Name => _fileDetails.Name;

    public DotNetProject(FileDetails fileDetails)
    {
        _fileDetails = fileDetails;
    }

    public bool IsOutdated(DotNetVersions latestVersions, Enums.WarningLevel warningLevel)
    {
        if (HasOutdatedProjectForType(Enums.DotNetType.Standard,
                warningLevel, latestVersions.Standard))
        {
            return true;
        }

        if (HasOutdatedProjectForType(Enums.DotNetType.Core,
                warningLevel, latestVersions.Core))
        {
            return true;
        }

        if (HasOutdatedProjectForType(Enums.DotNetType.Framework,
                warningLevel, latestVersions.Framework))
        {
            return true;
        }

        if (HasOutdatedProjectForType(Enums.DotNetType.DotNet,
                warningLevel, latestVersions.DotNet))
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