

var class6 = new Class6() { TestList = new List<string>(new[] { "val1", "val2", "val3" }) };
byte[] buffer;
using (var ms = new MemoryStream())
{
    Class6.Serialize(class6, ms);
    buffer = ms.ToArray();
}

Class6 deserialized;
using (var ms = new MemoryStream(buffer))
{
    deserialized = Class6.Deserialize(ms);
}

return class6.TestList == deserialized.TestList;