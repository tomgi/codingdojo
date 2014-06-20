using System;
using com.quark.qpp.common.dto;
using com.quark.qpp.core.attribute.service.dto;
using Attribute = com.quark.qpp.core.attribute.service.dto.Attribute;

namespace IHS.Phoenix.QPP.Facade.SoapFacade.QppAttributes
{
    public class CollectionAttr : BaseAttribute
    {
        private readonly Func<string, long> _getCollectionValues;
        private readonly int _collectionId;

        public CollectionAttr(Attribute attribute, Func<string, long> getCollectionValues)
            : base(attribute)
        {
            _collectionId = (Attribute.defaultValuePreference as DomainValuePreferences).domainId;
            _getCollectionValues = getCollectionValues;
        }

        public override object FromAttributeValue(AttributeValue value)
        {
            return (value.attributeValue as DomainValue).name;
        }

        public override AttributeValue ToAttributeValue(object value)
        {
            var domainValue = _getCollectionValues(value.ToString());
            var attribValue = ToAttributeValue<DomainValue>(
                attributeValue =>
                {
                    attributeValue.domainId = _collectionId;
                    attributeValue.id = domainValue;
                    attributeValue.name = (string) value;
                });
            return attribValue;
        }

    }
}