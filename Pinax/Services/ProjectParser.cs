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

        project.Packages.AddRange(GetPackages(projectFileText));

        return project;
    }

    private static List<ProjectType> GetProjectTypes(string projectFileText)
    {
        List<ProjectType> projectTypes = new List<ProjectType>();

        var root = XElement.Parse(projectFileText);
        RemoveNamespacePrefix(root);
        var propertyGroups = root.Elements("PropertyGroup").ToList();

        if (propertyGroups.Any())
        {
            foreach (var propertyGroup in propertyGroups)
            {
                // Check for .NET Framework versions
                var targetFrameworkVersion =
                    propertyGroup.Element("TargetFrameworkVersion");

                if (targetFrameworkVersion != null &&
                    targetFrameworkVersion?.Value != null)
                {
                    foreach (var version in targetFrameworkVersion.Value.Split(';', StringSplitOptions.TrimEntries))
                    {
                        string ver = "";

                        foreach (char c in targetFrameworkVersion.Value)
                        {
                            if (char.IsDigit(c) || c == '.')
                            {
                                ver += c.ToString();
                            }
                        }

                        projectTypes.Add(
                            new ProjectType(
                                Enums.DotNetType.Framework, Version.Parse(ver)));
                    }
                }

                // Check for .NET Core/5/6/7 versions
                var targetFramework =
                    propertyGroup.Element("TargetFramework");

                if (targetFramework != null &&
                    targetFramework?.Value != null)
                {
                    foreach (var version in targetFramework.Value.Split(';', StringSplitOptions.TrimEntries))
                    {
                        var type = Enums.DotNetType.DotNet;

                        if (targetFramework.Value.StartsWith("netcore",
                                StringComparison.InvariantCultureIgnoreCase))
                        {
                            type = Enums.DotNetType.Core;
                        }
                        else if (targetFramework.Value.StartsWith("standard",
                                StringComparison.InvariantCultureIgnoreCase))
                        {
                            type = Enums.DotNetType.Standard;
                        }

                        string ver = "";

                        foreach (char c in version)
                        {
                            if (char.IsDigit(c) || c == '.')
                            {
                                ver += c.ToString();
                            }
                        }

                        projectTypes.Add(new ProjectType(type, Version.Parse(ver)));
                    }
                }
            }
        }

        return projectTypes;
    }

    private static List<Package> GetPackages(string projectFileText)
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
                        Version = packageReference.Attributes("Version").First().Value
                    });
                }
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