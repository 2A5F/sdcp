using System;
using System.Buffers.Text;
using System.Buffers;
using System.Diagnostics;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using SDcp.Primitives;

namespace SDcp.Json.Internal;

public abstract partial class AJsonAsyncSerializer<TSelf, TWriter, TFormatter>
{
    public ValueTask SerializeBoolAsync(bool value) => WriteShortString(value ? "true" : "false");
    public ValueTask SerializeSByteAsync(sbyte value) => WriteFormat(4, value);
    public ValueTask SerializeInt16Async(short value) => WriteFormat(8, value);
    public ValueTask SerializeInt32Async(int value) => WriteFormat(16, value);
    public ValueTask SerializeInt64Async(long value) => WriteFormat(24, value, Formatter.LargeNumberUseString);
    public ValueTask SerializeInt128Async(Int128 value) => WriteFormat(48, value, Formatter.LargeNumberUseString);
    public ValueTask SerializeByteAsync(byte value) => WriteFormat(4, value);
    public ValueTask SerializeUInt16Async(ushort value) => WriteFormat(8, value);
    public ValueTask SerializeUInt32Async(uint value) => WriteFormat(16, value);
    public ValueTask SerializeUInt64Async(ulong value) => WriteFormat(24, value, Formatter.LargeNumberUseString);
    public ValueTask SerializeUInt128Async(UInt128 value) => WriteFormat(48, value, Formatter.LargeNumberUseString);
    public ValueTask SerializeIntPtrAsync(nint value) => WriteFormat(24, value, Formatter.LargeNumberUseString);
    public ValueTask SerializeUIntPtrAsync(nuint value) => WriteFormat(24, value, Formatter.LargeNumberUseString);
    public ValueTask SerializeHalfAsync(Half value) => WriteFormat(32, value);
    public ValueTask SerializeSingleAsync(float value) => WriteFormat(32, value);
    public ValueTask SerializeDoubleAsync(double value) => WriteFormat(32, value);
    public ValueTask SerializeDecimalAsync(decimal value) => WriteFormat(32, value, Formatter.DecimalUseString);
    public ValueTask SerializeBigIntegerAsync(BigInteger value) => WriteFormat(128, value, Formatter.LargeNumberUseString);
    public ValueTask SerializeComplexAsync(Complex value) => WriteFormat(0, value, quote: true);
    public ValueTask SerializeDateOnlyAsync(DateOnly value) => WriteFormat(16, value, quote: true);
    public ValueTask SerializeDateTimeAsync(DateTime value) => WriteFormat(16, value, quote: true);
    public ValueTask SerializeDateTimeOffsetAsync(DateTimeOffset value) => WriteFormat(16, value, quote: true);
    public ValueTask SerializeGuidAsync(Guid value) => WriteFormat(36, value, quote: true);
    public ValueTask SerializeRangeAsync(Range value) => WriteString(value.ToString(), quote: true);
    public ValueTask SerializeIndexAsync(Index value) => WriteString(value.ToString(), quote: true);
    public ValueTask SerializeCharAsync(char value) => WriteFormat(8, value, quote: true, escape: true);
    public ValueTask SerializeRuneAsync(Rune value) => WriteFormat(16, value, quote: true, escape: true);
    public ValueTask SerializeStringAsync(ReadOnlyMemory<char> value) => WriteString(value, quote: true, escape: true);

    public async ValueTask SerializeBytesAsync(ReadOnlyMemory<byte> value)
    {
        if (Formatter.Base64Bytes)
        {
            var init_len = Base64.GetMaxEncodedToUtf8Length(value.Length);
            var buf = ArrayPool<byte>.Shared.Rent(init_len);
            try
            {
                var r = Base64.EncodeToUtf8(value.Span, buf, out _, out var len);
                Debug.Assert(r == OperationStatus.Done);
                await Writer.WriteUtf8(buf.AsMemory(0, len), quote: true);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(buf);
            }
        }
        else
        {
            await EnumerableStartAsync((nuint)value.Length);
            for (var i = 0; i < value.Span.Length; i++)
            {
                await SerializeByteAsync(value.Span[i]);
            }
            await EnumerableEndAsync();
        }
    }
    public ValueTask SerializeNullAsync() => WriteShortString("null");
    public ValueTask SerializeNullableNotNullAsync<T, S>(T value, S serialize) where S : IAsyncSerialize<T> => serialize.SerializeAsync(this, value);

