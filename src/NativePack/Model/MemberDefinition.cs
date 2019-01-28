using System;
using System.Collections.Generic;
using System.Text;

namespace NativePack.Model
{
    public abstract class MemberDefinition
    {
        public MemberDefinition(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public abstract string GenerateSerializerCode();

        public abstract string GenerateDeserializerCode();
    }
}
