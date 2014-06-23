using com.quark.qpp.common.dto;
using com.quark.qpp.core.attribute.service.dto;

namespace IHS.Phoenix.QPP.Facade.SoapFacade.QppAttributes
{
    public class DomainAttr : BaseAttribute<PhoenixValue>
    {
        private readonly int _domainId;

        public DomainAttr(Attribute attribute) : base(attribute)
        {
            _domainId = (Attribute.defaultValuePreference as DomainValuePreferences).domainId;
        }

        public override PhoenixValue FromAttributeValue(AttributeValue value)
        {
            return null;
            //(value.attributeValue as DomainValue).name;
        }

        public override AttributeValue ToAttributeValue(PhoenixValue value)
        {
            var attribValue = ToAttributeValue<DomainValue>(
                attributeValue =>
                {
                    attributeValue.domainId = _domainId;
                    attributeValue.id = value;
                    attributeValue.name = value;
                });
            return attribValue;
        }
    }
}