using System.Xml.Linq;
using Pinax.Models;

namespace Pinax.Services;

public static class ProjectParser
{
    public static Project GetProject(string filename, IEnumerable<string> lines)
    {
        string projectFileText = string.Join(Environment.NewLine, lines);

        var projectTypes = GetProjectTypes(projectFileText);

        Project project = new Project
        {
            FileName = filename,
            ProjectTypes = projectTypes
        };

        project.Packages.AddRange(GetPackages(filename, projectFileText));

        return project;
    }

    private static List<ProjectType> GetProjectTypes(string projectFileText)
    {
        var projectTypes = new List<ProjectType>();

        XElement root = XElement.Parse(projectFileText);
        RemoveNamespacePrefix(root);
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

    private static Version GetVersionFromString(string version)
    {
        string ver =
            version.Where(c => char.IsDigit(c) || c == '.')
                .Aggregate("", (current, c) => current + c);

        return Version.Parse(ver);
    }

    private static List<ProjectType> GetProjectTypes(XElement? target)
    {
        List<ProjectType> projectTypes =
            new List<ProjectType>();

        if (target != null &&
            target?.Value != null)
        {
            foreach (var version in target.Value.Split(';'))
            {
                projectTypes.Add(
                    new ProjectType(
                        GetDotNetType(version),
                        GetVersionFromString(version)));
            }
        }

        return projectTypes;
    }

    private static List<Package> GetPackages(string projectFileName, string projectFileText)
    {
        var root = XElement.Parse(projectFileText);
        RemoveNamespacePrefix(root);
        var itemGroups = root.Elements("ItemGroup").ToList();

        var packages = new List<Package>();

        foreach (var itemGroup in itemGroups)
        {
            var packageReferences =
                itemGroup.Elements("PackageReference").ToList();

            if (!packageReferences.Any())
            {
                continue;
            }

            foreach (var packageReference in packageReferences)
            {
                if (packageReference.Attributes("Include").Any() &&
                    packageReference.Attributes("Version").Any())
                {
                    packages.Add(new Package
                    {
                        Name = packageReference.Attributes("Include").First().Value,
                        Version = Version.Parse(packageReference.Attributes("Version").First().Value)
                    });
                }
            }
        }

        // For older version of Visual Studio, with packages.config files
        packages.AddRange(GetPackagesFromConfig(projectFileName));

        return packages;
    }

    private static List<Package> GetPackagesFromConfig(string projectFileName)
    {
        List<Package> packages = new List<Package>();

        FileInfo projectFile = new FileInfo(projectFileName);

        string packageFileName =
            Path.Combine(projectFile.DirectoryName, "packages.config");

        if (File.Exists(packageFileName))
        {
            var root = XElement.Parse(File.ReadAllText(packageFileName));
            RemoveNamespacePrefix(root);

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

    private static void RemoveNamespacePrefix(XElement element)
    {
        element.Name = element.Name.LocalName;

        var attributes = element.Attributes().ToArray();

        element.RemoveAttributes();

        foreach (var attribute in attributes)
        {
            element.Add(new XAttribute(attribute.Name.LocalName, attribute.Value));
        }

        foreach (var child in element.Descendants())
        {
            RemoveNamespacePrefix(child);
        }
    }
}