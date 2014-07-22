using System.Collections.Generic;
using System.Linq;
using com.quark.qpp.core.asset.service.dto;
using com.quark.qpp.core.attribute.service.dto;
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
                return GenericAttributeMapper.Map<T>(attr);
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
                attributeValue = GenericAttributeMapper.Map(value),
                type = attribute.Type
            };
            _attributeValues.Add(newAttribute);
        }

        private void UpdateExistingAttribute<T>(AttributeValue existingAttr, T value)
        {
            existingAttr.attributeValue = GenericAttributeMapper.Map(value);
        }
    }

    public static class AttrbiuteBagExtensions
    {
        public static TAttributeBag With<TAttributeBag,TValue>(this TAttributeBag asset, IAttribute<TValue> attribute, TValue value)
            where TAttributeBag : AttributeBag
        {
            asset.Set(attribute, value);
            return asset;
        }

        public static TAttributeBag With<TAttributeBag>(this TAttributeBag asset, AttributeValue[] attributes)
            where TAttributeBag : AttributeBag
        {
            foreach (var attribute in attributes)
            {
                asset.Set(attribute);
            }
            return asset;
        }
    }
}