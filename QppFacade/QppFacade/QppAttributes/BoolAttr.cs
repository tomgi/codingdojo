using com.quark.qpp.common.dto;
using com.quark.qpp.core.attribute.service.dto;

namespace IHS.Phoenix.QPP.Facade.SoapFacade.QppAttributes
{
    public class BoolAttr : BaseAttribute
    {
        public BoolAttr(Attribute attribute)
            : base(attribute)
        {
        }

        public override AttributeValue CreateValue(object primitiveValue)
        {
            return CreateValue<BooleanValue>(attributeValue => attributeValue.value = (bool) primitiveValue);
        }

        public override object GetValue(AttributeValue value)
        {
            var typedValue = GetValue<BooleanValue>(value);
            return typedValue != null && typedValue.value;
        }
    }
}