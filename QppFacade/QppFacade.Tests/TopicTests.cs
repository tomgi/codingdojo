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
    [Subject(typeof (Topic), "Category")]
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
                    .With(PhoenixAttributes.CONTENT_TYPE, CustomContentTypes.IHSDocument)
                    .With(PhoenixAttributes.NAME, "topic.xml")
                    .With(PhoenixAttributes.COLLECTION,  CustomCollections.HomeTest)
                    .With(PhoenixAttributes.ORIGINAL_FILENAME, "topic.xml")
                    .With(PhoenixAttributes.DITA_TITLE, "topic")
                    .WithPicture(
                        new Picture("Assets\\just_image.jpg")
                            .With(PhoenixAttributes.COLLECTION,  CustomCollections.HomeTest))
                    .WithTableSpreadsheet(
                        new FileAsset("Assets\\tableSpreadsheet.xlsx")
                            .With(PhoenixAttributes.CONTENT_TYPE, CustomContentTypes.ObjectSourceSpreadsheet)
                            .With(PhoenixAttributes.WORKFLOW, CustomWorkflows.ObjectSource)
                            .With(PhoenixAttributes.STATUS, CustomStatuses.ReadyForDataAdminUpdate)
                            .With(PhoenixAttributes.COLLECTION,  CustomCollections.HomeTest))
                    .WithChart(
                        chart: new Picture("Assets\\chart.jpg").With(PhoenixAttributes.COLLECTION,  CustomCollections.HomeTest),
                        fromSpreadsheet: new FileAsset("Assets\\excelChartSpreadsheet.xlsx")
                            .With(PhoenixAttributes.CONTENT_TYPE, CustomContentTypes.ObjectSourceSpreadsheet)
                            .With(PhoenixAttributes.WORKFLOW, CustomWorkflows.ObjectSource)
                            .With(PhoenixAttributes.STATUS, CustomStatuses.ReadyForDataAdminUpdate)
                            .With(PhoenixAttributes.COLLECTION,  CustomCollections.HomeTest))
                );
            _topic = _sut.GetTopicWithReferencedItems(_assetId);
        };

        private It should_upload_pictures = () => _topic.Pictures.Count().ShouldEqual(2);

        private It should_upload_tables = () => _topic.Tables.Count().ShouldEqual(1);

        private It should_upload_chart_spreadsheets = () => _topic.Pictures.Any(picture => picture.AssetModel.IsChart).ShouldBeTrue();


        private Cleanup after = () => _sut.Delete(_topic);

        private static Qpp _sut;
        private static long _assetId;
        private static WindsorContainer _container;
        private static FileAsset _fileUpdated;
        private static Topic _topic;

        private static MemoryStream GenerateStreamFromString(string value)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(value ?? ""));
        }
    }
}