using System;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SDcp;

public interface ISerializerMeta
{
    public bool IsHumanReadable { get; }
}

public interface ISerializer : ISerializerMeta
{
    void SerializeBool(bool value) => throw new NotImplementedException();
    void SerializeSByte(sbyte value) => throw new NotImplementedException();
    void SerializeInt16(short value) => throw new NotImplementedException();
    void SerializeInt32(int value) => throw new NotImplementedException();
    void SerializeInt64(long value) => throw new NotImplementedException();
    void SerializeInt128(Int128 value) => throw new NotImplementedException();
    void SerializeByte(byte value) => throw new NotImplementedException();
    void SerializeUInt16(ushort value) => throw new NotImplementedException();
    void SerializeUInt32(uint value) => throw new NotImplementedException();
    void SerializeUInt64(ulong value) => throw new NotImplementedException();
    void SerializeUInt128(UInt128 value) => throw new NotImplementedException();
    void SerializeIntPtr(nint value) => throw new NotImplementedException();
    void SerializeUIntPtr(nuint value) => throw new NotImplementedException();
    void SerializeHalf(Half value) => throw new NotImplementedException();
    void SerializeSingle(float value) => throw new NotImplementedException();
    void SerializeDouble(double value) => throw new NotImplementedException();
    void SerializeDecimal(decimal value) => throw new NotImplementedException();
    void SerializeBigInteger(BigInteger value) => throw new NotImplementedException();
    void SerializeComplex(Complex value) => throw new NotImplementedException();
    void SerializeDateOnly(DateOnly value) => throw new NotImplementedException();
    void SerializeDateTime(DateTime value) => throw new NotImplementedException();
    void SerializeDateTimeOffset(DateTimeOffset value) => throw new NotImplementedException();
    void SerializeGuid(Guid value) => throw new NotImplementedException();
    void SerializeRange(Range value) => throw new NotImplementedException();
    void SerializeIndex(Index value) => throw new NotImplementedException();
    void SerializeChar(char value) => throw new NotImplementedException();
    void SerializeRune(Rune value) => throw new NotImplementedException();
    void SerializeString(string value) => SerializeString(value.AsSpan());
    void SerializeString(ReadOnlyMemory<char> value) => SerializeString(value.Span);
    void SerializeString(ReadOnlySpan<char> value) => throw new NotImplementedException();
    void SerializeBytes(byte[] value) => SerializeBytes(value.AsSpan());
    void SerializeBytes(ReadOnlyMemory<byte> value) => SerializeBytes(value.Span);
    void SerializeBytes(ReadOnlySpan<byte> value) => throw new NotImplementedException();
    void SerializeNull() => throw new NotImplementedException();
    void SerializeNullableNotNull<T, S>(in T value, S serialize) where S : ISerialize<T> => throw new NotImplementedException();
    void SerializeNullableNotNull<T, S>(T value, S serialize) where S : ISerialize<T> => SerializeNullableNotNull(in value, serialize);
    void SerializeEnum<E>(E e) where E : Enum => throw new NotImplementedException();
    void SerializeEnum<E, T, S>(E e, in T int_value, S serialize) where E : Enum where S : ISerialize<T> => throw new NotImplementedException();
    void SerializeEnum<E, T, S>(E e, T int_value, S serialize) where E : Enum where S : ISerialize<T> => SerializeEnum(e, in int_value, serialize);
    void SerializeEnum<E, T, S>(E e, string? name, in T int_value, S serialize) where E : Enum where S : ISerialize<T> => throw new NotImplementedException();
    void SerializeEnum<E, T, S>(E e, string? name, T int_value, S serialize) where E : Enum where S : ISerialize<T> => SerializeEnum(e, name, in int_value, serialize);

    #region Enumerable
    void EnumerableStart(nuint? len) => throw new NotImplementedException();
    void EnumerableStart<T, M>(nuint? len, M mark) where M : ITypeMark<T> => EnumerableStart(len);
    void EnumerableSerializeElement<T, S>(in T value, S serialize) where S : ISerialize<T> => throw new NotImplementedException();
    void EnumerableSerializeElement<T, S>(T value, S serialize) where S : ISerialize<T> => EnumerableSerializeElement(in value, serialize);
    void EnumerableEnd() => throw new NotImplementedException();
    #endregion

