using System;
using com.quark.qpp.common.dto;
using com.quark.qpp.core.attribute.service.constants;
using com.quark.qpp.core.attribute.service.dto;
using Attribute = com.quark.qpp.core.attribute.service.dto.Attribute;

namespace IHS.Phoenix.QPP.Facade.SoapFacade.QppAttributes
{
    public class TextAttr : BaseAttribute<string>
    {
        public TextAttr(Attribute attribute) : base(attribute)
        {
        }

        public override string FromAttributeValue(AttributeValue value)
        {
            return (value.attributeValue as TextValue).value;
        }

        public override AttributeValue ToAttributeValue(string value)
        {
            return ToAttributeValue<TextValue>(attributeValue => attributeValue.value = (string) value);
        }

        public override int Type
        {
            get { return AttributeValueTypes.TEXT; }
        }
    }
}