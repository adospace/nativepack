using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NativePack
{
    public static class SyntaxAnalisysExtensions
    {
        public static bool IsSerializable(this ClassDeclarationSyntax classDeclarationSyntax) =>
            HasAttribute(classDeclarationSyntax.AttributeLists, "GenerateSerializer", "NativePack.Attributes.GenerateSerializer");
        public static bool IsSerializable(this PropertyDeclarationSyntax propertyDeclarationSyntax) =>
            HasAttribute(propertyDeclarationSyntax.AttributeLists, "GenerateSerializer", "NativePack.Attributes.GenerateSerializer");
        public static bool IsSerializable(this FieldDeclarationSyntax fieldDeclarationSyntax) =>
            HasAttribute(fieldDeclarationSyntax.AttributeLists, "GenerateSerializer", "NativePack.Attributes.GenerateSerializer");

        public static bool HasAttribute(this SyntaxList<AttributeListSyntax> attributeListSyntaxes, params string[] attributes)
        {
            if (attributeListSyntaxes == null)
            {
                throw new ArgumentNullException(nameof(attributes));
            }

            return attributeListSyntaxes
                .Any(al => al.Attributes
                    .Any(a => attributes.Contains(a.Name.ToString())));
        }

        public static AttributeSyntax GetSerializerAttribute(this SyntaxList<AttributeListSyntax> attributeListSyntaxes) =>
            GetAttribute(attributeListSyntaxes, "GenerateSerializer") ?? GetAttribute(attributeListSyntaxes, "NativePack.Attributes.GenerateSerializer");

        public static AttributeSyntax GetAttribute(this SyntaxList<AttributeListSyntax> attributeListSyntaxes, string name)
        {
            return attributeListSyntaxes
                .SelectMany(al => al.Attributes)
                .FirstOrDefault(a => a.Name.ToString() == name);
        }

        public static IEnumerable<AttributeSyntax> GetAllAttributes(this SyntaxList<AttributeListSyntax> attributeListSyntaxes, string name)
        {
            return attributeListSyntaxes
                .SelectMany(al => al.Attributes)
                .Where(a => a.Name.ToString() == name)
                .ToList();
        }

        public static AttributeArgumentSyntax GetArgument(this AttributeSyntax attribute, string name) =>
            attribute.ArgumentList.DescendantNodes().OfType<AttributeArgumentSyntax>().FirstOrDefault(_ => _.NameColon.Name.Identifier.Text == name);

        public static bool IsTrue(this AttributeArgumentSyntax attributeArgument) =>
            attributeArgument.Expression.Kind() == Microsoft.CodeAnalysis.CSharp.SyntaxKind.TrueLiteralExpression;

        public static bool IsFalse(this AttributeArgumentSyntax attributeArgument) =>
            attributeArgument.Expression.Kind() == Microsoft.CodeAnalysis.CSharp.SyntaxKind.FalseLiteralExpression;

        public static bool HasAttributePropertySet(this SyntaxList<AttributeListSyntax> attributeListSyntaxes, string propertyName, bool defaultValue)
        {
            var attributeIncludeTypeName = attributeListSyntaxes.GetSerializerAttribute();
            if (attributeIncludeTypeName == null)
                return defaultValue;

            var attributeIncludeTypeNameArgument = attributeIncludeTypeName.GetArgument(propertyName);
            if (attributeIncludeTypeNameArgument == null)
                return defaultValue;

            return attributeIncludeTypeNameArgument.IsTrue();
        }

        public static bool IncludeTypeName(this ClassDeclarationSyntax classDeclarationSyntax) =>
            HasAttributePropertySet(classDeclarationSyntax.AttributeLists, "includeTypeName", true);

        public static bool CallBaseSerializer(this ClassDeclarationSyntax classDeclarationSyntax) =>
            HasAttributePropertySet(classDeclarationSyntax.AttributeLists, "callBaseSerializer", false);

        public static bool IsEnum(this PropertyDeclarationSyntax propertyDeclarationSyntax) =>
            HasAttributePropertySet(propertyDeclarationSyntax.AttributeLists, "isEnum", false);

        public static bool IsEnum(this FieldDeclarationSyntax fieldDeclarationSyntax) =>
            HasAttributePropertySet(fieldDeclarationSyntax.AttributeLists, "isEnum", false);

        public static string ContainingNamespace(this ClassDeclarationSyntax classDeclarationSyntax)
        {
            if (classDeclarationSyntax == null)
            {
                throw new ArgumentNullException(nameof(classDeclarationSyntax));
            }

            var parentNode = classDeclarationSyntax.Parent;

            string ns = null;
            while (!(parentNode is NamespaceDeclarationSyntax))
            {
                if (parentNode is ClassDeclarationSyntax)
                {
                    ns = ((ClassDeclarationSyntax)parentNode).Identifier.Text + (ns == null ? string.Empty : "." + ns);
                }
                else
                {
                    return ns;
                }

                parentNode = parentNode.Parent;
            }

            return ((NamespaceDeclarationSyntax)parentNode).Name.ToString() + (ns == null ? string.Empty : "." + ns);
        }
    }
}
