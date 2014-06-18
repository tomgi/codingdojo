using System.Collections.Generic;
using com.quark.qpp.core.attribute.service.constants;
using com.quark.qpp.core.relation.service.constants;
using IHS.Phoenix.QPP.Facade.SoapFacade.QppAttributes;

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

        private readonly ISet<IHaveNameAndId> _attributes = new HashSet<IHaveNameAndId>();
        public ISet<IHaveNameAndId> Attributes
        {
            get { return _attributes; }
        }

        public XmlReference(string xPath, TAssetModel assetModel)
        {
            AssetModel = assetModel;
            this.With(PhoenixAttributes.XPATH.WithValue(xPath));
        }
        public XmlReference(TAssetModel assetModel)
        {
            AssetModel = assetModel;
        }
    }
}