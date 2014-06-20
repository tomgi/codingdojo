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
        public override AttributeValue ToAttributeValue()
        {
            if (Value != null && false == (Value is DateTime))
                throw new ApplicationException("Attempt was made to initialize QPP Text Attribute with non string value");
            return ToAttributeValue<DateTimeValue>(attributeValue => attributeValue.value = ((DateTime)Value).ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'", CultureInfo.InvariantCulture));

        }
        public override void InitFromAttributeValue(AttributeValue value)
        {
            if(value != null) 
                Value=(DateTime?)DateTime.ParseExact((value.attributeValue as DateTimeValue).value, "yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'", CultureInfo.InvariantCulture);
        }

        public override IHaveNameAndId New()
        {
            return new DateTimeAttr(Attribute);
        }
    }
}