using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Pinax.Services;

public static class ProjectParser
{
    public enum DotNetVersion
    {
        Unknown,
        Framework_1_0,
        Framework_1_1,
        Framework_2_0,
        Framework_3_0,
        Framework_3_5,
        Framework_4_0,
        Framework_4_5,
        Framework_4_5_1,
        Framework_4_5_2,
        Framework_4_6,
        Framework_4_6_1,
        Framework_4_6_2,
        Framework_4_7,
        Framework_4_7_1,
        Framework_4_7_2,
        Framework_4_8,
        Core_1_0,
        Core_1_1,
        Core_2_0,
        Core_2_1,
        Core_2_2,
        Core_3_0,
        Core_3_1,
        Net_5,
        Net_6,
        Net_7
    }

    public static DotNetVersion GetVersion(string projectFileText)
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
                            return DotNetVersion.Framework_4_5;
                    }
                }

                // Check for .NET 6 version
                var targetFramework =
                    propertyGroup.Element("TargetFramework");

                if (targetFramework != null)
                {
                    switch (targetFramework.Value)
                    {
                        case "net6.0-windows":
                            return DotNetVersion.Net_6;
                    }
                }
            }
        }

        return DotNetVersion.Unknown;
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