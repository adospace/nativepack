using System;
using System.Collections.Generic;
using System.Text;

namespace NativePack.Model
{
    internal class Utils
    {
        public static bool TryParseNullable(string typeString, out string type)
        {
            type = null;
            if (typeString.EndsWith("?"))
            {
                type = typeString.Substring(0, typeString.Length - 1);
            }

            return type != null;
        }

        public static bool TestType(string typeString, string systemType)
        {
            return typeString == systemType || typeString == "System." + systemType;
        }


        public static bool TestNullableType(string typeString, string systemType)
        {
            return new List<string>(new[]{
                systemType + "?",
                "System." + systemType + "?",
                "Nullable<" + systemType + ">",
                "System.Nullable<" + systemType + ">",
                "System.Nullable<System." + systemType + ">"})
                .Contains(typeString);
        }

        public static bool TestType(string typeString, string primitiveType, string systemType)
        {
            return typeString == primitiveType || typeString == systemType || typeString == "System." + systemType;
        }

        public static bool TestNullableType(string typeString, string primitiveType, string systemType)
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

        public static bool TryGetValueType(string type, out ValueTypeKind valueTypeKind, out bool nullable)
        {
            valueTypeKind = ValueTypeKind.Undefined;
            nullable = false;

            foreach (var typeMatch in TypeMatch)
            {
                if (TestType(type, typeMatch.Value.Item1, typeMatch.Value.Item2))
                {
                    valueTypeKind = typeMatch.Key;
                    return true;
                }

                if (Utils.TestNullableType(type, typeMatch.Value.Item1, typeMatch.Value.Item2))
                {
                    nullable = true;
                    valueTypeKind = typeMatch.Key;
                    return true;
                }
            }

            return false;
        }


    }
}
