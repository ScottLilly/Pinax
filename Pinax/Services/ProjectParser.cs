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
            Version = GetVersion(projectFileText)
        };

        project.Packages.AddRange(GetPackages(projectFileText));

        return project;
    }

    public static List<Package> GetPackages(string projectFileText)
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

    public static Project.DotNetVersion GetVersion(string projectFileText)
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
                        case "v4.5":
                            return Project.DotNetVersion.Framework_4_5;
                    }
                }

                // Check for .NET 6 version
                var targetFramework =
                    propertyGroup.Element("TargetFramework");

                if (targetFramework?.Value != null)
                {
                    if (targetFramework.Value.StartsWith("net6.0", 
                            StringComparison.InvariantCultureIgnoreCase))
                    {
                        return Project.DotNetVersion.Net_6;
                    }

                    if (targetFramework.Value.StartsWith("net5.0",
                            StringComparison.InvariantCultureIgnoreCase))
                    {
                        return Project.DotNetVersion.Net_5;
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