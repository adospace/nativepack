using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NativePack.Runtime
{
    public class SerializerContext
    {
        internal SerializerContext(BinaryWriter binaryWriter)
        {
            Writer = binaryWriter;
        }

        public BinaryWriter Writer { get; }
    }
}