    public ValueTask SerializeEnumAsync<E>(E e) where E : Enum
    {
        var type = Enum.GetUnderlyingType(typeof(E));
        if (type == typeof(int)) return SerializeEnumAsync(e, Convert.ToInt32(e), PrimitiveImpl.Instance);
        else if (type == typeof(uint)) return SerializeEnumAsync(e, Convert.ToUInt32(e), PrimitiveImpl.Instance);
        else if (type == typeof(sbyte)) return SerializeEnumAsync(e, Convert.ToSByte(e), PrimitiveImpl.Instance);
        else if (type == typeof(byte)) return SerializeEnumAsync(e, Convert.ToByte(e), PrimitiveImpl.Instance);
        else if (type == typeof(short)) return SerializeEnumAsync(e, Convert.ToInt16(e), PrimitiveImpl.Instance);
        else if (type == typeof(ushort)) return SerializeEnumAsync(e, Convert.ToUInt16(e), PrimitiveImpl.Instance);
        else if (type == typeof(long)) return SerializeEnumAsync(e, Convert.ToInt64(e), PrimitiveImpl.Instance);
        else if (type == typeof(ulong)) return SerializeEnumAsync(e, Convert.ToUInt64(e), PrimitiveImpl.Instance);
        else throw new ArgumentException($"Enum of type {type} are not supported");
    }
    public ValueTask SerializeEnumAsync<E, T, S>(E e, T int_value, S serialize) where E : Enum where S : IAsyncSerialize<T>
        => Formatter.EnumUseString ? SerializeEnumAsync(e, Enum.GetName(typeof(E), e), int_value, serialize) : serialize.SerializeAsync(this, int_value);
    public ValueTask SerializeEnumAsync<E, T, S>(E e, string? name, T int_value, S serialize) where E : Enum where S : IAsyncSerialize<T> =>
        Formatter.EnumUseString && !string.IsNullOrEmpty(name) ? SerializeStringAsync(name.AsMemory()) : serialize.SerializeAsync(this, int_value);

    #region Enumerable
    public ValueTask EnumerableStartAsync(nuint? len)
    {
        state = State.None;
        return WriteShortString("[");
    }
    public async ValueTask EnumerableSerializeElementAsync<T, S>(T value, S serialize) where S : IAsyncSerialize<T>
    {
        if (state == State.ArrayItem)
        {
            await WriteShortString(",");
        }
        state = State.None;
        await serialize.SerializeAsync(this, value);
        state = State.ArrayItem;
    }
    public ValueTask EnumerableEndAsync()
    {
        state = State.None;
        return WriteShortString("]");
    }
    #endregion

    #region Tuple
    public ValueTask TupleStartAsync(nuint? len) => EnumerableStartAsync(len);
    public ValueTask TupleSerializeElementAsync<T, S>(T value, S serialize) where S : IAsyncSerialize<T> => EnumerableSerializeElementAsync(value, serialize);
    public ValueTask TupleEndAsync() => EnumerableEndAsync();
    #endregion

    #region Dictionary

    public ValueTask DictionaryStartAsync(nuint? len)
    {
        objectKeyKind = ObjectKeyKind.Object;
        state = State.None;
        return WriteShortString("[");
    }
    public ValueTask DictionaryStartAsync<K, KM>(nuint? len, KM k_mark) where KM : IAsyncTypeMark<K>
    {
        state = State.None;
        return k_mark.MatchTypeAsync(new DictionaryStartTypeMatch(Self));
    }

