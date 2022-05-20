using System.Xml.Linq;
using Pinax.Models;

namespace Pinax.Services;

public static class ProjectParser
{
    public static Project GetProject(string filename, IEnumerable<string> lines)
    {
        string projectFileText = string.Join(Environment.NewLine, lines);

        Project project = new Project
        {
            FileName = filename,
            DotNetType = GetDotNetType(projectFileText),
            Version = GetVersion(projectFileText)
        };

        project.Packages.AddRange(GetPackages(projectFileText));

        return project;
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

    private static Enums.DotNetType GetDotNetType(string projectFileText)
    {
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

                if (targetFrameworkVersion != null)
                {
                    return Enums.DotNetType.Framework;
                }

                // Check for .NET Core or .NET 5/6/7 version
                var targetFramework =
                    propertyGroup.Element("TargetFramework");

                if (targetFramework?.Value != null)
                {
                    if (targetFramework.Value.StartsWith("netcore",
                            StringComparison.InvariantCultureIgnoreCase))
                    {
                        return Enums.DotNetType.Core;
                    }

                    if (targetFramework.Value.StartsWith("net5.0",
                            StringComparison.InvariantCultureIgnoreCase))
                    {
                        return Enums.DotNetType.DotNet;
                    }

                    if (targetFramework.Value.StartsWith("net6.0",
                            StringComparison.InvariantCultureIgnoreCase))
                    {
                        return Enums.DotNetType.DotNet;
                    }

                    if (targetFramework.Value.StartsWith("net7.0",
                            StringComparison.InvariantCultureIgnoreCase))
                    {
                        return Enums.DotNetType.DotNet;
                    }
                }
            }
        }

        return Enums.DotNetType.Unknown;
    }

    private static Project.DotNetVersion GetVersion(string projectFileText)
    {
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

                if (targetFrameworkVersion != null)
                {
                    switch (targetFrameworkVersion.Value)
                    {
                        case "v1.0":
                            return Project.DotNetVersion.Framework_1_0;
                        case "v1.1":
                            return Project.DotNetVersion.Framework_1_1;
                        case "v2.0":
                            return Project.DotNetVersion.Framework_2_0;
                        case "v3.0":
                            return Project.DotNetVersion.Framework_3_0;
                        case "v3.5":
                            return Project.DotNetVersion.Framework_3_5;
                        case "v4.0":
                            return Project.DotNetVersion.Framework_4_0;
                        case "v4.5":
                            return Project.DotNetVersion.Framework_4_5;
                        case "v4.5.1":
                            return Project.DotNetVersion.Framework_4_5_1;
                        case "v4.5.2":
                            return Project.DotNetVersion.Framework_4_5_2;
                        case "v4.6":
                            return Project.DotNetVersion.Framework_4_6;
                        case "v4.6.1":
                            return Project.DotNetVersion.Framework_4_6_1;
                        case "v4.6.2":
                            return Project.DotNetVersion.Framework_4_6_2;
                        case "v4.7":
                            return Project.DotNetVersion.Framework_4_7;
                        case "v4.7.1":
                            return Project.DotNetVersion.Framework_4_7_1;
                        case "v4.7.2":
                            return Project.DotNetVersion.Framework_4_7_2;
                        case "v4.8":
                            return Project.DotNetVersion.Framework_4_8;
                    }
                }

                // Check for .NET Core or .NET 5/6/7 version
                var targetFramework =
                    propertyGroup.Element("TargetFramework");

                if (targetFramework?.Value != null)
                {
                    if (targetFramework.Value.StartsWith("netcoreapp1.0",
                            StringComparison.InvariantCultureIgnoreCase))
                    {
                        return Project.DotNetVersion.Core_1_0;
                    }

                    if (targetFramework.Value.StartsWith("netcoreapp1.1",
                            StringComparison.InvariantCultureIgnoreCase))
                    {
                        return Project.DotNetVersion.Core_1_1;
                    }

                    if (targetFramework.Value.StartsWith("netcoreapp2.0",
                            StringComparison.InvariantCultureIgnoreCase))
                    {
                        return Project.DotNetVersion.Core_2_0;
                    }

                    if (targetFramework.Value.StartsWith("netcoreapp2.1",
                            StringComparison.InvariantCultureIgnoreCase))
                    {
                        return Project.DotNetVersion.Core_2_1;
                    }

                    if (targetFramework.Value.StartsWith("netcoreapp2.2",
                            StringComparison.InvariantCultureIgnoreCase))
                    {
                        return Project.DotNetVersion.Core_2_2;
                    }

                    if (targetFramework.Value.StartsWith("netcoreapp3.0",
                            StringComparison.InvariantCultureIgnoreCase))
                    {
                        return Project.DotNetVersion.Core_3_0;
                    }

                    if (targetFramework.Value.StartsWith("netcoreapp3.1",
                            StringComparison.InvariantCultureIgnoreCase))
                    {
                        return Project.DotNetVersion.Core_3_1;
                    }

                    if (targetFramework.Value.StartsWith("net5.0",
                            StringComparison.InvariantCultureIgnoreCase))
                    {
                        return Project.DotNetVersion.Net_5;
                    }

                    if (targetFramework.Value.StartsWith("net6.0",
                            StringComparison.InvariantCultureIgnoreCase))
                    {
                        return Project.DotNetVersion.Net_6;
                    }

                    if (targetFramework.Value.StartsWith("net7.0",
                            StringComparison.InvariantCultureIgnoreCase))
                    {
                        return Project.DotNetVersion.Net_7;
                    }
                }
            }
        }

        return Project.DotNetVersion.Unknown;
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