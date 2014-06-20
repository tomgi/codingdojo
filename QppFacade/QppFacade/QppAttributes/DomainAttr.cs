using System;
using System.Collections.Generic;
using System.Linq;
using com.quark.qpp.common.dto;
using com.quark.qpp.core.attribute.service.dto;
using Attribute = com.quark.qpp.core.attribute.service.dto.Attribute;

namespace IHS.Phoenix.QPP.Facade.SoapFacade.QppAttributes
{
    public class DomainAttr : BaseAttribute
    {
        private readonly Func<int, IEnumerable<DomainValue>> _getDomainValues;
        private IEnumerable<DomainValue> _domainValues;
        private readonly int _domainId;

        public DomainAttr(Attribute attribute, Func<int, IEnumerable<DomainValue>> getDomainValues) : base(attribute)
        {
            _domainId = (Attribute.defaultValuePreference as DomainValuePreferences).domainId;
            _getDomainValues = getDomainValues;
        }

        private IEnumerable<DomainValue> DomainValues
        {
            get { return _domainValues ?? (_domainValues = _getDomainValues(_domainId)); }
        }

        public override object FromAttributeValue(AttributeValue value)
        {
            return (value.attributeValue as DomainValue).name;
        }

        public override AttributeValue ToAttributeValue(object value)
        {
            DomainValue domainValue;
            if (value is string)
                domainValue = DomainValues.FirstOrDefault(dv => dv.name.Equals(value));
            else if (value is PhoenixValue)
                domainValue = DomainValues.FirstOrDefault(dv => dv.id.Equals(value as PhoenixValue));
            else
                domainValue = DomainValues.FirstOrDefault(dv => dv.id.Equals(value));

            if (domainValue == null)
                return null;
            var domainValueId = domainValue.id;
            var attribValue = ToAttributeValue<DomainValue>(
                attributeValue =>
                {
                    attributeValue.domainId = _domainId;
                    attributeValue.id = domainValueId;
                    attributeValue.name = domainValue.name;
                });
            return attribValue;
        }

    }
}