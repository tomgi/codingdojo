using com.quark.qpp.common.dto;
using com.quark.qpp.core.attribute.service.dto;
using ApplicationException = System.ApplicationException;

namespace IHS.Phoenix.QPP.Facade.SoapFacade.QppAttributes
{
    public class BoolAttr : BaseAttribute
    {
        public BoolAttr(Attribute attribute)
            : base(attribute)
        {
        }

        public override AttributeValue ToAttributeValue()
        {
            if (Value != null && false == (Value is bool))
                throw new ApplicationException("Attempt was made to initialize QPP Text Attribute with non string value");
            return ToAttributeValue<BooleanValue>(attributeValue => attributeValue.value = (bool)Value);

        }
        public override IHaveNameAndId FromAttributeValue(AttributeValue value)
        {
            return value == null ? null : WithValue((value.attributeValue as BooleanValue).value);
        }

        public override IHaveNameAndId WithValue(object value)
        {
            return new BoolAttr(Attribute)
            {
                Value = value
            };
        }
    }
}