    #region Tuple
    void TupleStart(nuint? len) => throw new NotImplementedException();
    void TupleSerializeElement<T, S>(in T value, S serialize) where S : ISerialize<T> => throw new NotImplementedException();
    void TupleSerializeElement<T, S>(T value, S serialize) where S : ISerialize<T> => TupleSerializeElement(in value, serialize);
    void TupleEnd() => throw new NotImplementedException();
    #endregion

    #region Dictionary
    void DictionaryStart(nuint? len) => throw new NotImplementedException();
    void DictionaryStart<K, KM>(nuint? len, KM _mark) where KM : ITypeMark<K> => DictionaryStart(len);
    void DictionaryStart<K, V, KM, VM>(nuint? len, KM k_mark, VM v_mark) where KM : ITypeMark<K> where VM : ITypeMark<V> => DictionaryStart<K, KM>(len, k_mark);
    void DictionarySerializeKey<T, S>(in T key, S serialize) where S : ISerialize<T> => throw new NotImplementedException();
    void DictionarySerializeKey<T, S>(T key, S serialize) where S : ISerialize<T> => DictionarySerializeKey(in key, serialize);
    void DictionarySerializeValue<T, S>(in T value, S serialize) where S : ISerialize<T> => throw new NotImplementedException();
    void DictionarySerializeValue<T, S>(T value, S serialize) where S : ISerialize<T> => DictionarySerializeValue(in value, serialize);
    void DictionarySerializeEntry<K, V, KS, VS>(in K key, in V value, KS k_serialize, VS v_serialize) where KS : ISerialize<K> where VS : ISerialize<V>
    {
        DictionarySerializeKey(key, k_serialize);
        DictionarySerializeValue(value, v_serialize);
    }
    void DictionarySerializeEntry<K, V, KS, VS>(K key, V value, KS k_serialize, VS v_serialize)
        where KS : ISerialize<K> where VS : ISerialize<V> =>
        DictionarySerializeEntry(in key, in value, k_serialize, v_serialize);
    void DictionaryEnd() => throw new NotImplementedException();
    #endregion

    #region Struct
    void StructStart(string? name, nuint? len) => throw new NotImplementedException();
    void StructStart<S, M>(string? name, nuint? len, M mark) where M : ITypeMark<S> => StructStart(name, len);
    void StructSerializeField<T, S>(string key, T value, S serialize) where S : ISerialize<T> => StructSerializeField(key, in value, serialize);
    void StructSerializeField<T, S>(string key, in T value, S serialize) where S : ISerialize<T> => throw new NotImplementedException();
    void StructSkipField(string key) => throw new NotImplementedException();
    void StructEnd() => throw new NotImplementedException();
    #endregion
}

public interface IAsyncSerializer : ISerializerMeta
{
    ValueTask SerializeBoolAsync(bool value) => throw new NotImplementedException();
    ValueTask SerializeSByteAsync(sbyte value) => throw new NotImplementedException();
    ValueTask SerializeInt16Async(short value) => throw new NotImplementedException();
    ValueTask SerializeInt32Async(int value) => throw new NotImplementedException();
    ValueTask SerializeInt64Async(long value) => throw new NotImplementedException();
    ValueTask SerializeInt128Async(Int128 value) => throw new NotImplementedException();
    ValueTask SerializeByteAsync(byte value) => throw new NotImplementedException();
    ValueTask SerializeUInt16Async(ushort value) => throw new NotImplementedException();
    ValueTask SerializeUInt32Async(uint value) => throw new NotImplementedException();
    ValueTask SerializeUInt64Async(ulong value) => throw new NotImplementedException();
    ValueTask SerializeUInt128Async(UInt128 value) => throw new NotImplementedException();
    ValueTask SerializeIntPtrAsync(nint value) => throw new NotImplementedException();
    ValueTask SerializeUIntPtrAsync(nuint value) => throw new NotImplementedException();
    ValueTask SerializeHalfAsync(Half value) => throw new NotImplementedException();
    ValueTask SerializeSingleAsync(float value) => throw new NotImplementedException();
    ValueTask SerializeDoubleAsync(double value) => throw new NotImplementedException();
    ValueTask SerializeDecimalAsync(decimal value) => throw new NotImplementedException();
    ValueTask SerializeBigIntegerAsync(BigInteger value) => throw new NotImplementedException();
    ValueTask SerializeComplexAsync(Complex value) => throw new NotImplementedException();
    ValueTask SerializeDateOnlyAsync(DateOnly value) => throw new NotImplementedException();
    ValueTask SerializeDateTimeAsync(DateTime value) => throw new NotImplementedException();
    ValueTask SerializeDateTimeOffsetAsync(DateTimeOffset value) => throw new NotImplementedException();
    ValueTask SerializeGuidAsync(Guid value) => throw new NotImplementedException();
    ValueTask SerializeRangeAsync(Range value) => throw new NotImplementedException();
    ValueTask SerializeIndexAsync(Index value) => throw new NotImplementedException();
    ValueTask SerializeCharAsync(char value) => throw new NotImplementedException();
    ValueTask SerializeRuneAsync(Rune value) => throw new NotImplementedException();
    ValueTask SerializeStringAsync(string value) => SerializeStringAsync(value.AsMemory());
    ValueTask SerializeStringAsync(ReadOnlyMemory<char> value) => throw new NotImplementedException();
    ValueTask SerializeBytesAsync(byte[] value) => SerializeBytesAsync(value.AsMemory());
    ValueTask SerializeBytesAsync(ReadOnlyMemory<byte> value) => throw new NotImplementedException();
    ValueTask SerializeNullAsync() => throw new NotImplementedException();
    ValueTask SerializeNullableNotNullAsync<T, S>(T value, S serialize) where S : IAsyncSerialize<T> => throw new NotImplementedException();
    ValueTask SerializeEnumAsync<E>(E e) where E : Enum => throw new NotImplementedException();
    ValueTask SerializeEnumAsync<E, T, S>(E e, T int_value, S serialize) where E : Enum where S : IAsyncSerialize<T> => throw new NotImplementedException();
    ValueTask SerializeEnumAsync<E, T, S>(E e, string? name, T int_value, S serialize) where E : Enum where S : IAsyncSerialize<T> => throw new NotImplementedException();

