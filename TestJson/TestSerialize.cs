using SDcp.Json;
using SDcp.Json.Runtime;
using SDcp.Primitives;
using SDcp.Runtime;
using System.Runtime.InteropServices;
using SDcp.Json.Internal;
using SDcp;
using System.Reflection.Emit;
using System.Reflection;
using System.Text;

namespace TestJson;

public class TestSerialize
{
    [SetUp]
    public void Setup()
    {
    }


    [Test]
    public void Test1()
    {
        var r = SDJson.Serializer.ToString().Serialize(123, PrimitiveImpl.Instance);
        Assert.That(r, Is.EqualTo("123"));
    }

}
