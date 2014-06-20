using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

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
            this.With(PhoenixAttributes.WORKFLOW, "Document Workflow")
                .With(PhoenixAttributes.STATUS, "Published")
                .With(PhoenixAttributes.FILE_EXTENSION, "xml");
        }

        public Topic AddPictureReference(XmlReference<Picture> pictureReference)
        {
            _pictures.Add(pictureReference);
            return this;
        }

        public Topic WithPicture(Picture picture)
        {
            var imageTag =
                _xml.Descendants("image")
                    .FirstOrDefault(image => image.Attribute("href").Value.ToLower() == ((string) picture[PhoenixAttributes.NAME].Value).ToLower());
            return AddPictureReference(new XmlReference<Picture>(imageTag.AbsoluteXPath(), picture));
        }

        protected override Stream CreateStream()
        {
            Stream stream = new MemoryStream();
            _xml.Save(stream);
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }

        public IEnumerable<XmlReference<Picture>> Pictures
        {
            get { return _pictures; }
        }

        public IEnumerable<TableSourceReference> Tables
        {
            get { return _tables; }
        }

        public Topic AddTableReference(TableSourceReference tableReference)
        {
            _tables.Add(tableReference);
            return this;
        }

        public Topic WithTableSpreadsheet(FileAsset tableSpreadsheet)
        {
            var tables =
                _xml.Descendants("table")
                    .Where(image => image.Attribute("Location").Value.ToLower() == ((string) tableSpreadsheet[PhoenixAttributes.NAME].Value).ToLower());
            foreach (var tableTag in tables)
            {
                _tables.Add(
                    new TableSourceReference(tableSpreadsheet)
                        .With(PhoenixAttributes.XPATH, tableTag.AbsoluteXPath())
                        .With(PhoenixAttributes.InRangeName, tableTag.Attribute("InRangeName").Value)
                        .With(PhoenixAttributes.InRangeValue, tableTag.Attribute("InRangeValue").Value)
                        .With(PhoenixAttributes.OutRange, tableTag.Attribute("OutRange").Value)
                    );
            }
            return this;
        }

        public Topic WithChart(Picture chart, FileAsset fromSpreadsheet)
        {
            var spreadsheetName = ((string) fromSpreadsheet[PhoenixAttributes.NAME].Value);
            var chartName = ((string) chart[PhoenixAttributes.NAME].Value);
            var chartTag = _xml.Descendants("image").SingleOrDefault(
                image => image.HasAttribute("Location") &&
                         image.HasAttribute("href") &&
                         String.Equals(image.Attribute("Location").Value, spreadsheetName, StringComparison.CurrentCultureIgnoreCase) &&
                         String.Equals(image.Attribute("href").Value, chartName, StringComparison.CurrentCultureIgnoreCase));
            if (chartTag == null)
                throw new ApplicationException("Unable to find chart in content");

            chart.WithChartReference(
                new ChartSourceReference(fromSpreadsheet)
                    .With(PhoenixAttributes.InRangeName, chartTag.Attribute("InRangeName").Value)
                    .With(PhoenixAttributes.InRangeValue, chartTag.Attribute("InRangeValue").Value)
                    .With(PhoenixAttributes.OutSheet, chartTag.Attribute("OutSheet").Value)
                    .With(PhoenixAttributes.OutChartName, chartTag.Attribute("OutChartName").Value));

            _pictures.Add(new XmlReference<Picture>(chartTag.AbsoluteXPath(), chart));
            return this;
        }
    }
}