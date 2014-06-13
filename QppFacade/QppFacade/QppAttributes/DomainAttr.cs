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

        public override AttributeValue CreateValue(object value)
        {
            var domainValue = DomainValues.FirstOrDefault(dv => dv.name.Equals(value));
            if (domainValue == null)
                return null;
            var domainValueId = domainValue.id;
            var attribValue = CreateValue<DomainValue>(
                attributeValue =>
                {
                    attributeValue.domainId = _domainId;
                    attributeValue.id = domainValueId;
                    attributeValue.name = (string) value;
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