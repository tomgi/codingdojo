using com.quark.qpp.core.attribute.service.constants;
using com.quark.qpp.core.attribute.service.dto;

namespace IHS.Phoenix.QPP.Facade.SoapFacade.QppAttributes
{
    public class DomainAttr : BaseAttribute<PhoenixValue>
    {
        public DomainAttr(Attribute attribute) : base(attribute)
        {
        }

        public override int Type
        {
            get { return AttributeValueTypes.DOMAIN; }
        }
    }
}