using com.quark.qpp.common.dto;
using com.quark.qpp.core.attribute.service.dto;

namespace IHS.Phoenix.QPP.Facade.SoapFacade.QppAttributes
{
    public class NumAttr : BaseAttribute
    {
        public NumAttr(Attribute attribute)
            : base(attribute)
        {
        }

        public override AttributeValue CreateValue(object primitiveValue)
        {
            return CreateValue<NumericValue>(attributeValue => attributeValue.value = (long) primitiveValue);
        }

        public override object GetValue(AttributeValue value)
        {
            var typedValue = GetValue<NumericValue>(value);
            return typedValue != null ? typedValue.value : 0;
        }
    }
}