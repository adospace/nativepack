using System;
using System.Collections.Generic;
using System.Text;

namespace NativePack.Model
{
    internal class ValueTypeMemberDefintion : MemberDefinition
    {
        private ValueTypeMemberDefintion(string name, ValueTypeKind type, bool nullable = false) : base(name)
        {
            Type = type;
            Nullable = nullable;
        }

        public ValueTypeKind Type { get; }
        public bool Nullable { get; }

        private static readonly Dictionary<ValueTypeKind, Tuple<string, string>> TypeMatch = new Dictionary<ValueTypeKind, Tuple<string, string>>()
        {
            { ValueTypeKind.Boolean, new Tuple<string, string>("bool", "Boolean") },
            { ValueTypeKind.Byte, new Tuple<string, string>("byte", "Byte") },
            { ValueTypeKind.Char, new Tuple<string, string>("char", "Char") },
            { ValueTypeKind.Decimal, new Tuple<string, string>("decimal", "Decimal") },
            { ValueTypeKind.Double, new Tuple<string, string>("double", "Double") },
            { ValueTypeKind.Int16, new Tuple<string, string>("short", "Int16") },
            { ValueTypeKind.Int32, new Tuple<string, string>("int", "Int32") },
            { ValueTypeKind.Int64, new Tuple<string, string>("long", "Int64") },
            { ValueTypeKind.SByte, new Tuple<string, string>("sbyte", "SByte") },
            { ValueTypeKind.Single, new Tuple<string, string>("float", "Single") },
            { ValueTypeKind.UInt16, new Tuple<string, string>("ushort", "UInt16") },
            { ValueTypeKind.UInt32, new Tuple<string, string>("uint", "UInt32") },
            { ValueTypeKind.UInt64, new Tuple<string, string>("ulong", "UInt64") },
            { ValueTypeKind.String, new Tuple<string, string>("string", "String") },
        };

        public static bool TryParse(string name, string type, out ValueTypeMemberDefintion propertyDefintion)
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

            foreach (var typeMatch in TypeMatch)
            {
                if (Utils.TestType(type, typeMatch.Value.Item1, typeMatch.Value.Item2))
                {
                    propertyDefintion = new ValueTypeMemberDefintion(name, typeMatch.Key);
                    break;
                }

                if (Utils.TestNullableType(type, typeMatch.Value.Item1, typeMatch.Value.Item2))
                {
                    propertyDefintion = new ValueTypeMemberDefintion(name, typeMatch.Key, true);
                    break;
                }
            }

            return propertyDefintion != null;
        }

        public override string GenerateSerializerCode()
        {
            if (!Nullable)
                return $"writer.Write({Name});";
            else
                return $"writer.Write({Name} != null ? (byte)1 : (byte)0);" + Environment.NewLine +
                    $"            if ({Name} != null)" + Environment.NewLine +
                    $"                writer.Write({Name});";
        }

        public override string GenerateDeserializerCode()
        {
            if (!Nullable)
                return $"{Name} = reader.Read{Type}();";
            else
                return $"if (reader.ReadByte() == (byte)1)" + Environment.NewLine +
                    $"                {Name} = reader.Read{Type}();";
        }
    }
}
