using System;
using System.Collections.Generic;
using System.Linq;
using com.quark.qpp.core.asset.service.dto;
using com.quark.qpp.core.attribute.service.constants;
using com.quark.qpp.core.attribute.service.dto;
using IHS.Phoenix.QPP.Facade.SoapFacade.QppAttributes;

namespace QppFacade
{
    public abstract class AttributeBag
    {
        private IDictionary<Type, Func<Type, AttributeValue>> _funcs;
        protected readonly IDictionary<IAttribute, object> _attributes = new Dictionary<IAttribute, object>();

        public AttributeBag()
        {
            _funcs.Add(typeof(string), type => new AttributeValue{type = AttributeValueTypes.TEXT,});
        }

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

        public T Get<T>(IAttribute<T> attribute)
        {
            return default(T);
        }

        public void Set<T>(IAttribute<T> attribute, T value)
        {
            _attributes[attribute] = value;
        }

        public AttributeValue[] GimmeAttributeValues()
        {
            return null;
            //TODO _attributes.Select(attr => _funcs[attr.Value.GetType()].ToAttributeValue(attr.Value)).ToArray();
        }
    }

    public static class AssetExtensions
    {
        public static T With<T>(this T asset, IAttribute attributeId, object value) where T : AttributeBag
        {
            asset[attributeId] = value;
            return asset;
        }

        public static T With<T,TValue>(this T asset, IAttribute<TValue> attributeId, TValue value) where T : AttributeBag
        {
            asset.Set(attributeId, value);
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