    #region Enumerable
    ValueTask EnumerableStartAsync(nuint? len) => throw new NotImplementedException();
    ValueTask EnumerableStartAsync<T, M>(nuint? len, M mark) where M : IAsyncTypeMark<T> => EnumerableStartAsync(len);
    ValueTask EnumerableSerializeElementAsync<T, S>(T value, S serialize) where S : IAsyncSerialize<T> => throw new NotImplementedException();
    ValueTask EnumerableEndAsync() => throw new NotImplementedException();
    #endregion

    #region Tuple
    ValueTask TupleStartAsync(nuint? len) => throw new NotImplementedException();
    ValueTask TupleSerializeElementAsync<T, S>(T value, S serialize) where S : IAsyncSerialize<T> => throw new NotImplementedException();
    ValueTask TupleEndAsync() => throw new NotImplementedException();
    #endregion

    #region Dictionary
    ValueTask DictionaryStartAsync(nuint? len) => throw new NotImplementedException();
    ValueTask DictionaryStartAsync<K, KM>(nuint? len, KM k_mark) where KM : IAsyncTypeMark<K> => DictionaryStartAsync(len);
    ValueTask DictionaryStartAsync<K, V, KM, VM>(nuint? len, KM k_mark, VM v_mark) where KM : IAsyncTypeMark<K> where VM : IAsyncTypeMark<V> => DictionaryStartAsync<K, KM>(len, k_mark);
    ValueTask DictionarySerializeKeyAsync<T, S>(T key, S serialize) where S : IAsyncSerialize<T> => throw new NotImplementedException();
    ValueTask DictionarySerializeValueAsync<T, S>(T value, S serialize) where S : IAsyncSerialize<T> => throw new NotImplementedException();
    async ValueTask DictionarySerializeEntryAsync<K, V, KS, VS>(K key, V value, KS k_serialize, VS v_serialize) where KS : IAsyncSerialize<K> where VS : IAsyncSerialize<V>
    {
        await DictionarySerializeKeyAsync(key, k_serialize);
        await DictionarySerializeValueAsync(value, v_serialize);
    }
    ValueTask DictionaryEndAsync() => throw new NotImplementedException();
    #endregion

    #region Struct
    ValueTask StructStartAsync(string? name, nuint? len) => throw new NotImplementedException();
    ValueTask StructStartAsync<S, M>(string? name, nuint? len, M mark) where M : IAsyncTypeMark<S> => StructStartAsync(name, len);
    ValueTask StructSerializeFieldAsync<T, S>(string key, T value, S serialize) where S : IAsyncSerialize<T> => throw new NotImplementedException();
    ValueTask StructSkipFieldAsync<T>(string key) => throw new NotImplementedException();
    ValueTask StructEndAsync() => throw new NotImplementedException();
    #endregion
}
