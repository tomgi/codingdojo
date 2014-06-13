using System;
using System.Collections.Generic;
using com.quark.qpp.common.dto;
using com.quark.qpp.core.attribute.service.constants;
using Attribute = com.quark.qpp.core.attribute.service.dto.Attribute;

namespace IHS.Phoenix.QPP.Facade.SoapFacade.QppAttributes
{
    public class QppAttributes
    {
        private IDictionary<string, BaseAttribute> _attributesByName;
        private IDictionary<long, BaseAttribute> _attributesById;
        private readonly Func<IEnumerable<Attribute>> _getQppAttributes;
        private readonly Func<int, IEnumerable<DomainValue>> _getDomainValues;
        private readonly Func<string, long> _getCollectionValues; 


        public QppAttributes(Func<IEnumerable<Attribute>> getQppAttributes, Func<int, IEnumerable<DomainValue>> getDomainValues, Func<string, long> getCollectionValues)
        {
            _getQppAttributes = getQppAttributes;
            _getDomainValues = getDomainValues;
            _getCollectionValues = getCollectionValues;
        }

        private IDictionary<string, BaseAttribute> AttributesByName
        {
            get
            {
                if (_attributesByName == null)
                    Init();
                return _attributesByName;
            }
        }

        private IDictionary<long, BaseAttribute> AttributesById
        {
            get
            {
                if (_attributesById == null)
                    Init();
                return _attributesById;
            }
        }

        private void Init()
        {
            _attributesByName = new Dictionary<string, BaseAttribute>();
            _attributesById = new Dictionary<long, BaseAttribute>();
            foreach (var qppAttribute in _getQppAttributes())
            {
                BaseAttribute attribute = null;
                if (qppAttribute.valueType == AttributeValueTypes.TEXT)
                {
                        attribute = new TextAttr(qppAttribute);
                }
                else if (qppAttribute.valueType == AttributeValueTypes.NUMERIC)
                {
                        attribute = new NumAttr(qppAttribute);
                }
                else if (qppAttribute.valueType == AttributeValueTypes.BOOLEAN)
                {
                    attribute = new BoolAttr(qppAttribute);
                }
                else if (qppAttribute.valueType == AttributeValueTypes.DATETIME)
                {
                    attribute = new DateTimeAttr(qppAttribute);
                }
                else if (qppAttribute.valueType == AttributeValueTypes.DOMAIN)
                {
                    if(qppAttribute.id == DefaultAttributes.COLLECTION)
                        attribute = new CollectionAttr(qppAttribute, _getCollectionValues);
                    else
                        attribute = new DomainAttr(qppAttribute, _getDomainValues);
                }
                if (attribute != null)
                {
                    AttributesByName[qppAttribute.name] = attribute;
                    AttributesById[qppAttribute.id] = attribute;
                }
            }
        }

        private bool IsTaxonomyAttribute(Attribute qppAttribute)
        {
            return qppAttribute.name.ToLower().Contains("(taxonomy)");
        }

        public BaseAttribute FindByName(string attributeName)
        {
            return AttributesByName.ContainsKey(attributeName) ? AttributesByName[attributeName] : null;
        }
        public BaseAttribute FindById(long attributeId)
        {
            return AttributesById.ContainsKey(attributeId) ? AttributesById[attributeId] : null;
        }

        public BaseAttribute Find(IAttributeIdentifier attributeIdentifier)
        {
            if (attributeIdentifier.Id != 0)
                return FindById(attributeIdentifier.Id);
            
            if (false == string.IsNullOrEmpty(attributeIdentifier.Name))
                return FindByName(attributeIdentifier.Name);
            
            return null;
        }
    }
}
