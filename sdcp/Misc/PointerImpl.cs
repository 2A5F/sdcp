using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace SDcp.Misc;

public struct UnsafePointer
{
    public UnsafePointer(nuint ptr, nuint size)
    {
        Ptr = ptr;
        Size = size;
    }

    public nuint Ptr;
    public nuint Size;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Deconstruct(out nuint ptr, out nuint size)
    {
        ptr = Ptr;
        size = Size;
    }

    public UnsafePointer this[nuint index] => new(Ptr + index * Size, Size);
}

public struct UnsafeSpan
{
    public UnsafeSpan(UnsafePointer ptr, nuint length)
    {
        Ptr = ptr;
        Length = length;
    }

    public UnsafePointer Ptr;
    public nuint Length;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Deconstruct(out UnsafePointer ptr, out nuint length)
    {
        ptr = Ptr;
        length = Length;
    }

    public UnsafePointer this[nuint index] => Ptr[index];
}

public class UnsafeSpanImpl<TS> : ISerialize<UnsafeSpan> where TS : ISerialize<UnsafePointer>
{
    protected TS serialize;

    public UnsafeSpanImpl(TS serialize)
    {
        this.serialize = serialize;
    }

    public void Serialize<S>(S serializer, in UnsafeSpan value) where S : ISerializer
    {
        serializer.EnumerableStart(value.Length);
        for (nuint i = 0; i < value.Length; i++)
        {
            serializer.EnumerableSerializeElement(value[i], serialize);
        }
        serializer.EnumerableEnd();
    }
}

// only when T not byref struct
public unsafe class UnsafePointerImpl<T, TS> : ISerialize<UnsafePointer> where TS : ISerialize<T>
{
    protected TS serialize;

    public UnsafePointerImpl(TS serialize)
    {
        this.serialize = serialize;
    }

    public void Serialize<S>(S serializer, in UnsafePointer value) where S : ISerializer
    {
        ref readonly var r = ref Unsafe.AsRef<T>((void*)value.Ptr);
        serialize.Serialize(serializer, in r);
    }
}

#pragma warning disable CS8500
public static unsafe class SpanImplWarp<S, T, TS> where S : ISerializer where TS : ISerialize<UnsafeSpan>
{
    public static void Serialize(S serializer, in ReadOnlySpan<T> value, TS serialize)
    {
        fixed (void* p = value)
        {
            var us = new UnsafeSpan(new UnsafePointer((nuint)p, (nuint)sizeof(T)), (nuint)value.Length);
            serialize.Serialize(serializer, in us);
        }
    }
}
#pragma warning restore CS8500
