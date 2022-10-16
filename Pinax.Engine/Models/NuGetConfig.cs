namespace Pinax.Engine.Models;

public class NuGetConfig
{
    [SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public class configuration
    {
        private configurationAdd[] _packageSourcesField;

        [System.Xml.Serialization.XmlArrayItemAttribute("add", IsNullable = false)]
        public configurationAdd[] packageSources
        {
            get => _packageSourcesField;
            set => _packageSourcesField = value;
        }
    }

    [SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public class configurationAdd
    {
        private string _keyField;
        private string _valueField;
        private int _protocolVersionField;
        private bool _protocolVersionFieldSpecified;

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string key
        {
            get => _keyField;
            set => _keyField = value;
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string value
        {
            get => _valueField;
            set => _valueField = value;
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int protocolVersion
        {
            get => _protocolVersionField;
            set => _protocolVersionField = value;
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool protocolVersionSpecified
        {
            get => _protocolVersionFieldSpecified;
            set => _protocolVersionFieldSpecified = value;
        }

        public bool IsWebSource =>
            value.StartsWith("http", StringComparison.InvariantCultureIgnoreCase) &&
            value.EndsWith("index.json", StringComparison.InvariantCultureIgnoreCase);
    }
}