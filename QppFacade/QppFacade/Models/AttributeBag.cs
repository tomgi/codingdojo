using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using com.quark.qpp.common.dto;
using com.quark.qpp.core.asset.service.dto;
using com.quark.qpp.core.attribute.service.constants;
using com.quark.qpp.core.attribute.service.dto;
using IHS.Phoenix.QPP;
using IHS.Phoenix.QPP.Facade.SoapFacade.QppAttributes;

namespace QppFacade
{
    public abstract class AttributeBag
    {
        private readonly List<AttributeValue> _attributeValues = new List<AttributeValue>();

        public static Dictionary<long,bool> ModifiableAttributes { get; private set; }

        static AttributeBag ()
        {
            ModifiableAttributes = new Dictionary<long, bool>();
        }

        public AttributeValue[] GimmeAttributeValues()
        {
            return _attributeValues.ToArray();
        }

        public AttributeValue[] GimmeModifiableAttributeValues()
        {
            return _attributeValues
                .Where(a => ModifiableAttributes[a.attributeId])
                .ToArray();
        }

        public T Get<T>(IAttribute<T> attribute)
        {
            var attr = _attributeValues.SingleOrDefault(a => a.attributeId == attribute.Id);
            if (attr != null)
            {
                return GenericValueExtractor.Extract<T>(attr);
            }
            return default(T);
        }

        public void Set<T>(IAttribute<T> attribute, T value)
        {
            var existingAttr = _attributeValues.SingleOrDefault(a => a.attributeId == attribute.Id);
            if (existingAttr == null)
            {
                AddNewAttribute(attribute, value);
            }
            else
            {
                UpdateExistingAttribute(existingAttr, value);
            }
        }

        public void Set(AttributeValue attributeValue)
        {
            var existingAttr = _attributeValues.SingleOrDefault(a => a.attributeId == attributeValue.attributeId);
            if (existingAttr == null)
            {
                _attributeValues.Add(attributeValue);
            }
            else
            {
                existingAttr.attributeValue = attributeValue.attributeValue;
            }
        }

        private void AddNewAttribute<T>(IAttribute<T> attribute, T value)
        {
            var newAttribute = new AttributeValue
            {
                attributeId = attribute.Id,
                attributeValue = AttributeValueFactory.Create(value),
                type = attribute.Type
            };
            _attributeValues.Add(newAttribute);
        }

        private void UpdateExistingAttribute<T>(AttributeValue existingAttr, T value)
        {
            existingAttr.attributeValue = AttributeValueFactory.Create(value);
        }
    }

    internal static class GenericValueExtractor
    {
        private static IDictionary<int, Func<AttributeValue, object>> _map =
            new Dictionary<int, Func<AttributeValue, object>>
            {
                {AttributeValueTypes.BOOLEAN, GetBool},
                {AttributeValueTypes.NUMERIC, GetNum},
                {AttributeValueTypes.DATETIME, GetDateTime},
                {AttributeValueTypes.TEXT, GetText},
                {AttributeValueTypes.DOMAIN, GetDomain},
            };

        private static object GetDomain(AttributeValue attribute)
        {
            var domainVal = (attribute.attributeValue as DomainValue);
            return new PhoenixValue(domainVal.id, domainVal.name){ DomainId = domainVal.domainId};
        }

        private static object GetDateTime(AttributeValue attribute)
        {
            return DateTime.Parse((attribute.attributeValue as DateTimeValue).value);
        }

        private static object GetText(AttributeValue attribute)
        {
            return (attribute.attributeValue as TextValue).value;
        }

        private static object GetBool(AttributeValue attribute)
        {
            return (attribute.attributeValue as BooleanValue).value;
        }

        private static object GetNum(AttributeValue attribute)
        {
            return (attribute.attributeValue as NumericValue).value;
        }

        public static T Extract<T>(AttributeValue attributeValue)
        {
            return (T) _map[attributeValue.type](attributeValue);
        }
    }

    internal static class AttributeValueFactory
    {
        public static Value Create<T>(T value)
        {
            var methodInfo = 
                typeof (AttributeValueFactory)
                .GetMethod("Create", new[] {typeof (T)});
            return (Value) methodInfo.Invoke(null, new object[]{value});
        }

        public static Value Create(long value)
        {
            return new NumericValue{ value = value };
        }

        public static Value Create(string value)
        {
            return new TextValue { value = value };
        }
        
        public static Value Create(bool value)
        {
            return new BooleanValue { value = value };
        }

        public static Value Create(DateTime value)
        {
            return new DateTimeValue { value = value.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'", CultureInfo.InvariantCulture) };
        }

        public static Value Create(PhoenixValue value)
        {
            return new DomainValue
            {
                domainId = value.DomainId,
                id = value,
                name = value
            };
        }
    }

    public static class AssetModelExtensions
    {
        public static TAsset With<TAsset,TValue>(this TAsset asset, IAttribute<TValue> attribute, TValue value)
            where TAsset : AttributeBag
        {
            asset.Set(attribute, value);
            return asset;
        }
    }


    public static class AssetExtensions
    {
        public static Asset ToAsset(this FileAsset assetModel)
        {
            return new Asset
            {
                assetId = assetModel.Id,
                attributeValues = assetModel.GimmeAttributeValues()
            };
        }
    }
}