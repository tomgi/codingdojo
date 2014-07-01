using System;
using com.quark.qpp.core.attribute.service.constants;
using Attribute = com.quark.qpp.core.attribute.service.dto.Attribute;

namespace IHS.Phoenix.QPP.Facade.SoapFacade.QppAttributes
{
    public class DateTimeAttr : BaseAttribute<DateTime>
    {
        public DateTimeAttr(Attribute attribute)
            : base(attribute)
        {
        }

        public override int Type
        {
            get { return AttributeValueTypes.DATETIME; }
        }
    }
}