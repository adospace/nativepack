using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NativePack.Runtime
{
    public class BinarySerializer
    {
        public BinarySerializer()
        { }

        public void Serialize<T>(T value, Stream stream)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            var objectWriter = new ObjectWriter();
            using (var binaryWriter = new BinaryWriter(stream, Encoding.Default, true))
            {
                var ctx = new SerializerContext(binaryWriter);
                objectWriter.Serialize(ctx, value);
            }
        }

        public T Deserialize<T>(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            var objectReader = new ObjectReader();
            using (var binaryReader = new BinaryReader(stream, Encoding.Default, true))
            {
                var ctx = new DeserializerContext(binaryReader);
                return (T)objectReader.Deserialize(ctx);
            }
        }
    }

}
