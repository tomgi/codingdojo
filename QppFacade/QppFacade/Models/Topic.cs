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
            this.With(PhoenixAttributes.WORKFLOW.WithValue( "Document Workflow"))
                .With(PhoenixAttributes.STATUS.WithValue( "Published"))
                .With(PhoenixAttributes.FILE_EXTENSION.WithValue( "xml"));            
        }

        public Topic WithPictureReference(XmlReference<Picture> pictureReference)
        {
            _pictures.Add(pictureReference);
            return this;
        }        
        public Topic WithPicture(Picture picture)
        {
            var imageTag = _xml.Descendants("image").FirstOrDefault(image => image.Attribute("href").Value.ToLower() == ((string)picture[PhoenixAttributes.NAME]).ToLower());
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
            var tables = _xml.Descendants("table").Where(image => image.Attribute("Location").Value.ToLower() == ((string)tableSpreadsheet[PhoenixAttributes.NAME]).ToLower());
            foreach (var tableTag in tables)
            {
                _tables.Add(
                    new TableSourceReference(tableSpreadsheet)
                        .With(PhoenixAttributes.XPATH.WithValue( tableTag.AbsoluteXPath()))
                        .With(PhoenixAttributes.InRangeName.WithValue(tableTag.Attribute("InRangeName").Value))
                        .With(PhoenixAttributes.InRangeValue.WithValue( tableTag.Attribute("InRangeValue").Value))
                        .With(PhoenixAttributes.OutRange.WithValue( tableTag.Attribute("OutRange").Value))
                    );
            }
            return this;
        }

        public Topic WithChart(Picture chart, FileAsset fromSpreadsheet)
        {
            var spreadsheetName = ((string)fromSpreadsheet[PhoenixAttributes.NAME]);
            var chartName = ((string)chart[PhoenixAttributes.NAME]);
            var chartTag = _xml.Descendants("image").SingleOrDefault(image => image.HasAttribute("Location") && 
                                                                              image.HasAttribute("href") &&
                                                                              String.Equals(image.Attribute("Location").Value, spreadsheetName, StringComparison.CurrentCultureIgnoreCase) && 
                                                                              String.Equals(image.Attribute("href").Value, chartName, StringComparison.CurrentCultureIgnoreCase));
            if(chartTag==null)
                throw new ApplicationException("Unable to find chart in content");

            chart.WithChartReference(new ChartSourceReference(fromSpreadsheet)
                .With(PhoenixAttributes.InRangeName.WithValue( chartTag.Attribute("InRangeName").Value))
                .With(PhoenixAttributes.InRangeValue.WithValue( chartTag.Attribute("InRangeValue").Value))
                .With(PhoenixAttributes.OutSheet.WithValue( chartTag.Attribute("OutSheet").Value))
                .With(PhoenixAttributes.OutChartName.WithValue( chartTag.Attribute("OutChartName").Value)));

            _pictures.Add(new XmlReference<Picture>(chartTag.AbsoluteXPath(), chart));
            return this;
        }
    }
}