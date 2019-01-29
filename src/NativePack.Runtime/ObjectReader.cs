using System;
using System.Collections;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

namespace NativePack.Runtime
{
    internal class ObjectReader
    {
        public ObjectReader()
        {

        }

        public object Deserialize(DeserializerContext context, Type candidatedType = null)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            Type type = null;
            if (candidatedType != null)
            {
                if (context.Reader.ReadByte() == 0)
                    type = candidatedType;
            }

            if (type == null)
            {
                var typeFullName = context.Reader.ReadString();
                type = Type.GetType(typeFullName);

                if (type == null)
                {
                    throw new InvalidOperationException($"Unable to find type '{typeFullName}'");
                }
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
#if DEBUG
            if (property.SetMethod.IsPrivate)
                throw new InvalidOperationException($"Property {property.Name} has not a public set accessor");
#endif

            var propertyType = property.PropertyType;

            try
            {
                property.SetValue(value, DeserializeCore(context, propertyType));
            }
            catch (Exception e)
            {
                throw new ArgumentException($"Unable to set property '{property.Name}' of object", e);
            }
        }

        private object DeserializeCore(DeserializerContext context, Type typeOfValue)
        {
            var underlyingType = Nullable.GetUnderlyingType(typeOfValue);
            var enumType = typeOfValue.IsEnum ? typeOfValue : ((underlyingType != null && underlyingType.IsEnum) ? underlyingType : null);

            var isEnum = enumType != null;
            typeOfValue = underlyingType ?? typeOfValue;

            object valueOfPropertyDeserialized = null;

            if (isEnum)
                valueOfPropertyDeserialized = Enum.ToObject(typeOfValue, context.Reader.ReadInt32());
            else if (typeOfValue == typeof(char))
                valueOfPropertyDeserialized = context.Reader.ReadChar();
            else if (typeOfValue == typeof(byte))
                valueOfPropertyDeserialized = context.Reader.ReadByte();
            else if (typeOfValue == typeof(sbyte))
                valueOfPropertyDeserialized = context.Reader.ReadSByte();
            else if (typeOfValue == typeof(short))
                valueOfPropertyDeserialized = context.Reader.ReadInt16();
            else if (typeOfValue == typeof(ushort))
                valueOfPropertyDeserialized = context.Reader.ReadUInt16();
            else if (typeOfValue == typeof(int))
                valueOfPropertyDeserialized = context.Reader.ReadInt32();
            else if (typeOfValue == typeof(uint))
                valueOfPropertyDeserialized = context.Reader.ReadUInt32();
            else if (typeOfValue == typeof(long))
                valueOfPropertyDeserialized = context.Reader.ReadInt64();
            else if (typeOfValue == typeof(ulong))
                valueOfPropertyDeserialized = context.Reader.ReadUInt64();
            else if (typeOfValue == typeof(float))
                valueOfPropertyDeserialized = context.Reader.ReadSingle();
            else if (typeOfValue == typeof(double))
                valueOfPropertyDeserialized = context.Reader.ReadDouble();
            else if (typeOfValue == typeof(decimal))
                valueOfPropertyDeserialized = context.Reader.ReadDecimal();
            else if (typeOfValue == typeof(string))
                valueOfPropertyDeserialized = context.Reader.ReadString();
            else if (typeOfValue == typeof(DateTime))
                valueOfPropertyDeserialized = DateTime.FromBinary(context.Reader.ReadInt64());
            else if (typeOfValue == typeof(Guid))
                valueOfPropertyDeserialized = new Guid(context.Reader.ReadBytes(16));

            if (valueOfPropertyDeserialized != null)
            {
                return valueOfPropertyDeserialized;
            }

            var typeByte = context.Reader.ReadByte();
            if (typeByte == 1) //IList generic
            {
                var listTypeName = context.Reader.ReadString();
                var listType = Type.GetType(listTypeName);
                if (listType == null)
                {
                    throw new InvalidOperationException($"Unable to find type '{listTypeName}'");
                }
                var deserializedPropertyValue = Activator.CreateInstance(listType);
                var list = deserializedPropertyValue as IList;
                var itemCount = context.Reader.ReadInt32();

                Type currentItemType = null;
                for (int i = 0; i < itemCount; i++)
                {
                    var isNull = context.Reader.ReadByte() == 0;
                    if (isNull)
                        continue;

                    var sameTypeAsPrevious = context.Reader.ReadByte() == 0;

                    if (!sameTypeAsPrevious)
                    {
                        var itemTypeName = context.Reader.ReadString();
                        currentItemType = Type.GetType(itemTypeName);
                        if (currentItemType == null)
                        {
                            throw new InvalidOperationException($"Unable to find type '{itemTypeName}'");
                        }
                    }

                    if (currentItemType == null)
                    {
                        throw new InvalidOperationException();
                    }

                    var item = DeserializeCore(context, currentItemType);

                    list.Add(item);
                }

                return deserializedPropertyValue;
            }
            else if (typeByte == 0)
            {
                return Deserialize(context, typeOfValue);
            }

            throw new InvalidOperationException();
        }
    }
}
