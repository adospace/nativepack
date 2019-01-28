
    public enum TestEnum
    {
        EnumValue1,

        EnumValue2
    }

    [GenerateSerializer(includeTypeName : false)]
    partial class Class5
    {
        [GenerateSerializer(isEnum: true)]
        public TestEnum TestProperty { get; set; }
    }
