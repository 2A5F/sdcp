using SDcp.Json;
using SDcp.Json.Internal;

namespace TestJson;

public class TestSerializeString
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestStringEscape1()
    {
        using var ms = new MemoryStream();
        var se = new StreamJsonSerializer<MemoryStream, CompactJsonFormatter>(ms, CompactJsonFormatter.Default);
        se.SerializeString("\n");
        ms.Position = 0;
        using var r = new StreamReader(ms);
        var s = r.ReadToEnd();
        Assert.That(s, Is.EqualTo("\"\\n\""));
    }

    [Test]
    public void TestStringEscape2()
    {
        using var ms = new MemoryStream();
        var se = new StreamJsonSerializer<MemoryStream, CompactJsonFormatter>(ms, CompactJsonFormatter.Default);
        se.SerializeString("\\");
        ms.Position = 0;
        using var r = new StreamReader(ms);
        var s = r.ReadToEnd();
        Assert.That(s, Is.EqualTo("\"\\\\\""));
    }

    [Test]
    public void TestStringEscape3()
    {
        using var ms = new MemoryStream();
        var se = new StreamJsonSerializer<MemoryStream, CompactJsonFormatter>(ms, CompactJsonFormatter.Default);
        se.SerializeString("\u001A");
        ms.Position = 0;
        using var r = new StreamReader(ms);
        var s = r.ReadToEnd();
        Assert.That(s, Is.EqualTo("\"\\u001A\""));
    }

    [Test]
    public void TestStringEscape4()
    {
        using var ms = new MemoryStream();
        var se = new StreamJsonSerializer<MemoryStream, CompactJsonFormatter>(ms, CompactJsonFormatter.Default);
        se.SerializeString("\u200D");
        ms.Position = 0;
        using var r = new StreamReader(ms);
        var s = r.ReadToEnd();
        Assert.That(s, Is.EqualTo("\"\\u200D\""));
    }

    [Test]
    public void TestStringEscape5()
    {
        using var ms = new MemoryStream();
        var se = new StreamJsonSerializer<MemoryStream, CompactJsonFormatter>(ms, CompactJsonFormatter.Default);
        se.SerializeString("\n\r\t");
        ms.Position = 0;
        using var r = new StreamReader(ms);
        var s = r.ReadToEnd();
        Assert.That(s, Is.EqualTo("\"\\n\\r\\t\""));
    }

    [Test]
    public void TestStringEscape6()
    {
        using var ms = new MemoryStream();
        var se = new StreamJsonSerializer<MemoryStream, CompactJsonFormatter>(ms, new CompactJsonFormatter
        {
            EscapeAllNonAsciiChar = true,
        });
        se.SerializeString("太六了");
        ms.Position = 0;
        using var r = new StreamReader(ms);
        var s = r.ReadToEnd();
        Assert.That(s, Is.EqualTo("\"\\u592A\\u516D\\u4E86\""));
    }

    [Test]
    public void TestStringEscape7()
    {
        using var ms = new MemoryStream();
        var se = new StreamJsonSerializer<MemoryStream, CompactJsonFormatter>(ms, CompactJsonFormatter.Default);
        se.SerializeString("太六了");
        ms.Position = 0;
        using var r = new StreamReader(ms);
        var s = r.ReadToEnd();
        Assert.That(s, Is.EqualTo("\"太六了\""));
    }
}
