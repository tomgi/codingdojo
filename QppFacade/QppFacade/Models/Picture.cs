using System.IO;
using com.quark.qpp.core.attribute.service.constants;
using com.quark.qpp.core.content.service.constants;

namespace QppFacade
{
    public class Picture : FileAsset
    {
        public ChartSourceReference Chart { get; private set; }
        public bool IsChart {
            get { return Chart != null; }
        }

        public Picture()
        {
        }

        public Picture(string filePath)
            : base(filePath)
        {
            this.With(PhoenixAttributes.CONTENT_TYPE.WithValue(DefaultContentTypes.PICTURE))
            .With(PhoenixAttributes.NAME.WithValue(Path.GetFileName(filePath)))
            .With(PhoenixAttributes.ORIGINAL_FILENAME.WithValue(Path.GetFileName(filePath)))
            .With(PhoenixAttributes.FILE_EXTENSION.WithValue( Path.GetExtension(filePath)))
            .With(PhoenixAttributes.WORKFLOW.WithValue( "Default Workflow"))
            .With(PhoenixAttributes.STATUS.WithValue( "Default"));
        }

        public Picture WithChartReference(ChartSourceReference chartReference)
        {
            Chart = chartReference;
            return this;
        }
    }
}