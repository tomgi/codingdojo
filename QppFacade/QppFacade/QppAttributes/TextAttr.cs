using System;
using com.quark.qpp.core.attribute.service.constants;
using Attribute = com.quark.qpp.core.attribute.service.dto.Attribute;

namespace IHS.Phoenix.QPP.Facade.SoapFacade.QppAttributes
{
    public class TextAttr : BaseAttribute<string>
    {
        public TextAttr(Attribute attribute) : base(attribute)
        {
        }

        public override int Type
        {
            get { return AttributeValueTypes.TEXT; }
        }
    }
}