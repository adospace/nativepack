

var class4 = new Class4() { PropertyInt1 = true, PropertyInt2 = 100 };
byte[] buffer;
using (var ms = new MemoryStream())
{
    Class4.Serialize(class4, ms);
    buffer = ms.ToArray();
}

Class4 deserialized;
using (var ms = new MemoryStream(buffer))
{
    deserialized = Class4.Deserialize(ms);
}

return class4.PropertyInt1 == deserialized.PropertyInt1 && class4.PropertyInt2 == deserialized.PropertyInt2;