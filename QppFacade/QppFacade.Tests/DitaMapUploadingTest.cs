using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Castle.Windsor;
using IHS.Phoenix.QPP;
using IHS.Phoenix.QPP.Facade.SoapFacade;
using Machine.Specifications;

namespace QppFacade.Tests
{
    [Subject(typeof (AssetModel), "Category")]
    public class when_uploading_dita
    {
        private Establish context = () =>
        {
            _container = new WindsorContainer();
            _container.Install(new QppFacadeWindsorInstaller());
            _sut = _container.Resolve<Qpp>();
        };


        private Because of = () =>
        {
            var ditamapXml = "Assets\\ditamap.xml";
            var xDocument = XDocument.Load(ditamapXml);
            _assetId = _sut.UploadDitaMap(
                AssetModel.FromFile(ditamapXml)
                    .With(PhoenixAttributes.WORKFLOW, CustomWorkflows.Document)
                    .With(PhoenixAttributes.STATUS, CustomStatuses.Published)
                    .With(PhoenixAttributes.CONTENT_TYPE, CustomContentTypes.IHSDocumentMap)
                    .With(PhoenixAttributes.COLLECTION,  CustomCollections.HomeTest)
                    .With(PhoenixAttributes.ORIGINAL_FILENAME, "ditamap.xml")
                    .With(PhoenixAttributes.DITA_TITLE, "ditamap")
                    .WithTopic(AssetModel.FromFile("Assets\\topic1.xml")
                        .With(PhoenixAttributes.WORKFLOW, CustomWorkflows.Document)
                        .With(PhoenixAttributes.STATUS, CustomStatuses.Published)    
                        .With(PhoenixAttributes.CONTENT_TYPE, CustomContentTypes.IHSDocument)
                        .With(PhoenixAttributes.COLLECTION,  CustomCollections.HomeTest)
                        .With(PhoenixAttributes.ORIGINAL_FILENAME, "topic1.xml")
                        .With(PhoenixAttributes.DITA_TITLE, "topic"), xDocument)
                );
            _ditaMap = _sut.GetDitaMapWithReferencedItems(_assetId);
        };

        private It should_upload_topics = () => _ditaMap.Relations.Count().ShouldEqual(1);

        private Cleanup after = () => _sut.DeleteDitaMap(_ditaMap);

        private static Qpp _sut;
        private static long _assetId;
        private static WindsorContainer _container;
        private static AssetModel _fileUpdated;
        private static AssetModel _ditaMap;

        private static MemoryStream GenerateStreamFromString(string value)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(value ?? ""));
        }
    }
}