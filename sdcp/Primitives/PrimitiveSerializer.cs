using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SDcp.Primitives;

public partial class PrimitiveImpl : ISerialize<bool>, IAsyncSerialize<bool>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Serialize<S>(S serializer, in bool value) where S : ISerializer
        => serializer.SerializeBool(value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTask SerializeAsync<S>(S serializer, bool value) where S : IAsyncSerializer
        => serializer.SerializeBoolAsync(value);
}

public partial class PrimitiveImpl : ISerialize<sbyte>, IAsyncSerialize<sbyte>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Serialize<S>(S serializer, in sbyte value) where S : ISerializer
        => serializer.SerializeSByte(value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTask SerializeAsync<S>(S serializer, sbyte value) where S : IAsyncSerializer
        => serializer.SerializeSByteAsync(value);
}

public partial class PrimitiveImpl : ISerialize<short>, IAsyncSerialize<short>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Serialize<S>(S serializer, in short value) where S : ISerializer
        => serializer.SerializeInt16(value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTask SerializeAsync<S>(S serializer, short value) where S : IAsyncSerializer
        => serializer.SerializeInt16Async(value);
}

public partial class PrimitiveImpl : ISerialize<int>, IAsyncSerialize<int>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Serialize<S>(S serializer, in int value) where S : ISerializer
        => serializer.SerializeInt32(value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTask SerializeAsync<S>(S serializer, int value) where S : IAsyncSerializer
        => serializer.SerializeInt32Async(value);
}

public partial class PrimitiveImpl : ISerialize<long>, IAsyncSerialize<long>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Serialize<S>(S serializer, in long value) where S : ISerializer
        => serializer.SerializeInt64(value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTask SerializeAsync<S>(S serializer, long value) where S : IAsyncSerializer
        => serializer.SerializeInt64Async(value);
}

public partial class PrimitiveImpl : ISerialize<Int128>, IAsyncSerialize<Int128>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Serialize<S>(S serializer, in Int128 value) where S : ISerializer
        => serializer.SerializeInt128(value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTask SerializeAsync<S>(S serializer, Int128 value) where S : IAsyncSerializer
        => serializer.SerializeInt128Async(value);
}

public partial class PrimitiveImpl : ISerialize<byte>, IAsyncSerialize<byte>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Serialize<S>(S serializer, in byte value) where S : ISerializer
        => serializer.SerializeByte(value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTask SerializeAsync<S>(S serializer, byte value) where S : IAsyncSerializer
        => serializer.SerializeByteAsync(value);
}

public partial class PrimitiveImpl : ISerialize<ushort>, IAsyncSerialize<ushort>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Serialize<S>(S serializer, in ushort value) where S : ISerializer
        => serializer.SerializeUInt16(value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTask SerializeAsync<S>(S serializer, ushort value) where S : IAsyncSerializer
        => serializer.SerializeUInt16Async(value);
}

public partial class PrimitiveImpl : ISerialize<uint>, IAsyncSerialize<uint>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Serialize<S>(S serializer, in uint value) where S : ISerializer
        => serializer.SerializeUInt32(value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTask SerializeAsync<S>(S serializer, uint value) where S : IAsyncSerializer
        => serializer.SerializeUInt32Async(value);
}

public partial class PrimitiveImpl : ISerialize<ulong>, IAsyncSerialize<ulong>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Serialize<S>(S serializer, in ulong value) where S : ISerializer
        => serializer.SerializeUInt64(value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTask SerializeAsync<S>(S serializer, ulong value) where S : IAsyncSerializer
        => serializer.SerializeUInt64Async(value);
}

public partial class PrimitiveImpl : ISerialize<UInt128>, IAsyncSerialize<UInt128>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Serialize<S>(S serializer, in UInt128 value) where S : ISerializer
        => serializer.SerializeUInt128(value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTask SerializeAsync<S>(S serializer, UInt128 value) where S : IAsyncSerializer
        => serializer.SerializeUInt128Async(value);
}

public partial class PrimitiveImpl : ISerialize<nint>, IAsyncSerialize<nint>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Serialize<S>(S serializer, in nint value) where S : ISerializer
        => serializer.SerializeIntPtr(value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTask SerializeAsync<S>(S serializer, nint value) where S : IAsyncSerializer
        => serializer.SerializeIntPtrAsync(value);
}

public partial class PrimitiveImpl : ISerialize<nuint>, IAsyncSerialize<nuint>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Serialize<S>(S serializer, in nuint value) where S : ISerializer
        => serializer.SerializeUIntPtr(value);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTask SerializeAsync<S>(S serializer, nuint value) where S : IAsyncSerializer
        => serializer.SerializeUIntPtrAsync(value);
}

public partial class PrimitiveImpl : ISerialize<Half>, IAsyncSerialize<Half>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Serialize<S>(S serializer, in Half value) where S : ISerializer
        => serializer.SerializeHalf(value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTask SerializeAsync<S>(S serializer, Half value) where S : IAsyncSerializer
        => serializer.SerializeHalfAsync(value);
}

