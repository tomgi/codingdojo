using System;
using System.Collections.Generic;
using System.Linq;
using com.quark.qpp.common.dto;
using com.quark.qpp.core.attribute.service.dto;
using Attribute = System.Attribute;

namespace IHS.Phoenix.QPP.Facade.SoapFacade.QppAttributes
{
    public class CollectionAttr : BaseAttribute
    {
        private readonly Func<string, long> _getCollectionValues;
        private readonly int _collectionId;

        public CollectionAttr(com.quark.qpp.core.attribute.service.dto.Attribute attribute, Func<string, long> getCollectionValues)
            : base(attribute)
        {
            _collectionId = (Attribute.defaultValuePreference as DomainValuePreferences).domainId;
            _getCollectionValues = getCollectionValues;
        }

        public override AttributeValue ToAttributeValue()
        {
            var domainValue = _getCollectionValues(Value.ToString());
            var attribValue = ToAttributeValue<DomainValue>(
                attributeValue =>
                {
                    attributeValue.domainId = _collectionId;
                    attributeValue.id = domainValue;
                    attributeValue.name = (string)Value;
                });
            return attribValue;
        }
        public override IHaveNameAndId FromAttributeValue(AttributeValue value)
        {
            return value == null ? null : WithValue((value.attributeValue as DomainValue).name);
        }

        public override IHaveNameAndId WithValue(object value)
        {
            return new CollectionAttr(Attribute, _getCollectionValues)
            {
                Value = value
            };
        }
    }
}