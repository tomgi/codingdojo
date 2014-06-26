using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Castle.Windsor;
using IHS.Phoenix.QPP;
using IHS.Phoenix.QPP.Facade.SoapFacade;
using Machine.Specifications;
using QppFacade.Models;

namespace QppFacade.Tests
{
    [Subject(typeof (Topic), "Category")]
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
            _assetId = _sut.UploadDitaMap(
                new DitaMap(XDocument.Parse(File.ReadAllText("Assets\\ditamap.xml")))
                    .With(PhoenixAttributes.CONTENT_TYPE, CustomContentTypes.IHSDocumentMap)
                    .With(PhoenixAttributes.NAME, "ditamap.xml")
                    .With(PhoenixAttributes.COLLECTION,  CustomCollections.HomeTest)
                    .With(PhoenixAttributes.ORIGINAL_FILENAME, "ditamap.xml")
                    .With(PhoenixAttributes.DITA_TITLE, "ditamap")
                    .WithTopic(
                        new Topic(XDocument.Parse(File.ReadAllText("Assets\\topic1.xml")))
                            .With(PhoenixAttributes.CONTENT_TYPE, CustomContentTypes.IHSDocument)
                            .With(PhoenixAttributes.NAME, "topic1.xml")
                            .With(PhoenixAttributes.COLLECTION,  CustomCollections.HomeTest)
                            .With(PhoenixAttributes.ORIGINAL_FILENAME, "topic1.xml")
                            .With(PhoenixAttributes.DITA_TITLE, "topic"))
                );
            _ditaMap = _sut.GetDitaMapWithReferencedItems(_assetId);
        };

        private It should_upload_topics = () => _ditaMap.Topics.Count().ShouldEqual(1);

        private Cleanup after = () => _sut.Delete(_ditaMap);

        private static Qpp _sut;
        private static long _assetId;
        private static WindsorContainer _container;
        private static FileAsset _fileUpdated;
        private static DitaMap _ditaMap;

        private static MemoryStream GenerateStreamFromString(string value)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(value ?? ""));
        }
    }
}