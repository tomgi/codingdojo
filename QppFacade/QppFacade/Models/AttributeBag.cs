using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using com.quark.qpp.core.asset.service.dto;
using com.quark.qpp.core.attribute.service.constants;
using com.quark.qpp.core.attribute.service.dto;
using IHS.Phoenix.QPP;
using IHS.Phoenix.QPP.Facade.SoapFacade.QppAttributes;

namespace QppFacade
{
    public abstract class AttributeBag
    {
        protected readonly IDictionary<IAttribute, object> _attributes = new Dictionary<IAttribute, object>();

        private readonly Dictionary<Type, IDictionary> _nonGenDict = new Dictionary<Type, IDictionary>();
        private readonly Dictionary<IAttribute<string>, string> _str = new Dictionary<IAttribute<string>, string>();
        private readonly Dictionary<IAttribute<long>, long> _long = new Dictionary<IAttribute<long>, long>();
        private readonly Dictionary<IAttribute<bool>, bool> _bools = new Dictionary<IAttribute<bool>, bool>();
        private readonly Dictionary<IAttribute<DateTime>, DateTime> _dateTimes = new Dictionary<IAttribute<DateTime>, DateTime>();
        private readonly Dictionary<IAttribute<PhoenixValue>, PhoenixValue> _phxVals = new Dictionary<IAttribute<PhoenixValue>, PhoenixValue>();
        private readonly Dictionary<IAttribute<CollectionValue>, CollectionValue> _collections = new Dictionary<IAttribute<CollectionValue>, CollectionValue>();

   
        public AttributeBag()
        {
            _nonGenDict = new Dictionary<Type, IDictionary>
            {
                {typeof(string), _str},
                {typeof(long), _long},
                {typeof(bool), _bools},
                {typeof(DateTime), _dateTimes},
                {typeof(PhoenixValue), _phxVals},
                {typeof(CollectionValue), _collections},
            };
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
            return (T) _nonGenDict[typeof(T)][attribute];
        }

        public void Set<T>(IAttribute<T> attribute, T value)
        {
            _nonGenDict[typeof (T)][attribute] = value;
            
            _attributes[attribute] = value;
        }

        public AttributeValue[] GimmeAttributeValues()
        {

            return _str.ToAttributeValues()
                .Union(_long.ToAttributeValues())
                .Union(_bools.ToAttributeValues())
                .Union(_collections.ToAttributeValues())
                .Union(_dateTimes.ToAttributeValues())
                .Union(_phxVals.ToAttributeValues())
                .ToArray();
            //TODO _attributes.Select(attr => _funcs[attr.Value.GetType()].ToAttributeValue(attr.Value)).ToArray();
        }

    }

    public static class AssetExtensions
    {
        public static AttributeValue[] ToAttributeValues<T>(
            this IDictionary<IAttribute<T>, T> dictionary)
        {
            return dictionary.Select(kv => kv.Key.ToAttributeValue(kv.Value)).ToArray();
        }

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