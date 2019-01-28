using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace NativePack.Model
{
    public class ListMemberDefinition : MemberDefinition
    {
        public ValueTypeKind Type { get; }
        public bool Nullable { get; }

        public ListMemberDefinition(string name, ValueTypeKind type, bool nullable = false) : base(name)
        {
            Type = type;
            Nullable = nullable;
        }

        public static bool TryParse(PropertyDeclarationSyntax propertyDeclarationSyntax, out ListMemberDefinition propertyDefintion)
        {
            propertyDefintion = null;
            var name = propertyDeclarationSyntax.Identifier.Text;

            if (!(propertyDeclarationSyntax.Type is GenericNameSyntax typeNameSyntax))
                return false;

            if (!(typeNameSyntax.Identifier.Text == "List" || typeNameSyntax.Identifier.Text == "System.Collections.Generic.List"))
                return false;

            if (typeNameSyntax.TypeArgumentList.Arguments.Count != 1)
                return false;

            var type = typeNameSyntax.TypeArgumentList.Arguments[0].ToString();

            Utils.TryGetValueType(type, out var valueTypeKind, out var nullable);

            propertyDefintion = new ListMemberDefinition(name, valueTypeKind, nullable);

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
