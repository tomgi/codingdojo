using System;
using com.quark.qpp.common.dto;
using com.quark.qpp.core.attribute.service.dto;
using Attribute = com.quark.qpp.core.attribute.service.dto.Attribute;

namespace IHS.Phoenix.QPP.Facade.SoapFacade.QppAttributes
{
    public class NumAttr : BaseAttribute
    {
        public NumAttr(Attribute attribute)
            : base(attribute)
        {
        }

        public override AttributeValue ToAttributeValue()
        {
            if (Value != null && false == (Value is long))
                throw new ApplicationException("Attempt was made to initialize QPP Text Attribute with non string value");
            return ToAttributeValue<NumericValue>(attributeValue => attributeValue.value = (long) Value);
        }

        public override void InitFromAttributeValue(AttributeValue value)
        {
            if (value != null)
                Value = (value.attributeValue as NumericValue).value;
        }

        public override IAttribute New()
        {
            return new NumAttr(Attribute);
        }
    }
}