    private readonly record struct DictionaryStartTypeMatch(TSelf Self) : IAsyncTypeMatch
    {
        public ValueTask MatchDefaultAsync()
        {
            Self.objectKeyKind = ObjectKeyKind.Object;
            return Self.WriteShortString("[");
        }

        private ValueTask OnStr()
        {
            Self.objectKeyKind = ObjectKeyKind.String;
            return Self.WriteShortString("{");
        }

        public ValueTask MatchBoolAsync() => OnStr();
        public ValueTask MatchSByteAsync() => OnStr();
        public ValueTask MatchInt16Async() => OnStr();
        public ValueTask MatchInt32Async() => OnStr();
        public ValueTask MatchInt64Async() => OnStr();
        public ValueTask MatchInt128Async() => OnStr();
        public ValueTask MatchByteAsync() => OnStr();
        public ValueTask MatchUInt16Async() => OnStr();
        public ValueTask MatchUInt32Async() => OnStr();
        public ValueTask MatchUInt64Async() => OnStr();
        public ValueTask MatchUInt128Async() => OnStr();
        public ValueTask MatchIntPtrAsync() => OnStr();
        public ValueTask MatchUIntPtrAsync() => OnStr();
        public ValueTask MatchHalfAsync() => OnStr();
        public ValueTask MatchSingleAsync() => OnStr();
        public ValueTask MatchDoubleAsync() => OnStr();
        public ValueTask MatchDecimalAsync() => OnStr();
        public ValueTask MatchBigIntegerAsync() => OnStr();
        public ValueTask MatchDateOnlyAsync() => OnStr();
        public ValueTask MatchDateTimeAsync() => OnStr();
        public ValueTask MatchDateTimeOffsetAsync() => OnStr();
        public ValueTask MatchGuidAsync() => OnStr();
        public ValueTask MatchCharAsync() => OnStr();
        public ValueTask MatchRuneAsync() => OnStr();
        public ValueTask MatchStringAsync() => OnStr();
        public ValueTask MatchNullableAsync<T, M>(M mark) where M : IAsyncTypeMark<T> => mark.MatchTypeAsync(this);
    }

    public async ValueTask DictionarySerializeKeyAsync<T, S>(T key, S serialize) where S : IAsyncSerialize<T>
    {
        if (state == State.ObjectValue)
        {
            await WriteShortString(",");
        }
        state = State.None;
        if (objectKeyKind == ObjectKeyKind.Object)
        {
            await TupleStartAsync(2);
            await TupleSerializeElementAsync(key, serialize);
            await WriteShortString(",");
        }
        else
        {
            await WriteShortString("\"");
            await serialize.SerializeAsync(new SerializeAsStringValue(Self), key);
            await WriteShortString("\"");
            await WriteShortString(":");
        }
        state = State.ObjectKey;
    }
    public async ValueTask DictionarySerializeValueAsync<T, S>(T value, S serialize) where S : IAsyncSerialize<T>
    {
        if (objectKeyKind == ObjectKeyKind.Object)
        {
            await TupleSerializeElementAsync(value, serialize);
            await TupleEndAsync();
        }
        else
        {
            await serialize.SerializeAsync(this, value);
        }
        state = State.ObjectValue;
    }
    public async ValueTask DictionaryEndAsync()
    {
        if (objectKeyKind == ObjectKeyKind.Object)
        {
            await WriteShortString("]");
        }
        else
        {
            await WriteShortString("}");
        }
        state = State.None;
    }
    #endregion

    #region Struct
    public ValueTask StructStartAsync(string? name, nuint? len)
    {
        objectKeyKind = ObjectKeyKind.String;
        state = State.None;
        return WriteShortString("{");
    }
    public async ValueTask StructSerializeFieldAsync<T, S>(string key, T value, S serialize) where S : IAsyncSerialize<T>
    {
        if (state == State.ObjectValue)
        {
            await WriteShortString(",");
        }
        state = State.None;
        await WriteString(key, quote: true, escape: true);
        await WriteShortString(":");
        await serialize.SerializeAsync(this, value);
        state = State.ObjectValue;
    }
    public ValueTask StructSkipFieldAsync<T>(string key) => ValueTask.CompletedTask;
    public ValueTask StructEndAsync()
    {
        state = State.None;
        return WriteShortString("}");
    }
    #endregion
}
