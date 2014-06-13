using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using com.quark.qpp.core.attribute.service.constants;

namespace QppFacade
{
    public class DitaMap : FileAsset
    {
        public DitaMap(XDocument xml)
        {
        }

        public long Id { get; set; }
    }

    public class Topic : FileAsset
    {
        public Topic(XDocument xml)
        {
        }

        public long Id { get; set; }
    }

    public class Picture : FileAsset
    {
        public Picture(string filePath)
            : base(filePath)
        {
        }

        public long Id { get; set; }
    }

    public class FileAsset
    {
        public long Id { get; set; }

        public Dictionary<object, object> Attributes
        {
            get { return _attributes; }
        }

        public Stream Content {
            get
            {
                return File.Open(_filePath, FileMode.Open, FileAccess.Read);
            }
        }

        private readonly Dictionary<object, object> _attributes = new Dictionary<object, object>();
        private readonly string _filePath;

        public FileAsset(string filePath)
        {
            _filePath = filePath;
        }

        public FileAsset()
        {
        }

        private FileAsset With(object attributeId, object attributeValue)
        {
            if (false == Attributes.ContainsKey(attributeId))
                Attributes.Add(attributeId, attributeValue);
            Attributes[attributeId] = attributeValue;
            return this;
        }

        public FileAsset With(long attributeId, object attributeValue)
        {
            With(attributeId as object, attributeValue);
            return this;
        }
        public FileAsset With(string attributeName, object attributeValue)
        {
            With(attributeName as object, attributeValue);
            return this;
        }
    }
}