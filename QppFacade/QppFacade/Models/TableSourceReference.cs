namespace QppFacade
{
    public class TableSourceReference : AttributeBag, IReference<FileAsset>
    {
        public FileAsset AssetModel { get; private set; }

        public long RelationType
        {
            get { return 1000; }
        }

        public TableSourceReference(FileAsset assetModel)
        {
            AssetModel = assetModel;
        }
    }
}