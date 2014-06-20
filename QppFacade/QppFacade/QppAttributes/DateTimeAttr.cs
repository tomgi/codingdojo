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

        public override object FromAttributeValue(AttributeValue value)
        {
            return
                (DateTime?)
                    DateTime.ParseExact((value.attributeValue as DateTimeValue).value, "yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'", CultureInfo.InvariantCulture);
        }

        public override AttributeValue ToAttributeValue(object value)
        {
            if (value != null && false == (value is DateTime))
                throw new ApplicationException("Attempt was made to initialize QPP Text Attribute with non string value");
            return
                ToAttributeValue<DateTimeValue>(
                    attributeValue => attributeValue.value = ((DateTime) value).ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'", CultureInfo.InvariantCulture));
        }
    }
}