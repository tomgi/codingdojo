namespace QppFacade
{
    public class ChartSourceReference : AttributeBag, IReference<FileAsset>
    {
        public FileAsset AssetModel { get; private set; }

        public long RelationType
        {
            get { return 1001; }
        }

        public ChartSourceReference(FileAsset assetModel)
        {
            AssetModel = assetModel;
        }
    }
}