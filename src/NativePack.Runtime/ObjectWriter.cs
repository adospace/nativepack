using System;
using System.Collections;
using System.IO;
using System.Reflection;

namespace NativePack.Runtime
{
    internal class ObjectWriter
    {
        public ObjectWriter()
        {
        }

        public void Serialize(SerializerContext context, object value, Type candidateType = null)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var typeValue = value.GetType();
            if (typeValue.GetCustomAttribute(typeof(SerializableAttribute)) == null)
                throw new InvalidOperationException($"Type '{typeValue.FullName}' is not marked with the Serializable attribute");

            if (candidateType != null)
            {
                context.Writer.Write(typeValue != candidateType ? (byte)1 : (byte)0);
            }

            if (typeValue != candidateType)
            {
                context.Writer.Write(typeValue.AssemblyQualifiedName);
            }

            if (typeValue.GetCustomAttribute(typeof(SerializableAttribute)) == null)
                throw new InvalidOperationException();

            var properties = typeValue.GetProperties();
            
            foreach (var property in properties)
            {
                Serialize(context, value, property);
            }
        }

        private void Serialize(SerializerContext context, object value, PropertyInfo propertyInfo)
        {
            var propertyValue = propertyInfo.GetValue(value);
            context.Writer.Write(propertyValue == null ? (byte)0 : (byte)1);

            if (propertyValue == null)
                return;

            SerializeCore(context, propertyValue, propertyValue.GetType());
        }

        private void SerializeCore(SerializerContext context, object value, Type typeOfValue)
        {
            switch (value)
            {
                #region Primitive Types
                case byte v:
                    context.Writer.Write(v);
                    return;
                case sbyte v:
                    context.Writer.Write(v);
                    return;
                case char v:
                    context.Writer.Write(v);
                    return;
                case short v:
                    context.Writer.Write(v);
                    return;
                case ushort v:
                    context.Writer.Write(v);
                    return;
                case int v:
                    context.Writer.Write(v);
                    return;
                case uint v:
                    context.Writer.Write(v);
                    return;
                case long v:
                    context.Writer.Write(v);
                    return;
                case ulong v:
                    context.Writer.Write(v);
                    return;
                case float v:
                    context.Writer.Write(v);
                    return;
                case double v:
                    context.Writer.Write(v);
                    return;
                case decimal v:
                    context.Writer.Write(v);
                    return;
                case DateTime v:
                    context.Writer.Write(v.ToBinary());
                    return;
                case Guid v:
                    context.Writer.Write(v.ToByteArray());
                    return;
                case string v:
                    context.Writer.Write(v);
                    return;
                    #endregion
            }

            if (typeOfValue.IsEnum)
            {
                context.Writer.Write((int)value);
                return;
            }

            #region IList
            if (value is IList)
            {
                var list = value as IList;
                context.Writer.Write((byte)1);
                context.Writer.Write(typeOfValue.AssemblyQualifiedName);

                Type currentItemType = null;
                context.Writer.Write(list.Count);
                foreach (var listItem in list)
                {
                    context.Writer.Write(listItem != null ? (byte)1 : (byte)0);
                    if (listItem == null)
                        continue;
                    var itemType = listItem.GetType();
                    context.Writer.Write(itemType != currentItemType ? (byte)1 : (byte)0);
                    if (currentItemType != itemType)
                    {
                        currentItemType = itemType;
                        context.Writer.Write(currentItemType.AssemblyQualifiedName);
                    }

                    SerializeCore(context, listItem, itemType);
                }


                return;
            }
            #endregion

            context.Writer.Write((byte)0);

            Serialize(context, value, typeOfValue);
        }
    }
}
