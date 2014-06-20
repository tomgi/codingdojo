using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace QppFacade.Models
{
    public class DitaMap : FileAsset
    {
        private readonly XDocument _xml;
        private readonly IList<XmlReference<Topic>> _topicsReferences = new List<XmlReference<Topic>>();

        public DitaMap()
        {
        }

        public DitaMap(XDocument xml)
        {
            _xml = xml;
            this.With(PhoenixAttributes.WORKFLOW, "Document Workflow")
                .With(PhoenixAttributes.STATUS, "Published")
                .With(PhoenixAttributes.FILE_EXTENSION, "xml");
        }

        public IEnumerable<XmlReference<Topic>> Topics
        {
            get { return _topicsReferences; }
        }

        protected override Stream CreateStream()
        {
            Stream stream = new MemoryStream();
            _xml.Save(stream);
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }

        public DitaMap AddTopicReference(XmlReference<Topic> topicReference)
        {
            _topicsReferences.Add(topicReference);
            return this;
        }

        public DitaMap WithTopic(Topic topic)
        {
            var imageTag =
                _xml.Descendants("topicref")
                    .FirstOrDefault(topicRef => topicRef.Attribute("href").Value.ToLower() == ((string) topic[PhoenixAttributes.NAME].Value).ToLower());
            return AddTopicReference(new XmlReference<Topic>(imageTag.AbsoluteXPath(), topic));
        }
    }
}