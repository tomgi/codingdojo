using com.quark.qpp.core.attribute.service.constants;
using QppFacade;
using Attribute = com.quark.qpp.core.attribute.service.dto.Attribute;

namespace IHS.Phoenix.QPP.Facade.SoapFacade.QppAttributes
{
    public class CollectionAttr : BaseAttribute<CollectionValue>
    {
        public CollectionAttr(Attribute attribute)
            : base(attribute)
        {
        }

        public override int Type
        {
            get { return AttributeValueTypes.DOMAIN; }
        }
    }
}