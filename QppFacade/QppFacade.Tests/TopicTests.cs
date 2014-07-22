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
            var directory = "Assets";
            var topicPath = "Assets\\topic.xml";
            var xDocument = XDocument.Load(topicPath);
            _assetId = _sut.UploadAssetModelFromDirectory(
                AssetModel.FromFile(topicPath)
                    .With(PhoenixAttributes.WORKFLOW, CustomWorkflows.Document)
                    .With(PhoenixAttributes.STATUS, CustomStatuses.Published)
                    .With(PhoenixAttributes.CONTENT_TYPE, CustomContentTypes.IHSDocument)
                    .With(PhoenixAttributes.COLLECTION, CustomCollections.HomeTest)
                    .WithPicture(
                        AssetModel.FromFile("Assets\\just_image.jpg")
                            .With(PhoenixAttributes.CONTENT_TYPE, new PhoenixValue(DefaultContentTypes.PICTURE, "Picture"))
                            .With(PhoenixAttributes.WORKFLOW, CustomWorkflows.Default)
                            .With(PhoenixAttributes.STATUS, CustomStatuses.Default)
                            .With(PhoenixAttributes.COLLECTION, CustomCollections.HomeTest),
                        xDocument)
                    .WithTableSpreadsheet(
                        AssetModel.FromFile("Assets\\tableSpreadsheet.xlsx")
                            .With(PhoenixAttributes.CONTENT_TYPE, CustomContentTypes.ObjectSourceSpreadsheet)
                            .With(PhoenixAttributes.WORKFLOW, CustomWorkflows.ObjectSource)
                            .With(PhoenixAttributes.STATUS, CustomStatuses.ReadyForDataAdminUpdate)
                            .With(PhoenixAttributes.COLLECTION, CustomCollections.HomeTest),
                        xDocument).WithChart(
                            chart: AssetModel.FromFile("Assets\\chart.jpg")
                                .With(PhoenixAttributes.CONTENT_TYPE, new PhoenixValue(DefaultContentTypes.PICTURE, "Picture"))
                                .With(PhoenixAttributes.WORKFLOW, CustomWorkflows.Default)
                                .With(PhoenixAttributes.STATUS, CustomStatuses.Default)
                                .With(PhoenixAttributes.COLLECTION, CustomCollections.HomeTest),
                            fromSpreadsheet: AssetModel.FromFile("Assets\\excelChartSpreadsheet.xlsx")
                                .With(PhoenixAttributes.CONTENT_TYPE, CustomContentTypes.ObjectSourceSpreadsheet)
                                .With(PhoenixAttributes.WORKFLOW, CustomWorkflows.ObjectSource)
                                .With(PhoenixAttributes.STATUS, CustomStatuses.ReadyForDataAdminUpdate)
                                .With(PhoenixAttributes.COLLECTION, CustomCollections.HomeTest),
                            xml: xDocument),
                new DirectoryInfo(directory)
                );
            _topic = _sut.GetAssetModel(_assetId);
        };

        private It should_upload_pictures = () => _topic.Relations.Count(r => r.RelationType == (long) DefaultRelationTypes.XML_COMP_REFERENCE).ShouldEqual(2);

        private It should_upload_tables = () => _topic.Relations.Count(r => r.RelationType == (long) CustomRelations.TableSource).ShouldEqual(1);

        private It should_upload_chart_spreadsheets = () =>
            _topic.Relations
                  .Where(r => r.RelationType == (long) DefaultRelationTypes.XML_COMP_REFERENCE).Any(
                picture =>
                    picture.AssetModel.Relations.Count(r => r.RelationType == (long) CustomRelations.ChartSource) == 1)
                  .ShouldBeTrue();


        private Cleanup after = () => _sut.DeleteAssetModel(_topic);

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