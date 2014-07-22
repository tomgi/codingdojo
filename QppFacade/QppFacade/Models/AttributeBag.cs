using System.Collections;
using System.Collections.Generic;
using System.Linq;
using com.quark.qpp.core.attribute.service.dto;

namespace QppFacade
{
    public sealed class AttributeBag : IReadOnlyCollection<AttributeValue>
    {
        private readonly List<AttributeValue> _attributeValues;

        public AttributeBag() : this(Enumerable.Empty<AttributeValue>())  { }

        public AttributeBag(IEnumerable<AttributeValue> attributeValues)
        {
            _attributeValues = attributeValues.ToList();
        }

        public IEnumerator<AttributeValue> GetEnumerator()
        {
            return _attributeValues.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count { get { return _attributeValues.Count; } }

        public T Get<T>(PhoenixAttribute<T> attribute)
        {
            var attr = _attributeValues.SingleOrDefault(a => a.attributeId == attribute.Id);
            if (attr != null)
            {
                return GenericAttributeMapper.Map<T>(attr);
            }
            return default(T);
        }

        public void Set<T>(PhoenixAttribute<T> attribute, T value)
        {
            var existingAttr = _attributeValues.SingleOrDefault(a => a.attributeId == attribute.Id);
            if (existingAttr == null)
            {
                _attributeValues.Add(new AttributeValue
                {
                    attributeId = attribute.Id,
                    attributeValue = GenericAttributeMapper.Map(value),
                    type = attribute.Type
                });
            }
            else
            {
                existingAttr.attributeValue = GenericAttributeMapper.Map(value);
            }
        }
    }
}