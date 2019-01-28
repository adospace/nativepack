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



            throw new NotImplementedException();
        }
    }
}