public partial class PrimitiveImpl : ISerialize<float>, IAsyncSerialize<float>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Serialize<S>(S serializer, in float value) where S : ISerializer
        => serializer.SerializeSingle(value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTask SerializeAsync<S>(S serializer, float value) where S : IAsyncSerializer
        => serializer.SerializeSingleAsync(value);
}

public partial class PrimitiveImpl : ISerialize<double>, IAsyncSerialize<double>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Serialize<S>(S serializer, in double value) where S : ISerializer
        => serializer.SerializeDouble(value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTask SerializeAsync<S>(S serializer, double value) where S : IAsyncSerializer
        => serializer.SerializeDoubleAsync(value);
}

public partial class PrimitiveImpl : ISerialize<decimal>, IAsyncSerialize<decimal>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Serialize<S>(S serializer, in decimal value) where S : ISerializer
        => serializer.SerializeDecimal(value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTask SerializeAsync<S>(S serializer, decimal value) where S : IAsyncSerializer
        => serializer.SerializeDecimalAsync(value);
}

public partial class PrimitiveImpl : ISerialize<BigInteger>, IAsyncSerialize<BigInteger>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Serialize<S>(S serializer, in BigInteger value) where S : ISerializer
        => serializer.SerializeBigInteger(value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTask SerializeAsync<S>(S serializer, BigInteger value) where S : IAsyncSerializer
        => serializer.SerializeBigIntegerAsync(value);
}

public partial class PrimitiveImpl : ISerialize<Complex>, IAsyncSerialize<Complex>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Serialize<S>(S serializer, in Complex value) where S : ISerializer
        => serializer.SerializeComplex(value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTask SerializeAsync<S>(S serializer, Complex value) where S : IAsyncSerializer
        => serializer.SerializeComplexAsync(value);
}

public partial class PrimitiveImpl : ISerialize<DateOnly>, IAsyncSerialize<DateOnly>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Serialize<S>(S serializer, in DateOnly value) where S : ISerializer
        => serializer.SerializeDateOnly(value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTask SerializeAsync<S>(S serializer, DateOnly value) where S : IAsyncSerializer
        => serializer.SerializeDateOnlyAsync(value);
}

public partial class PrimitiveImpl : ISerialize<DateTime>, IAsyncSerialize<DateTime>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Serialize<S>(S serializer, in DateTime value) where S : ISerializer
        => serializer.SerializeDateTime(value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTask SerializeAsync<S>(S serializer, DateTime value) where S : IAsyncSerializer
        => serializer.SerializeDateTimeAsync(value);
}

public partial class PrimitiveImpl : ISerialize<DateTimeOffset>, IAsyncSerialize<DateTimeOffset>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Serialize<S>(S serializer, in DateTimeOffset value) where S : ISerializer
        => serializer.SerializeDateTimeOffset(value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTask SerializeAsync<S>(S serializer, DateTimeOffset value) where S : IAsyncSerializer
        => serializer.SerializeDateTimeOffsetAsync(value);
}

public partial class PrimitiveImpl : ISerialize<Guid>, IAsyncSerialize<Guid>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Serialize<S>(S serializer, in Guid value) where S : ISerializer
        => serializer.SerializeGuid(value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTask SerializeAsync<S>(S serializer, Guid value) where S : IAsyncSerializer
        => serializer.SerializeGuidAsync(value);
}

public partial class PrimitiveImpl : ISerialize<Range>, IAsyncSerialize<Range>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Serialize<S>(S serializer, in Range value) where S : ISerializer
        => serializer.SerializeRange(value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTask SerializeAsync<S>(S serializer, Range value) where S : IAsyncSerializer
        => serializer.SerializeRangeAsync(value);
}

public partial class PrimitiveImpl : ISerialize<Index>, IAsyncSerialize<Index>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Serialize<S>(S serializer, in Index value) where S : ISerializer
        => serializer.SerializeIndex(value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTask SerializeAsync<S>(S serializer, Index value) where S : IAsyncSerializer
        => serializer.SerializeIndexAsync(value);
}

public partial class PrimitiveImpl : ISerialize<char>, IAsyncSerialize<char>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Serialize<S>(S serializer, in char value) where S : ISerializer
        => serializer.SerializeChar(value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTask SerializeAsync<S>(S serializer, char value) where S : IAsyncSerializer
        => serializer.SerializeCharAsync(value);
}

public partial class PrimitiveImpl : ISerialize<Rune>, IAsyncSerialize<Rune>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Serialize<S>(S serializer, in Rune value) where S : ISerializer
        => serializer.SerializeRune(value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTask SerializeAsync<S>(S serializer, Rune value) where S : IAsyncSerializer
        => serializer.SerializeRuneAsync(value);
}

public partial class PrimitiveImpl : ISerialize<string>, IAsyncSerialize<string>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Serialize<S>(S serializer, in string value) where S : ISerializer
        => serializer.SerializeString(value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTask SerializeAsync<S>(S serializer, string value) where S : IAsyncSerializer
        => serializer.SerializeStringAsync(value);
}
