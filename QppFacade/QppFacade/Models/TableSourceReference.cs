using System.Collections.Generic;

namespace QppFacade
{
    public class TableSourceReference : IReference<FileAsset>, IAttributeBag
    {
        public FileAsset AssetModel { get; private set; }
        public long RelationType
        {
            get
            {
                return 1000;
            }
        }
        private readonly Dictionary<object, object> _attributes = new Dictionary<object, object>();

        public Dictionary<object, object> Attributes
        {
            get { return _attributes; }
        }

        public TableSourceReference(FileAsset assetModel)
        {
            AssetModel = assetModel;
        }
    }
}