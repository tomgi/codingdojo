using System;
using com.quark.qpp.common.dto;
using com.quark.qpp.core.attribute.service.dto;
using Attribute = com.quark.qpp.core.attribute.service.dto.Attribute;

namespace IHS.Phoenix.QPP.Facade.SoapFacade.QppAttributes
{
    public class TextAttr : BaseAttribute
    {
        public TextAttr(Attribute attribute) : base(attribute)
        {
        }

        public override AttributeValue CreateValue(object primitiveValue)
        {
            if (primitiveValue!=null && false == (primitiveValue is string))
                throw new ApplicationException("Attempt was made to initialize QPP Text Attribute with non string value");
            return CreateValue<TextValue>(attributeValue => attributeValue.value = (string) primitiveValue);
        }

        public override object GetValue(AttributeValue value)
        {
            var typedValue = GetValue<TextValue>(value);
            return typedValue != null ? typedValue.value : null;
        }
    }
}