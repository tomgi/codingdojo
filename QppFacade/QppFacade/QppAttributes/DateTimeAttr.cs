using System;
using System.Globalization;
using com.quark.qpp.common.dto;
using com.quark.qpp.core.attribute.service.dto;
using Attribute = com.quark.qpp.core.attribute.service.dto.Attribute;

namespace IHS.Phoenix.QPP.Facade.SoapFacade.QppAttributes
{
    public class DateTimeAttr : BaseAttribute
    {
        public DateTimeAttr(Attribute attribute)
            : base(attribute)
        {
        }

        public override AttributeValue CreateValue(object primitiveValue)
        {
            var dateTimeValue = primitiveValue as DateTime?;

            return CreateValue<DateTimeValue>(attributeValue => attributeValue.value = dateTimeValue.HasValue ? dateTimeValue.Value.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'", CultureInfo.InvariantCulture) : string.Empty);
        }

        public override object GetValue(AttributeValue value)
        {
            var typedValue = GetValue<DateTimeValue>(value);
            return typedValue != null ? (DateTime?)DateTime.ParseExact(typedValue.value, "yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'", CultureInfo.InvariantCulture) : null;
        }
    }
}