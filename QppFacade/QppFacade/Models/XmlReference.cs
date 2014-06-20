using com.quark.qpp.core.relation.service.constants;

namespace QppFacade
{
    public class XmlReference<TAssetModel> : AttributeBag, IReference<TAssetModel> where TAssetModel : FileAsset
    {
        public TAssetModel AssetModel { get; set; }

        public long RelationType
        {
            get { return DefaultRelationTypes.XML_COMP_REFERENCE; }
        }

        public XmlReference(string xPath, TAssetModel assetModel)
        {
            AssetModel = assetModel;
            this.With(PhoenixAttributes.XPATH, xPath);
        }

        public XmlReference(TAssetModel assetModel)
        {
            AssetModel = assetModel;
        }
    }
}