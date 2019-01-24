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
            HasAttribute(classDeclarationSyntax, "DataContract", "System.Runtime.Serialization.DataContract");
        public static bool IsSerializable(this PropertyDeclarationSyntax propertyDeclarationSyntax) =>
            HasAttribute(propertyDeclarationSyntax, "DataMember", "System.Runtime.Serialization.DataMember");
        public static bool IsSerializable(this FieldDeclarationSyntax fieldDeclarationSyntax) =>
            HasAttribute(fieldDeclarationSyntax, "DataMember", "System.Runtime.Serialization.DataMember");

        public static bool HasAttribute(this ClassDeclarationSyntax classDeclarationSyntax, params string[] attributes)
        {
            if (attributes == null)
            {
                throw new ArgumentNullException(nameof(attributes));
            }

            return classDeclarationSyntax.AttributeLists
                .Any(al => al.Attributes
                    .Any(a => attributes.Contains(a.Name.ToString())));
        }

        public static bool HasAttribute(this PropertyDeclarationSyntax propertyDeclarationSyntax, params string[] attributes)
        {
            if (attributes == null)
            {
                throw new ArgumentNullException(nameof(attributes));
            }

            return propertyDeclarationSyntax.AttributeLists
                .Any(al => al.Attributes
                    .Any(a => attributes.Contains(a.Name.ToString())));
        }

        public static bool HasAttribute(this FieldDeclarationSyntax fieldDeclarationSyntax, params string[] attributes)
        {
            if (attributes == null)
            {
                throw new ArgumentNullException(nameof(attributes));
            }

            return fieldDeclarationSyntax.AttributeLists
                .Any(al => al.Attributes
                    .Any(a => attributes.Contains(a.Name.ToString())));
        }

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
