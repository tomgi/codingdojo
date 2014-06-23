using System;
using System.Globalization;
using com.quark.qpp.common.dto;
using com.quark.qpp.core.attribute.service.dto;
using Attribute = com.quark.qpp.core.attribute.service.dto.Attribute;

namespace IHS.Phoenix.QPP.Facade.SoapFacade.QppAttributes
{
    public class DateTimeAttr : BaseAttribute<DateTime>
    {
        public DateTimeAttr(Attribute attribute)
            : base(attribute)
        {
        }

        public override DateTime FromAttributeValue(AttributeValue value)
        {
            return DateTime.Now;
//                (DateTime?)
//                    DateTime.ParseExact((value.attributeValue as DateTimeValue).value, "yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'", CultureInfo.InvariantCulture);
        }

        public override AttributeValue ToAttributeValue(DateTime value)
        {
            return
                ToAttributeValue<DateTimeValue>(
                    attributeValue => attributeValue.value = value.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'", CultureInfo.InvariantCulture));
        }
    }
}