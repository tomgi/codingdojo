namespace QppFacade
{
    public interface IReference<out TAssetModel> 
    {
        TAssetModel AssetModel { get; }
    }
}