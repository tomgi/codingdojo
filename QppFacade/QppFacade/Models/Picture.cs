using System.IO;
using com.quark.qpp.core.content.service.constants;
using IHS.Phoenix.QPP;

namespace QppFacade
{
    public class Picture : FileAsset
    {
        public ChartSourceReference Chart { get; private set; }

        public bool IsChart
        {
            get { return Chart != null; }
        }

        public Picture()
        {
        }

        public Picture(string filePath)
            : base(filePath)
        {
            this.With(PhoenixAttributes.CONTENT_TYPE, new PhoenixValue(DefaultContentTypes.PICTURE, "Picture"))
                .With(PhoenixAttributes.NAME, Path.GetFileName(filePath))
                .With(PhoenixAttributes.ORIGINAL_FILENAME, Path.GetFileName(filePath))
                .With(PhoenixAttributes.FILE_EXTENSION, Path.GetExtension(filePath))
                .With(PhoenixAttributes.WORKFLOW, CustomWorkflows.Default)
                .With(PhoenixAttributes.STATUS, CustomStatuses.Default);
        }

        public Picture WithChartReference(ChartSourceReference chartReference)
        {
            Chart = chartReference;
            return this;
        }
    }
}