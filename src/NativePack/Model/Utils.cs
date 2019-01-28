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

    }
}
