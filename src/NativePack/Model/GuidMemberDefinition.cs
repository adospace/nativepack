using System;
using System.Collections.Generic;
using System.Text;

namespace NativePack.Model
{
    public class GuidMemberDefinition : MemberDefinition
    {
        public GuidMemberDefinition(string name, bool nullable = false) : base(name)
        {
            Nullable = nullable;
        }

        public bool Nullable { get; }

        public static bool TryParse(string name, string type, out GuidMemberDefinition propertyDefintion)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("can't be null or empty", nameof(name));
            }

            if (string.IsNullOrWhiteSpace(type))
            {
                throw new ArgumentException("can't be null or empty", nameof(type));
            }

            propertyDefintion = null;

            if (Utils.TestType(type, "Guid"))
            {
                propertyDefintion = new GuidMemberDefinition(name);
                return true;
            }

            if (Utils.TestNullableType(type, "Guid"))
            {
                propertyDefintion = new GuidMemberDefinition(name, true);
                return true;
            }

            return false;
        }


        public override string GenerateSerializerCode()
        {
            if (!Nullable)
                return $"writer.Write({Name}.ToByteArray());";
            else
                return $"writer.Write({Name} != null ? (byte)1 : (byte)0);" + Environment.NewLine +
                    $"            if ({Name} != null)" + Environment.NewLine +
                    $"                writer.Write({Name}.ToByteArray());";
        }

        public override string GenerateDeserializerCode()
        {
            if (!Nullable)
                return $"{Name} = new Guid(reader.ReadBytes(16));";
            else
                return $"if (reader.ReadByte() == (byte)1)" + Environment.NewLine +
                    $"                {Name} = new Guid(reader.ReadBytes(16));";
        }
    }
}
