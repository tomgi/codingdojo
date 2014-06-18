using System;
using System.Runtime.Remoting.Messaging;
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

        public override AttributeValue ToAttributeValue()
        {
            if (Value!=null && false == (Value is string))
                throw new ApplicationException("Attempt was made to initialize QPP Text Attribute with non string value");
            return ToAttributeValue<TextValue>(attributeValue => attributeValue.value = (string) Value);
        }
        public override IHaveNameAndId FromAttributeValue(AttributeValue value)
        {
            return value==null ? null : WithValue((value.attributeValue as TextValue).value);
        }

        public override IHaveNameAndId WithValue(object value)
        {
            return new TextAttr(Attribute) {Value = value};
        }
    }
}