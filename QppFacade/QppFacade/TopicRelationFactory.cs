using System;
using System.Linq;
using System.Xml.Linq;
using com.quark.qpp.core.relation.service.constants;
using IHS.Phoenix.QPP;

namespace QppFacade
{
    public static class TopicRelationFactory
    {
        public static AssetModel WithChart(this AssetModel topic, AssetModel chart, AssetModel fromSpreadsheet, XDocument xml)
        {
            var spreadsheetName = fromSpreadsheet.Get(PhoenixAttributes.NAME);
            var chartName = chart.Get(PhoenixAttributes.NAME);
            var chartTag = xml.Descendants("image").SingleOrDefault(
                image => image.HasAttribute("Location") &&
                         image.HasAttribute("href") &&
                         String.Equals(image.Attribute("Location").Value, spreadsheetName, StringComparison.CurrentCultureIgnoreCase) &&
                         String.Equals(image.Attribute("href").Value, chartName, StringComparison.CurrentCultureIgnoreCase));
            if (chartTag == null)
                throw new ApplicationException("Unable to find chart in content");

            chart.WithRelation(
                Relation.To(fromSpreadsheet).OfType(CustomRelations.ChartSource)
                        .With(PhoenixAttributes.InRangeName, chartTag.Attribute("InRangeName").Value)
                        .With(PhoenixAttributes.InRangeValue, chartTag.Attribute("InRangeValue").Value)
                        .With(PhoenixAttributes.OutSheet, chartTag.Attribute("OutSheet").Value)
                        .With(PhoenixAttributes.OutChartName, chartTag.Attribute("OutChartName").Value));

            topic.Relations.Add(Relation.To(chart).OfType(DefaultRelationTypes.XML_COMP_REFERENCE)
                                        .With(PhoenixAttributes.XPATH, chartTag.AbsoluteXPath()));
            return topic;
        }

        public static AssetModel WithPicture(this AssetModel topic, AssetModel picture, XDocument xml)
        {
            var imageTag =
                xml.Descendants("image")
                   .FirstOrDefault(image => image.Attribute("href").Value.ToLower() == picture.Get(PhoenixAttributes.NAME).ToLower());
            
            topic.WithRelation(Relation.To(picture).OfType(DefaultRelationTypes.XML_COMP_REFERENCE)
                                       .With(PhoenixAttributes.XPATH, imageTag.AbsoluteXPath()));
            return topic;
        }

        public static AssetModel WithTableSpreadsheet(this AssetModel topic, AssetModel tableSpreadsheet, XDocument xml)
        {
            var tables =
                xml.Descendants("table")
                   .Where(image => image.Attribute("Location").Value.ToLower() == tableSpreadsheet.Get(PhoenixAttributes.NAME).ToLower());

            foreach (var tableTag in tables)
            {
                topic.Relations.Add(
                    Relation.To(tableSpreadsheet).OfType(CustomRelations.TableSource)
                            .With(PhoenixAttributes.XPATH, tableTag.AbsoluteXPath())
                            .With(PhoenixAttributes.InRangeName, tableTag.Attribute("InRangeName").Value)
                            .With(PhoenixAttributes.InRangeValue, tableTag.Attribute("InRangeValue").Value)
                            .With(PhoenixAttributes.OutRange, tableTag.Attribute("OutRange").Value)
                    );
            }
            return topic;
        }

        public static AssetModel WithTopic(this AssetModel ditaMap, AssetModel topic, XDocument xml)
        {
            var imageTag =
                xml.Descendants("topicref")
                   .FirstOrDefault(topicRef => topicRef.Attribute("href").Value.ToLower() == ((string) topic.Get(PhoenixAttributes.NAME).ToLower()));

            ditaMap.WithRelation(Relation.To(topic)
                .OfType(DefaultRelationTypes.XML_COMP_REFERENCE)
                .With(PhoenixAttributes.XPATH, imageTag.AbsoluteXPath()));
            return ditaMap;
        }
    }
}