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
            this.With(DefaultAttributes.CONTENT_TYPE, DefaultContentTypes.PICTURE)
            .With(DefaultAttributes.NAME, Path.GetFileName(filePath))
            .With(DefaultAttributes.ORIGINAL_FILENAME, Path.GetFileName(filePath))
            .With(DefaultAttributes.FILE_EXTENSION, Path.GetExtension(filePath))
            .With(DefaultAttributes.WORKFLOW, "Default Workflow")
            .With(DefaultAttributes.STATUS, "Default");
        }

        public Picture WithChartReference(ChartSourceReference chartReference)
        {
            Chart = chartReference;
            return this;
        }
    }
}