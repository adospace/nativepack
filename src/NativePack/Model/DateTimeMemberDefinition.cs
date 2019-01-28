using System;
using System.Collections.Generic;
using System.Text;

namespace NativePack.Model
{
    public class DateTimeMemberDefinition : MemberDefinition
    {
        public DateTimeMemberDefinition(string name, bool nullable = false) : base(name)
        {
            Nullable = nullable;
        }

        public bool Nullable { get; }

        public static bool TryParse(string name, string type, out DateTimeMemberDefinition propertyDefintion)
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

            if (Utils.TestType(type, "DateTime"))
            {
                propertyDefintion = new DateTimeMemberDefinition(name);
                return true;
            }

            if (Utils.TestNullableType(type, "DateTime"))
            {
                propertyDefintion = new DateTimeMemberDefinition(name, true);
                return true;

            }

            return false;
        }


        public override string GenerateSerializerCode()
        {
            if (!Nullable)
                return $"writer.Write({Name}.ToBinary());";
            else
                return $"writer.Write({Name} != null ? (byte)1 : (byte)0);" + Environment.NewLine +
                    $"            if ({Name} != null)" + Environment.NewLine +
                    $"                writer.Write({Name}.ToBinary());";
        }

        public override string GenerateDeserializerCode()
        {
            if (!Nullable)
                return $"{Name} = DateTime.FromBinary(reader.ReadInt64());";
            else
                return $"if (reader.ReadByte() == (byte)1)" + Environment.NewLine +
                    $"                {Name} = DateTime.FromBinary(reader.ReadInt64());";
        }
    }
}
