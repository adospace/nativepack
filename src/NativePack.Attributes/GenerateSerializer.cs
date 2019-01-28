using System;

namespace NativePack.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Property)]
    public class GenerateSerializer : Attribute
    {
        public GenerateSerializer(bool includeTypeName = true, bool callBaseSerializer = false, bool isEnum = false)
        {
            IncludeTypeName = includeTypeName;
            CallBaseSerializer = callBaseSerializer;
            IsEnum = isEnum;
        }

        public bool IncludeTypeName { get; }
        public bool CallBaseSerializer { get; }
        public bool IsEnum { get; }
    }
}
