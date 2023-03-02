using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using SDcp.Primitives;

namespace SDcp.Runtime.Internal;

class Foo
{
    public static void Serialize1<S>(S serializer, in Bar value) where S : ISerializer
    {
        serializer.StructStart("Bar", 0);
        serializer.StructSerializeField("Foo", value.Foo, PrimitiveImpl.Instance);
        serializer.StructSerializeField("Foo2", in value.Foo2, PrimitiveImpl.Instance);
        serializer.StructEnd();
    }
    public static async ValueTask Serialize2<S>(S serializer, Bar value) where S : IAsyncSerializer
    {
        await serializer.StructStartAsync("Bar", 0);
        await serializer.StructSerializeFieldAsync("Foo", value.Foo, PrimitiveImpl.Instance);
        await serializer.StructSerializeFieldAsync("Foo2", value.Foo2, PrimitiveImpl.Instance);
        await serializer.StructEndAsync();
    }
}

class Foo2
{
    public static void Serialize1<S>(S serializer, ref Bar2 value) where S : ISerializer
    {
        serializer.StructStart("Bar", 0);
        serializer.StructSerializeField("Foo", value.Foo, PrimitiveImpl.Instance);
        serializer.StructSerializeField("Foo2", in value.Foo2, PrimitiveImpl.Instance);
        serializer.StructEnd();
    }
    public static async ValueTask Serialize2<S>(S serializer, Bar2 value) where S : IAsyncSerializer
    {
        await serializer.StructStartAsync("Bar", 0);
        await serializer.StructSerializeFieldAsync("Foo", value.Foo, PrimitiveImpl.Instance);
        await serializer.StructSerializeFieldAsync("Foo2", value.Foo2, PrimitiveImpl.Instance);
        await serializer.StructEndAsync();
    }
    public static void Serialize3<S, T>(S serializer, ref T value) where S : ISerializer where T : IFuck
    {
        serializer.StructStart("Bar", 0);
        serializer.StructSerializeField("Foo", value.Foo, PrimitiveImpl.Instance);
        serializer.StructEnd();
    }
}

interface IFuck
{
    public int Foo { get; }
}


class Bar
{
    public int Foo => 1;
    public int Foo2 = 1;

    public static int GetFoo(ref Bar bar) => bar.Foo;
    public static ref int GetFoo2(ref Bar bar) => ref bar.Foo2;
}
struct Bar2
{
    public int Foo => 1;
    public int Foo2 = 1;

    public Bar2()
    {
    }

    public static int GetFoo(ref Bar bar) => bar.Foo;
    public static ref int GetFoo2(ref Bar bar) => ref bar.Foo2;
}
