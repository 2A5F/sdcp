using SDcp.Json;
using SDcp.Json.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestJson;

public class TestRuntimeSerialize
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestRuntime1()
    {
        var r = SDJson.Serializer.ToString().Serialize(123);
        Assert.That(r, Is.EqualTo("123"));
    }

    class Foo
    {
        public int Bar => 123;
        public int Baz = 456;
        public Bar1 Asd = new();
    }

    class Bar1
    {
        public int Foo = 789;
    }

    struct FooS
    {
        public int Bar => 123;
        public int Baz = 456;
        public Bar1S Asd = new();

        public FooS() { }
    }

    struct Bar1S
    {
        public int Foo = 789;

        public Bar1S() { }
    }


    [Test]
    public void TestRuntime2()
    {
        var r = SDJson.Serializer.ToString().Serialize(new Foo());
        Assert.That(r, Is.EqualTo("{\"Bar\":123,\"Baz\":456,\"Asd\":{\"Foo\":789}}"));
    }

    [Test]
    public void TestRuntime3()
    {
        var r = SDJson.Serializer.ToString().Serialize(new FooS());
        Assert.That(r, Is.EqualTo("{\"Bar\":123,\"Baz\":456,\"Asd\":{\"Foo\":789}}"));
    }

    //[Test]
    //public async ValueTask TestRuntimeAsync2()
    //{
    //    using var ms = new MemoryStream();
    //    await SDJson.Serializer.ToStream(ms).SerializeAsync(new Foo());
    //    ms.Position = 0;
    //    var r = await new StreamReader(ms).ReadToEndAsync();
    //    Assert.That(r, Is.EqualTo("{\"Bar\":123,\"Baz\":456,\"Asd\":{\"Foo\":789}}"));
    //}

}
