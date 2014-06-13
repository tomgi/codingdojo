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

        public override AttributeValue CreateValue(object value)
        {
            var domainValue = _getCollectionValues(value.ToString());
            var attribValue = CreateValue<DomainValue>(
                attributeValue =>
                {
                    attributeValue.domainId = _collectionId;
                    attributeValue.id = domainValue;
                    attributeValue.name = (string)value;
                });
            return attribValue;
        }

        public override object GetValue(AttributeValue value)
        {
            var typedValue = GetValue<DomainValue>(value);
            return typedValue != null ? typedValue.name : string.Empty;
        }
    }
}