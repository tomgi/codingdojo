using System;
using com.quark.qpp.common.dto;
using com.quark.qpp.core.attribute.service.constants;
using com.quark.qpp.core.attribute.service.dto;
using Attribute = com.quark.qpp.core.attribute.service.dto.Attribute;

namespace IHS.Phoenix.QPP.Facade.SoapFacade.QppAttributes
{
    public class NumAttr : BaseAttribute<long>
    {
        public NumAttr(Attribute attribute)
            : base(attribute)
        {
        }

        public override long FromAttributeValue(AttributeValue value)
        {
            return (value.attributeValue as NumericValue).value;
        }

        public override AttributeValue ToAttributeValue(long value)
        {
            if (value != null && false == (value is long))
                throw new ApplicationException("Attempt was made to initialize QPP Text Attribute with non string value");
            return ToAttributeValue<NumericValue>(attributeValue => attributeValue.value = (long) value);
        }

        public override int Type
        {
            get { return AttributeValueTypes.NUMERIC; }
        }
    }
}