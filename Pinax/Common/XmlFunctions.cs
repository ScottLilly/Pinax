using System.Xml.Linq;

namespace Pinax.Common;

public static class XmlFunctions
{
    public static void RemoveNamespacePrefix(XElement element)
    {
        element.Name = element.Name.LocalName;

        var attributes = element.Attributes().ToArray();

        element.RemoveAttributes();

        foreach (XAttribute attribute in attributes)
        {
            element.Add(new XAttribute(attribute.Name.LocalName, attribute.Value));
        }

        foreach (XElement child in element.Descendants())
        {
            RemoveNamespacePrefix(child);
        }
    }
}