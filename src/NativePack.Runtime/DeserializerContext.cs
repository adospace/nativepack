using System.IO;

namespace NativePack.Runtime
{
    public class DeserializerContext
    {
        internal DeserializerContext(BinaryReader reader)
        {
            Reader = reader;
        }

        public BinaryReader Reader { get; }
    }
}