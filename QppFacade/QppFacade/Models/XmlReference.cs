using System.Collections.Generic;
using com.quark.qpp.core.attribute.service.constants;
using com.quark.qpp.core.relation.service.constants;

namespace QppFacade
{
    public class XmlReference<TAssetModel> : IReference<TAssetModel>, IAttributeBag where TAssetModel : FileAsset
    {
        public TAssetModel AssetModel { get; set; }

        public long RelationType {
            get
            {
                return DefaultRelationTypes.XML_COMP_REFERENCE;
            }
        }

        private readonly Dictionary<object, object> _attributes = new Dictionary<object, object>();

        public Dictionary<object, object> Attributes
        {
            get { return _attributes; }
        }

        public XmlReference(string xPath, TAssetModel assetModel)
        {
            AssetModel = assetModel;
            this.With(DefaultAttributes.XPATH,xPath);
        }
        public XmlReference(TAssetModel assetModel)
        {
            AssetModel = assetModel;
        }
    }
}