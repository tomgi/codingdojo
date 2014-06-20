using System.Collections.Generic;
using System.Linq;
using com.quark.qpp.core.asset.service.dto;
using com.quark.qpp.core.attribute.service.dto;
using IHS.Phoenix.QPP.Facade.SoapFacade.QppAttributes;

namespace QppFacade
{
    public abstract class AttributeBag
    {
        protected readonly IDictionary<IAttribute, object> _attributes = new Dictionary<IAttribute, object>();

        public object this[IAttribute index]
        {
            get
            {
                return _attributes[index];
            }
            set
            {
                _attributes[index] = value;
            }
        }

        public AttributeValue[] GimmeAttributeValues()
        {
            return _attributes.Select(attr => attr.Key.ToAttributeValue(attr.Value)).ToArray();
        }
    }

    public static class AssetExtensions
    {
        public static T With<T>(this T asset, IAttribute attributeId, object value) where T : AttributeBag
        {
            asset[attributeId] = value;
            return asset;
        }

        public static Asset ToAsset(this FileAsset assetModel)
        {
            return new Asset()
            {
                assetId = assetModel.Id,
                attributeValues = assetModel.GimmeAttributeValues()
            };
        }
    }
}