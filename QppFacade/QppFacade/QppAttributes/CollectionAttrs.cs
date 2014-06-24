using System;
using com.quark.qpp.common.dto;
using com.quark.qpp.core.attribute.service.constants;
using com.quark.qpp.core.attribute.service.dto;
using QppFacade;
using Attribute = com.quark.qpp.core.attribute.service.dto.Attribute;

namespace IHS.Phoenix.QPP.Facade.SoapFacade.QppAttributes
{
    public class CollectionAttr : BaseAttribute<CollectionValue>
    {
        private readonly Func<string, long> _getCollectionValues;

        public CollectionAttr(Attribute attribute, Func<string, long> getCollectionValues)
            : base(attribute)
        {
            _getCollectionValues = getCollectionValues;
        }

        public override CollectionValue FromAttributeValue(AttributeValue value)
        {
            return new CollectionValue((value.attributeValue as DomainValue).name);
        }

        public override AttributeValue ToAttributeValue(CollectionValue value)
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

        public override int Type
        {
            get { return AttributeValueTypes.DOMAIN; }
        }
    }
}