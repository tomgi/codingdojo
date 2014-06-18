using System;
using com.quark.qpp.common.dto;
using com.quark.qpp.core.attribute.service.dto;
using Attribute = com.quark.qpp.core.attribute.service.dto.Attribute;

namespace IHS.Phoenix.QPP.Facade.SoapFacade.QppAttributes
{
    public interface IHaveNameAndId
    {
        long Id { get; }
        string Name { get; }
        object Value { get; }
        IHaveNameAndId WithValue(object value);
        IHaveNameAndId FromAttributeValue(AttributeValue value);
        AttributeValue ToAttributeValue();
        bool CanBeUpdated();
    }
    public abstract class BaseAttribute : IHaveNameAndId
    {
        public override int GetHashCode()
        {
            return (Attribute != null ? Attribute.GetHashCode() : 0);
        }

        protected readonly Attribute Attribute;
        public string Name {
            get
            {
                return Attribute.name;
            }
        }
        public long Id
        {
            get
            {
                return Attribute.id;
            }
        }

        public bool Equals(IHaveNameAndId other)
        {
            return Id == other.Id || string.Equals(Name, other.Name);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            return Equals((IHaveNameAndId)obj);
        }

        public object Value { get; protected set; }

        protected BaseAttribute(Attribute attribute)
        {
            Attribute = attribute;
        }

        public abstract AttributeValue ToAttributeValue();
        
        public static implicit operator AttributeValue(BaseAttribute attribute)
        {
            return attribute.ToAttributeValue();
        }

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
        public abstract IHaveNameAndId FromAttributeValue(AttributeValue value);
        public abstract IHaveNameAndId WithValue(object value);
    }
}