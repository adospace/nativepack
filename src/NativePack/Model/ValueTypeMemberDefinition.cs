using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace NativePack.Model
{
    internal class ValueTypeMemberDefinition : MemberDefinition
    {
        private ValueTypeMemberDefinition(string name, ValueTypeKind type, bool nullable = false) : base(name)
        {
            Type = type;
            Nullable = nullable;
        }

        public ValueTypeKind Type { get; }
        public bool Nullable { get; }

        public static bool TryParse(PropertyDeclarationSyntax propertyDeclarationSyntax, out ValueTypeMemberDefinition propertyDefintion)
        {
            var name = propertyDeclarationSyntax.Identifier.Text;
            var type = propertyDeclarationSyntax.Type.ToString();

            return TryParse(name, type, out propertyDefintion);
        }

        public static bool TryParse(string name, string type, out ValueTypeMemberDefinition propertyDefintion)
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

            if (Utils.TryGetValueType(type, out var valueTypeKind, out var nullable))
                propertyDefintion = new ValueTypeMemberDefinition(name, valueTypeKind, nullable);

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
