using SDcp.Json;
using SDcp.Json.Internal;

namespace TestJson;

public class TestSerializeNumber
{
    [SetUp]
    public void Setup()
    {
    }
    
    [Test]
    public void TestInt1()
    {
        using var ms = new MemoryStream();
        var se = new StreamJsonSerializer<MemoryStream, CompactJsonFormatter>(ms, CompactJsonFormatter.Default);
        se.SerializeInt32(123);
        ms.Position = 0;
        using var r = new StreamReader(ms);
        var s = r.ReadToEnd();
        Assert.That(s, Is.EqualTo("123"));
    }

    [Test]
    public void TestInt2()
    {
        using var ms = new MemoryStream();
        var se = new StreamJsonSerializer<MemoryStream, CompactJsonFormatter>(ms, CompactJsonFormatter.Default);
        se.SerializeInt64(123);
        ms.Position = 0;
        using var r = new StreamReader(ms);
        var s = r.ReadToEnd();
        Assert.That(s, Is.EqualTo("\"123\""));
    }
}
