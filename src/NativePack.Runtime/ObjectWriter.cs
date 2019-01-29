using System;
using System.IO;
using System.Reflection;

namespace NativePack.Runtime
{
    public class ObjectWriter
    {
        public ObjectWriter()
        {
        }

        public void Serialize(SerializerContext context, object value)
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

            context.Writer.Write(typeValue.AssemblyQualifiedName);

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

            switch (propertyValue)
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

            #region IEnumerable

            #endregion

            Serialize(context, propertyValue);
        }
    }
}
