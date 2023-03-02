using System.Text;
using SDcp.Json;
using SDcp.Json.Internal;

namespace TestJson;

public class TestSerializeEnum
{
    [SetUp]
    public void Setup()
    {
    }

    enum Enum1
    {
        Foo, Bar,
    }

    [Test]
    public void TestEnum1()
    {
        var sb = new StringBuilder();
        var se = new StringJsonSerializer<CompactJsonFormatter>(sb, new CompactJsonFormatter { EnumUseString = false });
        se.SerializeEnum(Enum1.Foo);
        var s = sb.ToString();
        Assert.That(s, Is.EqualTo("0"));
    }

    [Test]
    public void TestEnum2()
    {
        var sb = new StringBuilder();
        var se = new StringJsonSerializer<CompactJsonFormatter>(sb, new CompactJsonFormatter { EnumUseString = false });
        se.SerializeEnum(Enum1.Bar);
        var s = sb.ToString();
        Assert.That(s, Is.EqualTo("1"));
    }
}
