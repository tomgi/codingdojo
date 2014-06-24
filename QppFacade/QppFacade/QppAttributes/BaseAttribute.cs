using System;
using com.quark.qpp.common.dto;
using com.quark.qpp.core.attribute.service.dto;
using Attribute = com.quark.qpp.core.attribute.service.dto.Attribute;

namespace IHS.Phoenix.QPP.Facade.SoapFacade.QppAttributes
{
    public interface IAttribute
    {
        long Id { get; }
        string Name { get; }
    }

    public interface IAttribute<OurAttributeValueType> : IAttribute
    {
        OurAttributeValueType FromAttributeValue(AttributeValue value);
        AttributeValue ToAttributeValue(OurAttributeValueType value);
        bool CanBeUpdated();
        int Type { get; }
    }

    public abstract class BaseAttribute<OurAttributeValueType> : IAttribute<OurAttributeValueType>
    {
        public override int GetHashCode()
        {
            return (Attribute != null ? Attribute.GetHashCode() : 0);
        }

        protected readonly Attribute Attribute;

        public string Name
        {
            get { return Attribute.name; }
        }

        public long Id
        {
            get { return Attribute.id; }
        }

        public abstract int Type { get;}

        public bool Equals(IAttribute<OurAttributeValueType> other)
        {
            return Id == other.Id || string.Equals(Name, other.Name);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            return Equals((IAttribute<OurAttributeValueType>) obj);
        }

        protected BaseAttribute(Attribute attribute)
        {
            Attribute = attribute;
        }

        public abstract OurAttributeValueType FromAttributeValue(AttributeValue value);
        public abstract AttributeValue ToAttributeValue(OurAttributeValueType value);

        protected AttributeValue ToAttributeValue<TValue>(Action<TValue> setValue)
            where TValue : Value
        {
            var attributeValue = new AttributeValue
            {
                attributeId = Attribute.id,
                type = Attribute.valueType,
                attributeValue = Activator.CreateInstance<TValue>()
            };
            var value = attributeValue.attributeValue as TValue;
            if (value == null)
                throw new ApplicationException(string.Format("Failed to set value to an attribute of type id {0}", Attribute.id));
            setValue(value);
            return attributeValue;
        }

        public bool CanBeUpdated()
        {
            return Attribute.constraintsChangeable;
        }
    }
}