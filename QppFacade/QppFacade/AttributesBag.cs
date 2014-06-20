using System;
using System.Collections;
using System.Collections.Generic;
using IHS.Phoenix.QPP.Facade.SoapFacade.QppAttributes;

namespace QppFacade
{
    public class AttributesBag : IEnumerable<BaseAttribute>
    {
        public IEnumerator<BaseAttribute> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}