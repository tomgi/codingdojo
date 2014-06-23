using System;
using com.quark.qpp.common.dto;
using com.quark.qpp.core.attribute.service.constants;
using com.quark.qpp.core.attribute.service.dto;
using Attribute = com.quark.qpp.core.attribute.service.dto.Attribute;

namespace IHS.Phoenix.QPP.Facade.SoapFacade.QppAttributes
{
    public class CollectionAttr : BaseAttribute<string>
    {
        private readonly Func<string, long> _getCollectionValues;

        public CollectionAttr(Attribute attribute, Func<string, long> getCollectionValues)
            : base(attribute)
        {
            _getCollectionValues = getCollectionValues;
        }

        public override string FromAttributeValue(AttributeValue value)
        {
            return (value.attributeValue as DomainValue).name;
        }

        public override AttributeValue ToAttributeValue(string value)
        {
            var collectionId = _getCollectionValues(value);
            var attribValue = ToAttributeValue<DomainValue>(
                attributeValue =>
                {
                    attributeValue.domainId = DefaultDomains.COLLECTIONS;
                    attributeValue.id = collectionId;
                    attributeValue.name = value;
                });
            return attribValue;
        }

    }
}