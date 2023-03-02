using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SDcp.Primitives;

public partial class PrimitiveImpl : IPrimitiveMark<bool>, ITypeMark<bool>, IAsyncTypeMark<bool>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void MatchType<M>(M matcher, bool _) where M : ITypeMatch => matcher.MatchBool();
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTask MatchTypeAsync<M>(M matcher, bool _) where M : IAsyncTypeMatch => matcher.MatchBoolAsync();
}

public partial class PrimitiveImpl : IPrimitiveMark<sbyte>, ITypeMark<sbyte>, IAsyncTypeMark<sbyte>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void MatchType<M>(M matcher, sbyte _) where M : ITypeMatch => matcher.MatchSByte();
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTask MatchTypeAsync<M>(M matcher, sbyte _) where M : IAsyncTypeMatch => matcher.MatchSByteAsync();
}

public partial class PrimitiveImpl : IPrimitiveMark<short>, ITypeMark<short>, IAsyncTypeMark<short>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void MatchType<M>(M matcher, short _) where M : ITypeMatch => matcher.MatchInt16();
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTask MatchTypeAsync<M>(M matcher, short _) where M : IAsyncTypeMatch => matcher.MatchInt16Async();
}

public partial class PrimitiveImpl : IPrimitiveMark<int>, ITypeMark<int>, IAsyncTypeMark<int>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void MatchType<M>(M matcher, int _) where M : ITypeMatch => matcher.MatchInt32();
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTask MatchTypeAsync<M>(M matcher, int _) where M : IAsyncTypeMatch => matcher.MatchInt32Async();
}

public partial class PrimitiveImpl : IPrimitiveMark<long>, ITypeMark<long>, IAsyncTypeMark<long>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void MatchType<M>(M matcher, long _) where M : ITypeMatch => matcher.MatchInt64();
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTask MatchTypeAsync<M>(M matcher, long _) where M : IAsyncTypeMatch => matcher.MatchInt64Async();
}

public partial class PrimitiveImpl : IPrimitiveMark<Int128>, ITypeMark<Int128>, IAsyncTypeMark<Int128>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void MatchType<M>(M matcher, Int128 _) where M : ITypeMatch => matcher.MatchInt128();
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTask MatchTypeAsync<M>(M matcher, Int128 _) where M : IAsyncTypeMatch => matcher.MatchInt128Async();
}

public partial class PrimitiveImpl : IPrimitiveMark<byte>, ITypeMark<byte>, IAsyncTypeMark<byte>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void MatchType<M>(M matcher, byte _) where M : ITypeMatch => matcher.MatchByte();
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTask MatchTypeAsync<M>(M matcher, byte _) where M : IAsyncTypeMatch => matcher.MatchByteAsync();
}

public partial class PrimitiveImpl : IPrimitiveMark<ushort>, ITypeMark<ushort>, IAsyncTypeMark<ushort>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void MatchType<M>(M matcher, ushort _) where M : ITypeMatch => matcher.MatchUInt16();
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTask MatchTypeAsync<M>(M matcher, ushort _) where M : IAsyncTypeMatch => matcher.MatchUInt16Async();
}

public partial class PrimitiveImpl : IPrimitiveMark<uint>, ITypeMark<uint>, IAsyncTypeMark<uint>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void MatchType<M>(M matcher, uint _) where M : ITypeMatch => matcher.MatchUInt32();
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTask MatchTypeAsync<M>(M matcher, uint _) where M : IAsyncTypeMatch => matcher.MatchUInt32Async();
}

public partial class PrimitiveImpl : IPrimitiveMark<ulong>, ITypeMark<ulong>, IAsyncTypeMark<ulong>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void MatchType<M>(M matcher, ulong _) where M : ITypeMatch => matcher.MatchUInt64();
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTask MatchTypeAsync<M>(M matcher, ulong _) where M : IAsyncTypeMatch => matcher.MatchUInt64Async();
}

public partial class PrimitiveImpl : IPrimitiveMark<UInt128>, ITypeMark<UInt128>, IAsyncTypeMark<UInt128>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void MatchType<M>(M matcher, UInt128 _) where M : ITypeMatch => matcher.MatchUInt128();
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTask MatchTypeAsync<M>(M matcher, UInt128 _) where M : IAsyncTypeMatch => matcher.MatchUInt128Async();
}

public partial class PrimitiveImpl : IPrimitiveMark<nint>, ITypeMark<nint>, IAsyncTypeMark<nint>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void MatchType<M>(M matcher, nint _) where M : ITypeMatch => matcher.MatchIntPtr();
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTask MatchTypeAsync<M>(M matcher, nint _) where M : IAsyncTypeMatch => matcher.MatchIntPtrAsync();
}

public partial class PrimitiveImpl : IPrimitiveMark<nuint>, ITypeMark<nuint>, IAsyncTypeMark<nuint>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void MatchType<M>(M matcher, nuint _) where M : ITypeMatch => matcher.MatchUIntPtr();
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTask MatchTypeAsync<M>(M matcher, nuint _) where M : IAsyncTypeMatch => matcher.MatchUIntPtrAsync();
}

public partial class PrimitiveImpl : IPrimitiveMark<Half>, ITypeMark<Half>, IAsyncTypeMark<Half>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void MatchType<M>(M matcher, Half _) where M : ITypeMatch => matcher.MatchHalf();
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTask MatchTypeAsync<M>(M matcher, Half _) where M : IAsyncTypeMatch => matcher.MatchHalfAsync();
}

