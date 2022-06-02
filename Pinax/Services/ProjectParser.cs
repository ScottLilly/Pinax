using System.Xml.Linq;
using Pinax.Common;
using Pinax.Models;
using Pinax.Models.PackageManagers;
using Pinax.Models.Projects;
using Pinax.Models.ProjectTypes;

namespace Pinax.Services;

public static class ProjectParser
{
    private static readonly object s_syncLock = new();
    private static readonly Dictionary<string, Version> s_nuGetPackageVersions = new();

    public static DotNetProject GetProject(string filename,
        IEnumerable<string> lines, DotNetVersions latestVersions)
    {
        string projectFileText = string.Join(Environment.NewLine, lines);

        var projectTypes = GetProjectTypes(projectFileText);

        var project = new DotNetProject(filename, latestVersions);

        project.ProjectTypes.AddRange(projectTypes);
        project.Packages.AddRange(GetPackages(filename, projectFileText));

        foreach (Package package in project.Packages)
        {
            lock (s_syncLock)
            {
                // TODO: Pass version from NuGetPackageDetails to package,
                // so it can determine if the package is out of date.
                // Use s_nuGetPackageVersions as a cache,
                // to prevent multiple calls to the NuGet API for the same package.
                if (!s_nuGetPackageVersions.ContainsKey(package.Name))
                {
                    NuGetPackageVersions? nuGetPackageVersions =
                        PackageManagerService.GetNuGetPackageVersions(package.Name);

                    if (nuGetPackageVersions != null)
                    {
                        var nonBetaVersion =
                            nuGetPackageVersions.Versions
                                .Where(v => v.ToCharArray().All(c => char.IsDigit(c) || c == '.'));

                        var lastVersion = new Version(nonBetaVersion.Last());

                        s_nuGetPackageVersions
                            .TryAdd(package.Name.ToLowerInvariant(),
                                lastVersion);
                    }

                    //NuGetPackageDetails? nuGetPackageDetails =
                    //    PackageManagerService.GetNuGetPackageDetails(package.Name);

                    //if (nuGetPackageDetails != null)
                    //{
                    //    Version latestVersion =
                    //        GetLatestVersionFromPackageDetails(nuGetPackageDetails);

                    //    s_nuGetPackageVersions
                    //        .TryAdd(package.Name.ToLowerInvariant(),
                    //            latestVersion);
                    //    //File.WriteAllText($"{package.Name}.json",
                    //    //    JsonConvert.SerializeObject(nuGetPackageDetails, Formatting.Indented));
                    //}
                }

                package.LatestNuGetVersion =
                    s_nuGetPackageVersions[package.Name.ToLowerInvariant()];
            }
        }

        return project;
    }

    private static Version GetLatestVersionFromPackageDetails(NuGetPackageDetails nuGetPackageDetails)
    {
        Version latestVersion = new(0, 0, 0, 0);

        foreach (NuGetPackageDetails.Item item in nuGetPackageDetails.items)
        {
            // TODO: Find better solution here
            if (item?.items == null)
            {
                continue;
            }

            foreach (NuGetPackageDetails.Item1 versionDetails in item.items)
            {
                string digits =
                    new(versionDetails.catalogEntry.version
                        .TakeWhile(c => char.IsDigit(c) || c == '.').ToArray());

                Version thisVersion =
                    Version.Parse(digits);

                if (thisVersion.CompareTo(latestVersion) > 0)
                {
                    latestVersion = thisVersion;
                }
            }
        }

        return latestVersion;
    }

    #region Private methods

    private static List<DotNetProjectType> GetProjectTypes(string projectFileText)
    {
        var projectTypes = new List<DotNetProjectType>();

        XElement root = XElement.Parse(projectFileText);
        XmlFunctions.RemoveNamespacePrefix(root);
        var propertyGroups = root.Elements("PropertyGroup").ToList();

        foreach (XElement propertyGroup in propertyGroups)
        {
            // Check for .NET Framework versions
            projectTypes.AddRange(
                GetProjectTypes(propertyGroup.Element("TargetFrameworkVersion")));

            // Check for .NET Core/5/6/7 versions
            projectTypes.AddRange(
                GetProjectTypes(propertyGroup.Element("TargetFramework")));
        }

        return projectTypes;
    }

    private static Enums.DotNetType GetDotNetType(string version)
    {
        if (version.StartsWith("v",
                StringComparison.InvariantCultureIgnoreCase))
        {
            return Enums.DotNetType.Framework;
        }

        if (version.StartsWith("netcore",
                StringComparison.InvariantCultureIgnoreCase))
        {
            return Enums.DotNetType.Core;
        }

        if (version.StartsWith("standard",
                StringComparison.InvariantCultureIgnoreCase))
        {
            return Enums.DotNetType.Standard;
        }

        if (version.StartsWith("net",
                StringComparison.InvariantCultureIgnoreCase))
        {
            return Enums.DotNetType.DotNet;
        }

        return Enums.DotNetType.Unknown;
    }

    private static List<DotNetProjectType> GetProjectTypes(XElement? target)
    {
        List<DotNetProjectType> projectTypes =
            new List<DotNetProjectType>();

        if (target != null &&
            target?.Value != null)
        {
            foreach (var version in target.Value.Split(';'))
            {
                projectTypes.Add(
                    new DotNetProjectType(
                        GetDotNetType(version), PinaxFunctions.GetVersionFromString(version)));
            }
        }

        return projectTypes;
    }

    private static List<Package> GetPackages(string projectFileName, string projectFileText)
    {
        XElement root = XElement.Parse(projectFileText);
        XmlFunctions.RemoveNamespacePrefix(root);
        var itemGroups = root.Elements("ItemGroup").ToList();

        var packages = new List<Package>();

        foreach (XElement itemGroup in itemGroups)
        {
            var packageReferences =
                itemGroup.Elements("PackageReference").ToList();

            if (!packageReferences.Any())
            {
                continue;
            }

            foreach (XElement packageReference in packageReferences)
            {
                packages.Add(new Package
                {
                    Name = packageReference.Attributes("Include").First().Value,
                    Version = Version.Parse(packageReference.Attributes("Version").First().Value)
                });
            }
        }

        // For older version of Visual Studio, with packages.config files
        packages.AddRange(PackagesFromPackagesConfig(projectFileName));

        return packages;
    }

    private static List<Package> PackagesFromPackagesConfig(string projectFileName)
    {
        var packages = new List<Package>();
        var projectFile = new FileInfo(projectFileName);
        string packageFileName =
            Path.Combine(projectFile.DirectoryName, "packages.config");

        if (File.Exists(packageFileName))
        {
            XElement root = XElement.Parse(File.ReadAllText(packageFileName));
            XmlFunctions.RemoveNamespacePrefix(root);

            var packageElements = root.Elements("package").ToList();

            foreach (XElement packageElement in packageElements)
            {
                var newPackage = new Package
                {
                    Name = packageElement.Attributes("id").First().Value,
                    Version = Version.Parse(packageElement.Attributes("version").First().Value)
                };

                packages.Add(newPackage);
            }
        }

        return packages;
    }

    #endregion
}