using System.Collections.Generic;

namespace QppFacade
{
    public class ChartSourceReference : IReference<FileAsset>, IAttributeBag
    {
        public FileAsset AssetModel { get; private set; }
        public long RelationType
        {
            get
            {
                return 1001;
            }
        }
        private readonly Dictionary<object, object> _attributes = new Dictionary<object, object>();

        public Dictionary<object, object> Attributes
        {
            get { return _attributes; }
        }

        public ChartSourceReference(FileAsset assetModel)
        {
            AssetModel = assetModel;
        }
    }
}