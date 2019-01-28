using System;
using System.Collections.Generic;
using System.Text;

namespace NativePack.Model
{
    internal class EnumPropertyDefinition : MemberDefinition
    {
        public EnumPropertyDefinition(string name, string type, bool nullable = false) : base(name)
        {
            Type = type;
            Nullable = nullable;
        }

        public string Type { get; }
        public bool Nullable { get; }

        public static bool TryParse(string name, string type, out EnumPropertyDefinition propertyDefintion)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("can't be null or empty", nameof(name));
            }

            if (string.IsNullOrWhiteSpace(type))
            {
                throw new ArgumentException("can't be null or empty", nameof(type));
            }

            if (Utils.TryParseNullable(type, out var internalType))
                propertyDefintion = new EnumPropertyDefinition(name, internalType, true);
            else
                propertyDefintion = new EnumPropertyDefinition(name, type, true);

            return true;
        }


        public override string GenerateSerializerCode()
        {
            if (!Nullable)
                return $"writer.Write((int){Name});";
            else
                return $"writer.Write({Name} != null ? (byte)1 : (byte)0);" + Environment.NewLine +
                    $"            if ({Name} != null)" + Environment.NewLine +
                    $"                writer.Write((int){Name});";
        }

        public override string GenerateDeserializerCode()
        {
            if (!Nullable)
                return $"{Name} = ({Type})reader.ReadInt32();";
            else
                return $"if (reader.ReadByte() == (byte)1)" + Environment.NewLine +
                    $"                {Name} = ({Type})reader.ReadInt32();";
        }

    }
}
