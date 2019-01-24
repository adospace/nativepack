using System;
using System.Collections.Generic;
using System.Text;

namespace NativePack.Model
{
    internal class ValueTypeMemberDefintion : PropertyDefinition
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
                if (TestType(type, typeMatch.Value.Item1, typeMatch.Value.Item2))
                {
                    propertyDefintion = new ValueTypeMemberDefintion(name, typeMatch.Key);
                    break;
                }

                if (TestNullableType(type, typeMatch.Value.Item1, typeMatch.Value.Item2))
                {
                    propertyDefintion = new ValueTypeMemberDefintion(name, typeMatch.Key, true);
                    break;
                }
            }

            return propertyDefintion != null;
        }

        private static bool TestType(string typeString, string primitiveType, string systemType)
        {
            return typeString == primitiveType || typeString == systemType || typeString == "System." + systemType;
        }


        private static bool TestNullableType(string typeString, string primitiveType, string systemType)
        {
            return new List<string>(new[]{
                primitiveType + "?",
                systemType + "?",
                "System." + systemType + "?",
                "Nullable<" + primitiveType + ">",
                "Nullable<" + systemType + ">",
                "System.Nullable<" + primitiveType + ">",
                "System.Nullable<" + systemType + ">",
                "System.Nullable<System." + systemType + ">"})
                .Contains(typeString);
        }
    }
}
