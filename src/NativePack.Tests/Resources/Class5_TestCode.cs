

var class5 = new Class5() { TestProperty = TestEnum.EnumValue1 };
byte[] buffer;
using (var ms = new MemoryStream())
{
    Class5.Serialize(class5, ms);
    buffer = ms.ToArray();
}

Class5 deserialized;
using (var ms = new MemoryStream(buffer))
{
    deserialized = Class5.Deserialize(ms);
}

return class5.TestProperty == deserialized.TestProperty;