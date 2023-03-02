using System;
using System.Threading.Tasks;

namespace SDcp;

public interface ITypeMatch
{
    void MatchDefault() {}
    void MatchBool() => MatchDefault();
    void MatchSByte() => MatchDefault();
    void MatchInt16() => MatchDefault();
    void MatchInt32() => MatchDefault();
    void MatchInt64() => MatchDefault();
    void MatchInt128() => MatchDefault();
    void MatchByte() => MatchDefault();
    void MatchUInt16() => MatchDefault();
    void MatchUInt32() => MatchDefault();
    void MatchUInt64() => MatchDefault();
    void MatchUInt128() => MatchDefault();
    void MatchIntPtr() => MatchDefault();
    void MatchUIntPtr() => MatchDefault();
    void MatchHalf() => MatchDefault();
    void MatchSingle() => MatchDefault();
    void MatchDouble() => MatchDefault();
    void MatchDecimal() => MatchDefault();
    void MatchBigInteger() => MatchDefault();
    void MatchComplex() => MatchDefault();
    void MatchDateOnly() => MatchDefault();
    void MatchDateTime() => MatchDefault();
    void MatchDateTimeOffset() => MatchDefault();
    void MatchGuid() => MatchDefault();
    void MatchRange() => MatchDefault();
    void MatchIndex() => MatchDefault();
    void MatchChar() => MatchDefault();
    void MatchRune() => MatchDefault();
    void MatchString() => MatchDefault();
    void MatchNullable<T, M>(M mark) where M : ITypeMark<T> => MatchDefault();
    void MatchEnum<E>() where E : Enum => MatchDefault();
    void MatchEnumerable<T, M>(M mark) where M : ITypeMark<T> => MatchDefault();
    void MatchDictionary<K, KM>(KM k_mark) where KM : ITypeMark<K> => MatchDefault();
    void MatchDictionary<K, V, KM, VM>(KM k_mark, VM v_mark) where KM : ITypeMark<K> where VM : ITypeMark<V> => MatchDefault();
}

public interface IAsyncTypeMatch
{
    ValueTask MatchDefaultAsync() => ValueTask.CompletedTask;
    ValueTask MatchBoolAsync() => MatchDefaultAsync();
    ValueTask MatchSByteAsync() => MatchDefaultAsync();
    ValueTask MatchInt16Async() => MatchDefaultAsync();
    ValueTask MatchInt32Async() => MatchDefaultAsync();
    ValueTask MatchInt64Async() => MatchDefaultAsync();
    ValueTask MatchInt128Async() => MatchDefaultAsync();
    ValueTask MatchByteAsync() => MatchDefaultAsync();
    ValueTask MatchUInt16Async() => MatchDefaultAsync();
    ValueTask MatchUInt32Async() => MatchDefaultAsync();
    ValueTask MatchUInt64Async() => MatchDefaultAsync();
    ValueTask MatchUInt128Async() => MatchDefaultAsync();
    ValueTask MatchIntPtrAsync() => MatchDefaultAsync();
    ValueTask MatchUIntPtrAsync() => MatchDefaultAsync();
    ValueTask MatchHalfAsync() => MatchDefaultAsync();
    ValueTask MatchSingleAsync() => MatchDefaultAsync();
    ValueTask MatchDoubleAsync() => MatchDefaultAsync();
    ValueTask MatchDecimalAsync() => MatchDefaultAsync();
    ValueTask MatchBigIntegerAsync() => MatchDefaultAsync();
    ValueTask MatchComplexAsync() => MatchDefaultAsync();
    ValueTask MatchDateOnlyAsync() => MatchDefaultAsync();
    ValueTask MatchDateTimeAsync() => MatchDefaultAsync();
    ValueTask MatchDateTimeOffsetAsync() => MatchDefaultAsync();
    ValueTask MatchGuidAsync() => MatchDefaultAsync();
    ValueTask MatchRangeAsync() => MatchDefaultAsync();
    ValueTask MatchIndexAsync() => MatchDefaultAsync();
    ValueTask MatchCharAsync() => MatchDefaultAsync();
    ValueTask MatchRuneAsync() => MatchDefaultAsync();
    ValueTask MatchStringAsync() => MatchDefaultAsync();
    ValueTask MatchNullableAsync<T, M>(M mark) where M : IAsyncTypeMark<T> => MatchDefaultAsync();
    ValueTask MatchEnumAsync<E>() where E : Enum => MatchDefaultAsync();
    ValueTask MatchEnumerableAsync<T, M>(M mark) where M : IAsyncTypeMark<T> => MatchDefaultAsync();
    ValueTask MatchDictionaryAsync<K, KM>(KM k_mark) where KM : IAsyncTypeMark<K> => MatchDefaultAsync();
    ValueTask MatchDictionaryAsync<K, V, KM, VM>(KM k_mark, VM v_mark) where KM : IAsyncTypeMark<K> where VM : IAsyncTypeMark<V> => MatchDefaultAsync();
}
