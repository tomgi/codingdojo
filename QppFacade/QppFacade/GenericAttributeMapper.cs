using System;
using System.Collections.Generic;
using System.Globalization;
using com.quark.qpp.common.dto;
using com.quark.qpp.core.attribute.service.constants;
using com.quark.qpp.core.attribute.service.dto;
using IHS.Phoenix.QPP;

namespace QppFacade
{
    internal static class GenericAttributeMapper
    {
        private static readonly IDictionary<int, Func<AttributeValue, object>> _map =
            new Dictionary<int, Func<AttributeValue, object>>
            {
                {AttributeValueTypes.BOOLEAN, MapBool},
                {AttributeValueTypes.NUMERIC, MapNumeric},
                {AttributeValueTypes.DATETIME, MapDateTime},
                {AttributeValueTypes.TEXT, MapText},
                {AttributeValueTypes.DOMAIN, MapDomain},
            };

        public static T Map<T>(AttributeValue attributeValue)
        {
            return (T) _map[attributeValue.type](attributeValue);
        }

        private static object MapDomain(AttributeValue attribute)
        {
            var domainVal = (attribute.attributeValue as DomainValue);
            return new PhoenixValue(domainVal.id, domainVal.name){ DomainId = domainVal.domainId};
        }

        private static object MapDateTime(AttributeValue attribute)
        {
            return DateTime.Parse((attribute.attributeValue as DateTimeValue).value);
        }

        private static object MapText(AttributeValue attribute)
        {
            return (attribute.attributeValue as TextValue).value;
        }

        private static object MapBool(AttributeValue attribute)
        {
            return (attribute.attributeValue as BooleanValue).value;
        }

        private static object MapNumeric(AttributeValue attribute)
        {
            return (attribute.attributeValue as NumericValue).value;
        }



        public static Value Map<T>(T value)
        {
            var methodInfo =
                typeof(GenericAttributeMapper)
                    .GetMethod("Map", new[] {typeof (T)});
            return (Value) methodInfo.Invoke(null, new object[]{value});
        }

        public static Value Map(long value)
        {
            return new NumericValue{ value = value };
        }

        public static Value Map(string value)
        {
            return new TextValue { value = value };
        }

        public static Value Map(bool value)
        {
            return new BooleanValue { value = value };
        }

        public static Value Map(DateTime value)
        {
            return new DateTimeValue { value = value.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'", CultureInfo.InvariantCulture) };
        }

        public static Value Map(PhoenixValue value)
        {
            return new DomainValue
            {
                domainId = value.DomainId,
                id = value,
                name = value
            };
        }
    }
}