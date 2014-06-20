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

        public override AttributeValue ToAttributeValue()
        {
            var domainValue = _getCollectionValues(Value.ToString());
            var attribValue = ToAttributeValue<DomainValue>(
                attributeValue =>
                {
                    attributeValue.domainId = _collectionId;
                    attributeValue.id = domainValue;
                    attributeValue.name = (string) Value;
                });
            return attribValue;
        }

        public override void InitFromAttributeValue(AttributeValue value)
        {
            if (value != null)
                Value = (value.attributeValue as DomainValue).name;
        }

        public override IAttribute New()
        {
            return new CollectionAttr(Attribute, _getCollectionValues);
        }
    }
}