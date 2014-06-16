using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using com.quark.qpp.core.attribute.service.constants;
using com.quark.qpp.core.content.service.constants;

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
        private readonly XDocument _xml;
        private readonly IList<XmlReference<Picture>> _pictures = new List<XmlReference<Picture>>();
        private readonly IList<TableSourceReference> _tables = new List<TableSourceReference>();

        public Topic()
        {
        }

        public Topic(XDocument xml)
        {
            _xml = xml;
            With(DefaultAttributes.WORKFLOW, "Document Workflow").
                With(DefaultAttributes.STATUS, "Published");
            //With(DefaultAttributes.FILE_EXTENSION, "xml");
        }

        public new Topic With(long attributeId, object attributeValue)
        {
            return (Topic) base.With(attributeId, attributeValue);
        }

        public new Topic With(string attributeName, object attributeValue)
        {
            return (Topic)base.With(attributeName, attributeValue);
        }


        public Topic WithPictureReference(XmlReference<Picture> pictureReference)
        {
            _pictures.Add(pictureReference);
            return this;
        }        
        public Topic WithPicture(Picture picture)
        {
            var imageTag = _xml.Descendants("image").FirstOrDefault(image => image.Attribute("href").Value.ToLower() == ((string)picture.Attributes[DefaultAttributes.NAME]).ToLower());
            _pictures.Add(new XmlReference<Picture>(imageTag.AbsoluteXPath(),picture));
            return this;
        }

        public override Stream Content
        {
            get
            {
                Stream stream = new MemoryStream();
                _xml.Save(stream);
                stream.Seek(0, SeekOrigin.Begin);
                return stream;
            }
        }

        public long Id { get; set; }
        public IEnumerable<XmlReference<Picture>> Pictures {get { return _pictures; }}
        public IEnumerable<TableSourceReference>  Tables {get { return _tables; }}

        public Topic WithTableReference(TableSourceReference tableReference)
        {
            _tables.Add(tableReference);
            return this;
        }

        public Topic WithTableSpreadsheet(FileAsset tableSpreadsheet)
        {
            var tables = _xml.Descendants("table").Where(image => image.Attribute("Location").Value.ToLower() == ((string)tableSpreadsheet.Attributes[DefaultAttributes.NAME]).ToLower());
            foreach (var tableTag in tables)
            {
                _tables.Add(
                    new TableSourceReference(tableSpreadsheet)
                        .With("XPath", tableTag.AbsoluteXPath())
                        .With("InRangeName", tableTag.Attribute("InRangeName").Value)
                        .With("InRangeValue", tableTag.Attribute("InRangeValue").Value)
                        .With("OutRange", tableTag.Attribute("OutRange").Value)
                    );
            }
            return this;
        }
    }

    public class Picture : FileAsset
    {
        public Picture()
        {
        }

        public Picture(string filePath)
            : base(filePath)
        {
            With(DefaultAttributes.CONTENT_TYPE, DefaultContentTypes.PICTURE);
            With(DefaultAttributes.NAME, Path.GetFileName(filePath));
            With(DefaultAttributes.ORIGINAL_FILENAME, Path.GetFileName(filePath));
            With(DefaultAttributes.WORKFLOW, "Default Workflow");
            With(DefaultAttributes.STATUS, "Default");
        }

        public new Picture With(long attributeId, object attributeValue)
        {
            return (Picture)base.With(attributeId, attributeValue);
        }

        public new Picture With(string attributeName, object attributeValue)
        {
            return (Picture)base.With(attributeName, attributeValue);
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

        public virtual Stream Content {
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
            With(DefaultAttributes.NAME, Path.GetFileName(filePath));
            With(DefaultAttributes.ORIGINAL_FILENAME, Path.GetFileName(filePath));
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