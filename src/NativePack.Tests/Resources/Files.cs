using System;
using System.IO;
using System.Reflection;

namespace NativePack.Tests.Resources
{
    internal static class Files
    {
        public static string Class1() => GetFileContent("Class1.cs");
        public static string Class2() => GetFileContent("Class2.cs");
        public static string Class3() => GetFileContent("Class3.cs");
        public static string Class4() => GetFileContent("Class4.cs");
        public static string Class4_TestCode() => GetFileContent("Class4_TestCode.cs");
        public static string Class5() => GetFileContent("Class5.cs");
        public static string Class5_TestCode() => GetFileContent("Class5_TestCode.cs");

        public static string GetFileContent(string resourceName)
        {
            if (string.IsNullOrWhiteSpace(resourceName))
            {
                throw new ArgumentException("can't be empty or null", nameof(resourceName));
            }

            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream($"NativePack.Tests.Resources.{resourceName}")))
                return sr.ReadToEnd();
        }

    }
}