using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using com.quark.qpp.core.attribute.service.constants;

namespace QppFacade
{
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
            this.With(DefaultAttributes.WORKFLOW, "Document Workflow")
                .With(DefaultAttributes.STATUS, "Published")
                .With(DefaultAttributes.FILE_EXTENSION, "xml");            
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

        public Stream Content
        {
            get
            {
                Stream stream = new MemoryStream();
                _xml.Save(stream);
                stream.Seek(0, SeekOrigin.Begin);
                return stream;
            }
        }

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

        public Topic WithChart(Picture chart, FileAsset fromSpreadsheet)
        {
            var spreadsheetName = ((string)fromSpreadsheet.Attributes[DefaultAttributes.NAME]);
            var chartName = ((string)chart.Attributes[DefaultAttributes.NAME]);
            var chartTag = _xml.Descendants("image").SingleOrDefault(image => image.HasAttribute("Location") && 
                                                                              image.HasAttribute("href") &&
                                                                              String.Equals(image.Attribute("Location").Value, spreadsheetName, StringComparison.CurrentCultureIgnoreCase) && 
                                                                              String.Equals(image.Attribute("href").Value, chartName, StringComparison.CurrentCultureIgnoreCase));
            if(chartTag==null)
                throw new ApplicationException("Unable to find chart in content");

            chart.WithChartReference(new ChartSourceReference(fromSpreadsheet)
                .With("InRangeName", chartTag.Attribute("InRangeName").Value)
                .With("InRangeValue", chartTag.Attribute("InRangeValue").Value)
                .With("OutSheet", chartTag.Attribute("OutSheet").Value)
                .With("OutChartName", chartTag.Attribute("OutChartName").Value));

            _pictures.Add(new XmlReference<Picture>(chartTag.AbsoluteXPath(), chart));
            return this;
        }
    }
}