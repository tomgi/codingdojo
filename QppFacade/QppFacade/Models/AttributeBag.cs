using System.Collections.Generic;
using System.Linq;
using com.quark.qpp.core.asset.service.dto;
using com.quark.qpp.core.attribute.service.dto;
using IHS.Phoenix.QPP.Facade.SoapFacade.QppAttributes;

namespace QppFacade
{
    public interface IAttributeBag
    {
        ISet<IHaveNameAndId> Attributes { get; }
        IHaveNameAndId this[IHaveNameAndId index] { get; }    
    }

    public static class AssetExtensions
    {
        public static T With<T>(this T asset, IHaveNameAndId attributeId, object value) where T : IAttributeBag
        {
            asset[attributeId].Value = value;
            return asset;
        }

        public static AttributeValue[] ToAttributeValues(this IEnumerable<IHaveNameAndId> bag)
        {
            return bag.Select(attr => attr.ToAttributeValue()).ToArray();
        }

        public static Asset ToAsset(this FileAsset assetModel)
        {
            return new Asset()
            {
                assetId = assetModel.Id,
                attributeValues = assetModel.Attributes.ToAttributeValues()
                
            };
        }
    }
}