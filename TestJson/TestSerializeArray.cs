using SDcp.Json.Internal;
using SDcp.Json;
using System.Text;
using SDcp;
using SDcp.Primitives;
using static SDcp.Json.SDJson;

namespace TestJson;

public class TestSerializeArray
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestArray1()
    {
        var sb = new StringBuilder();
        var se = new StringJsonSerializer<CompactJsonFormatter>(sb, new CompactJsonFormatter { EnumUseString = false });
        se.EnumerableStart(null);
        se.EnumerableSerializeElement(123, PrimitiveImpl.Instance);
        se.EnumerableSerializeElement(456, PrimitiveImpl.Instance);
        se.EnumerableEnd();
        var s = sb.ToString();
        Assert.That(s, Is.EqualTo("[123,456]"));
    }

    class TestArray2Class : ISerialize<int>
    {
        public void Serialize<S>(S serializer, in int value) where S : ISerializer
        {
            serializer.EnumerableStart(null);
            serializer.EnumerableSerializeElement(123, PrimitiveImpl.Instance);
            serializer.EnumerableSerializeElement(456, PrimitiveImpl.Instance);
            serializer.EnumerableEnd();
        }
    }

    [Test]
    public void TestArray2()
    {
        var sb = new StringBuilder();
        var se = new StringJsonSerializer<CompactJsonFormatter>(sb, new CompactJsonFormatter { EnumUseString = false });
        se.EnumerableStart(null);
        se.EnumerableSerializeElement(0, new TestArray2Class());
        se.EnumerableSerializeElement(789, PrimitiveImpl.Instance);
        se.EnumerableEnd();
        var s = sb.ToString();
        Assert.That(s, Is.EqualTo("[[123,456],789]"));
    }

    class TestArray3Class : ISerialize<int>
    {
        public void Serialize<S>(S serializer, in int value) where S : ISerializer
        {
            serializer.EnumerableStart(null);
            serializer.EnumerableSerializeElement(456, PrimitiveImpl.Instance);
            serializer.EnumerableSerializeElement(789, PrimitiveImpl.Instance);
            serializer.EnumerableEnd();
        }
    }

    [Test]
    public void TestArray3()
    {
        var sb = new StringBuilder();
        var se = new StringJsonSerializer<CompactJsonFormatter>(sb, new CompactJsonFormatter { EnumUseString = false });
        se.EnumerableStart(null);
        se.EnumerableSerializeElement(123, PrimitiveImpl.Instance);
        se.EnumerableSerializeElement(0, new TestArray3Class());
        se.EnumerableEnd();
        var s = sb.ToString();
        Assert.That(s, Is.EqualTo("[123,[456,789]]"));
    }
}