public partial class PrimitiveImpl : IPrimitiveMark<float>, ITypeMark<float>, IAsyncTypeMark<float>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void MatchType<M>(M matcher, float _) where M : ITypeMatch => matcher.MatchSingle();
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTask MatchTypeAsync<M>(M matcher, float _) where M : IAsyncTypeMatch => matcher.MatchSingleAsync();
}

public partial class PrimitiveImpl : IPrimitiveMark<double>, ITypeMark<double>, IAsyncTypeMark<double>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void MatchType<M>(M matcher, double _) where M : ITypeMatch => matcher.MatchDouble();
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTask MatchTypeAsync<M>(M matcher, double _) where M : IAsyncTypeMatch => matcher.MatchDoubleAsync();
}

public partial class PrimitiveImpl : IPrimitiveMark<decimal>, ITypeMark<decimal>, IAsyncTypeMark<decimal>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void MatchType<M>(M matcher, decimal _) where M : ITypeMatch => matcher.MatchDecimal();
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTask MatchTypeAsync<M>(M matcher, decimal _) where M : IAsyncTypeMatch => matcher.MatchDecimalAsync();
}

public partial class PrimitiveImpl : IPrimitiveMark<BigInteger>, ITypeMark<BigInteger>, IAsyncTypeMark<BigInteger>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void MatchType<M>(M matcher, BigInteger _) where M : ITypeMatch => matcher.MatchBigInteger();
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTask MatchTypeAsync<M>(M matcher, BigInteger _) where M : IAsyncTypeMatch => matcher.MatchBigIntegerAsync();
}

public partial class PrimitiveImpl : IPrimitiveMark<Complex>, ITypeMark<Complex>, IAsyncTypeMark<Complex>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void MatchType<M>(M matcher, Complex _) where M : ITypeMatch => matcher.MatchComplex();
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTask MatchTypeAsync<M>(M matcher, Complex _) where M : IAsyncTypeMatch => matcher.MatchComplexAsync();
}

public partial class PrimitiveImpl : IPrimitiveMark<DateOnly>, ITypeMark<DateOnly>, IAsyncTypeMark<DateOnly>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void MatchType<M>(M matcher, DateOnly _) where M : ITypeMatch => matcher.MatchDateOnly();
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTask MatchTypeAsync<M>(M matcher, DateOnly _) where M : IAsyncTypeMatch => matcher.MatchDateOnlyAsync();
}

public partial class PrimitiveImpl : IPrimitiveMark<DateTime>, ITypeMark<DateTime>, IAsyncTypeMark<DateTime>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void MatchType<M>(M matcher, DateTime _) where M : ITypeMatch => matcher.MatchDateTime();
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTask MatchTypeAsync<M>(M matcher, DateTime _) where M : IAsyncTypeMatch => matcher.MatchDateTimeAsync();
}

public partial class PrimitiveImpl : IPrimitiveMark<DateTimeOffset>, ITypeMark<DateTimeOffset>, IAsyncTypeMark<DateTimeOffset>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void MatchType<M>(M matcher, DateTimeOffset _) where M : ITypeMatch => matcher.MatchDateTimeOffset();
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTask MatchTypeAsync<M>(M matcher, DateTimeOffset _) where M : IAsyncTypeMatch => matcher.MatchDateTimeOffsetAsync();
}

public partial class PrimitiveImpl : IPrimitiveMark<Guid>, ITypeMark<Guid>, IAsyncTypeMark<Guid>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void MatchType<M>(M matcher, Guid _) where M : ITypeMatch => matcher.MatchGuid();
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTask MatchTypeAsync<M>(M matcher, Guid _) where M : IAsyncTypeMatch => matcher.MatchGuidAsync();
}

public partial class PrimitiveImpl : IPrimitiveMark<Range>, ITypeMark<Range>, IAsyncTypeMark<Range>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void MatchType<M>(M matcher, Range _) where M : ITypeMatch => matcher.MatchRange();
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTask MatchTypeAsync<M>(M matcher, Range _) where M : IAsyncTypeMatch => matcher.MatchRangeAsync();
}

public partial class PrimitiveImpl : IPrimitiveMark<Index>, ITypeMark<Index>, IAsyncTypeMark<Index>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void MatchType<M>(M matcher, Index _) where M : ITypeMatch => matcher.MatchIndex();
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTask MatchTypeAsync<M>(M matcher, Index _) where M : IAsyncTypeMatch => matcher.MatchIndexAsync();
}

public partial class PrimitiveImpl : IPrimitiveMark<char>, ITypeMark<char>, IAsyncTypeMark<char>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void MatchType<M>(M matcher, char _) where M : ITypeMatch => matcher.MatchChar();
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTask MatchTypeAsync<M>(M matcher, char _) where M : IAsyncTypeMatch => matcher.MatchCharAsync();
}

public partial class PrimitiveImpl : IPrimitiveMark<Rune>, ITypeMark<Rune>, IAsyncTypeMark<Rune>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void MatchType<M>(M matcher, Rune _) where M : ITypeMatch => matcher.MatchRune();
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTask MatchTypeAsync<M>(M matcher, Rune _) where M : IAsyncTypeMatch => matcher.MatchRuneAsync();
}

public partial class PrimitiveImpl : IPrimitiveMark<string>, ITypeMark<string>, IAsyncTypeMark<string>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void MatchType<M>(M matcher, string _) where M : ITypeMatch => matcher.MatchString();
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTask MatchTypeAsync<M>(M matcher, string _) where M : IAsyncTypeMatch => matcher.MatchStringAsync();
}
