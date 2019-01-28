using NativePack.Attributes;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace NativePack.Tests.Resources
{
    public enum TestEnum
    {
        EnumValue1,

        EnumValue2
    }

    [GenerateSerializer(includeTypeName:false)]
    partial class DummyClass
    {
        [GenerateSerializer(isEnum: true)]
        public TestEnum TestProperty { get; set; }

        [GenerateSerializer]
        public List<string> TestList { get; set; }
    }
}
