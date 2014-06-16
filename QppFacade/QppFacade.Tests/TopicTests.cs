using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Castle.Windsor;
using com.quark.qpp.core.attribute.service.constants;
using com.quark.qpp.core.content.service.constants;
using IHS.Phoenix.QPP.Facade.SoapFacade;
using Machine.Specifications;

namespace QppFacade.Tests
{
    [Subject(typeof(Topic), "Category")]
    public class when_doing_different_things_with_topic
    {
        private Establish context = () =>
        {
            _container = new WindsorContainer();
            _container.Install(new QppFacadeWindsorInstaller());
            _sut = _container.Resolve<Qpp>();
        };


        private Because of = () =>
        {
            _assetId = _sut.UploadTopic(
                new Topic(XDocument.Parse(File.ReadAllText("Assets\\topic.xml")))
                    .With(DefaultAttributes.CONTENT_TYPE, "IHS Document")
                    .With(DefaultAttributes.NAME, "topic.xml")
                    .With(DefaultAttributes.COLLECTION, "Home/Test")
                    .With(DefaultAttributes.ORIGINAL_FILENAME, "topic.xml")
                    .With(DefaultAttributes.DITA_TITLE, "topic")
                    .WithPicture(new Picture("Assets\\just_image.jpg").With(DefaultAttributes.COLLECTION, "Home/Test"))
                    .WithTableSpreadsheet(new FileAsset("Assets\\tableSpreadsheet.xlsx")
                                                .With(DefaultAttributes.CONTENT_TYPE, "Object Source Spreadsheet")
                                                .With(DefaultAttributes.WORKFLOW, "Object Source Workflow")
                                                .With(DefaultAttributes.STATUS, "Ready For Data Admin Update")
                                                .With(DefaultAttributes.COLLECTION, "Home/Test"))                    
                );
        };

        private It should_upload_pictures = () =>
        {
            var topic = _sut.GetTopicWithReferencedItems(_assetId);
            topic.Pictures.Count().ShouldEqual(1);
        };

        private It should_upload_tables = () =>
        {
            var topic = _sut.GetTopicWithReferencedItems(_assetId);
            topic.Tables.Count().ShouldEqual(1);
        };


        //private Cleanup after = () => _sut.Delete(_assetId);

        private static Qpp _sut;
        private static long _assetId;
        private static WindsorContainer _container;
        private static FileAsset _fileUpdated;

        private static MemoryStream GenerateStreamFromString(string value)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(value ?? ""));
        }

    }

}