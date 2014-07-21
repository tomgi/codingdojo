using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Castle.Windsor;
using com.quark.qpp.core.content.service.constants;
using com.quark.qpp.core.relation.service.constants;
using IHS.Phoenix.QPP;
using IHS.Phoenix.QPP.Facade.SoapFacade;
using Machine.Specifications;

namespace QppFacade.Tests
{
    [Subject(typeof (AssetModel), "Category")]
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
            var topicPath = "Assets\\topic.xml";
            var xDocument = XDocument.Load(topicPath);
            _assetId = _sut.UploadAssetFromFile(
                AssetModel.FromFile(topicPath)
                    .With(PhoenixAttributes.WORKFLOW, CustomWorkflows.Document)
                    .With(PhoenixAttributes.STATUS, CustomStatuses.Published)
                    .With(PhoenixAttributes.CONTENT_TYPE, CustomContentTypes.IHSDocument)
                    .With(PhoenixAttributes.COLLECTION, CustomCollections.HomeTest)
                    .With(PhoenixAttributes.DITA_TITLE, "topic")
                    .WithPicture(
                        AssetModel.FromFile("Assets\\just_image.jpg")
                            .With(
                                PhoenixAttributes.CONTENT_TYPE,
                                new PhoenixValue(DefaultContentTypes.PICTURE, "Picture"))
                            .With(PhoenixAttributes.WORKFLOW, CustomWorkflows.Default)
                            .With(PhoenixAttributes.STATUS, CustomStatuses.Default)
                            .With(PhoenixAttributes.COLLECTION, CustomCollections.HomeTest),
                        xDocument)
                    .WithTableSpreadsheet(
                        AssetModel.FromFile("Assets\\tableSpreadsheet.xlsx")
                            .With(
                                PhoenixAttributes.CONTENT_TYPE,
                                CustomContentTypes.ObjectSourceSpreadsheet)
                            .With(PhoenixAttributes.WORKFLOW, CustomWorkflows.ObjectSource)
                            .With(PhoenixAttributes.STATUS, CustomStatuses.ReadyForDataAdminUpdate)
                            .With(PhoenixAttributes.COLLECTION, CustomCollections.HomeTest),
                        xDocument).WithChart(
                            chart: AssetModel.FromFile("Assets\\chart.jpg")
                                .With(
                                    PhoenixAttributes.CONTENT_TYPE,
                                    new PhoenixValue(DefaultContentTypes.PICTURE, "Picture"))
                                .With(PhoenixAttributes.WORKFLOW, CustomWorkflows.Default)
                                .With(PhoenixAttributes.STATUS, CustomStatuses.Default)
                                .With(PhoenixAttributes.COLLECTION, CustomCollections.HomeTest),
                            fromSpreadsheet: AssetModel.FromFile("Assets\\excelChartSpreadsheet.xlsx")
                                .With(
                                    PhoenixAttributes.CONTENT_TYPE,
                                    CustomContentTypes.ObjectSourceSpreadsheet)
                                .With(PhoenixAttributes.WORKFLOW, CustomWorkflows.ObjectSource)
                                .With(
                                    PhoenixAttributes.STATUS,
                                    CustomStatuses.ReadyForDataAdminUpdate)
                                .With(PhoenixAttributes.COLLECTION, CustomCollections.HomeTest),
                            xml: xDocument),
                new FileInfo(topicPath)
                );
            _topic = _sut.GetTopicWithReferencedItems(_assetId);
        };

        private It should_upload_pictures = () => _topic.RelationsOfType(DefaultRelationTypes.XML_COMP_REFERENCE).Count().ShouldEqual(2);

        private It should_upload_tables = () => _topic.RelationsOfType(CustomRelations.TableSource).Count().ShouldEqual(1);

        private It should_upload_chart_spreadsheets = () =>
            _topic.RelationsOfType(DefaultRelationTypes.XML_COMP_REFERENCE).Any(
                picture =>
                    picture.AssetModel.RelationsOfType(CustomRelations.ChartSource).Count() == 1)
                  .ShouldBeTrue();


        private Cleanup after = () => _sut.Delete(_topic);

        private static Qpp _sut;
        private static long _assetId;
        private static WindsorContainer _container;
        private static AssetModel _fileUpdated;
        private static AssetModel _topic;

        private static MemoryStream GenerateStreamFromString(string value)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(value ?? ""));
        }
    }
}