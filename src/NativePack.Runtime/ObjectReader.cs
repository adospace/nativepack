using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

namespace NativePack.Runtime
{
    public class ObjectReader
    {
        public ObjectReader()
        {

        }

        public object Deserialize(DeserializerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var typeFullName = context.Reader.ReadString();
            var type = Type.GetType(typeFullName);

            if (type == null)
            {
                throw new InvalidOperationException($"Unable to find type '{typeFullName}'");
            }

            var properties = type.GetProperties();
            var value = FormatterServices.GetUninitializedObject(type);

            foreach (var property in properties)
            {
                Deserialize(context, value, property);
            }

            return value;
        }

        private void Deserialize(DeserializerContext context, object value, PropertyInfo property)
        {
            if (context.Reader.ReadByte() == 0)
            {
                property.SetValue(value, null);
                return;
            }

            try
            {
#if DEBUG
                if (property.SetMethod.IsPrivate)
                    throw new InvalidOperationException($"Property {property.Name} has not a public set accessor");
#endif
                var underlyingType = Nullable.GetUnderlyingType(property.PropertyType);
                var enumType = property.PropertyType.IsEnum ? property.PropertyType : ((underlyingType != null && underlyingType.IsEnum) ? underlyingType : null);

                //var isNullable = underlyingType != null;
                var isEnum = enumType != null;
                var propertyType = underlyingType ?? property.PropertyType;

                object valueOfPropertyDeserialized = null;
                //bool isNullableNull = false;

                //if (isNullable)
                //{
                //    isNullableNull = context.Reader.ReadByte() == 0;
                //}

                //if (!isNullable || !isNullableNull)
                //{
                    if (propertyType == typeof(char))
                        valueOfPropertyDeserialized = context.Reader.ReadChar();
                    else if (propertyType == typeof(byte))
                        valueOfPropertyDeserialized = context.Reader.ReadByte();
                    else if (propertyType == typeof(sbyte))
                        valueOfPropertyDeserialized = context.Reader.ReadSByte();
                    else if (propertyType == typeof(short))
                        valueOfPropertyDeserialized = context.Reader.ReadInt16();
                    else if (propertyType == typeof(ushort))
                        valueOfPropertyDeserialized = context.Reader.ReadUInt16();
                    else if (propertyType == typeof(int))
                        valueOfPropertyDeserialized = context.Reader.ReadInt32();
                    else if (propertyType == typeof(uint))
                        valueOfPropertyDeserialized = context.Reader.ReadUInt32();
                    else if (propertyType == typeof(long))
                        valueOfPropertyDeserialized = context.Reader.ReadInt64();
                    else if (propertyType == typeof(ulong))
                        valueOfPropertyDeserialized = context.Reader.ReadUInt64();
                    else if (propertyType == typeof(float))
                        valueOfPropertyDeserialized = context.Reader.ReadSingle();
                    else if (propertyType == typeof(double))
                        valueOfPropertyDeserialized = context.Reader.ReadDouble();
                    else if (propertyType == typeof(decimal))
                        valueOfPropertyDeserialized = context.Reader.ReadDecimal();
                    else if (propertyType == typeof(string))
                        valueOfPropertyDeserialized = context.Reader.ReadString();
                    else if (propertyType == typeof(DateTime))
                        valueOfPropertyDeserialized = DateTime.FromBinary(context.Reader.ReadInt64());
                    else if (propertyType == typeof(Guid))
                        valueOfPropertyDeserialized = new Guid(context.Reader.ReadBytes(16));
                //}

                //if (isNullable && isNullableNull)
                //{
                //    //this should not be required given is
                //    property.SetValue(value, null);
                //    return;
                //}

                if (valueOfPropertyDeserialized != null)
                {
                    property.SetValue(value, valueOfPropertyDeserialized);
                    return;
                }
            }
            catch (Exception e)
            {
                throw new ArgumentException($"Unable to set property '{property.Name}' of object", e);
            }


            var complexObjectDeserialized = Deserialize(context);
            property.SetValue(value, complexObjectDeserialized);
        }
    }
}
