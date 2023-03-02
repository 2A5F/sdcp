using SDcp.Primitives;
using System;
using System.Buffers.Text;
using System.Buffers;
using System.Diagnostics;
using System.Numerics;
using System.Text;

namespace SDcp.Json.Internal;

public abstract partial class AJsonSerializer<TSelf, TWriter, TFormatter>
{
    public void SerializeBool(bool value) => WriteShortString(value ? "true" : "false");
    public void SerializeSByte(sbyte value) => WriteFormat(4, value);
    public void SerializeInt16(short value) => WriteFormat(8, value);
    public void SerializeInt32(int value) => WriteFormat(16, value);
    public void SerializeInt64(long value) => WriteFormat(24, value, Formatter.LargeNumberUseString);
    public void SerializeInt128(Int128 value) => WriteFormat(48, value, Formatter.LargeNumberUseString);
    public void SerializeByte(byte value) => WriteFormat(4, value);
    public void SerializeUInt16(ushort value) => WriteFormat(8, value);
    public void SerializeUInt32(uint value) => WriteFormat(16, value);
    public void SerializeUInt64(ulong value) => WriteFormat(24, value, Formatter.LargeNumberUseString);
    public void SerializeUInt128(UInt128 value) => WriteFormat(48, value, Formatter.LargeNumberUseString);
    public void SerializeIntPtr(nint value) => WriteFormat(24, value, Formatter.LargeNumberUseString);
    public void SerializeUIntPtr(nuint value) => WriteFormat(24, value, Formatter.LargeNumberUseString);
    public void SerializeHalf(Half value) => WriteFormat(32, value);
    public void SerializeSingle(float value) => WriteFormat(32, value);
    public void SerializeDouble(double value) => WriteFormat(32, value);
    public void SerializeDecimal(decimal value) => WriteFormat(32, value, Formatter.DecimalUseString);
    public void SerializeBigInteger(BigInteger value) => WriteFormat(128, value, Formatter.LargeNumberUseString);
    public void SerializeComplex(Complex value) => WriteFormat(0, value, quote: true);
    public void SerializeDateOnly(DateOnly value) => WriteFormat(16, value, quote: true);
    public void SerializeDateTime(DateTime value) => WriteFormat(16, value, quote: true);
    public void SerializeDateTimeOffset(DateTimeOffset value) => WriteFormat(16, value, quote: true);
    public void SerializeGuid(Guid value) => WriteFormat(36, value, quote: true);
    public void SerializeRange(Range value) => WriteString(value.ToString(), quote: true);
    public void SerializeIndex(Index value) => WriteString(value.ToString(), quote: true);
    public void SerializeChar(char value) => WriteFormat(8, value, quote: true, escape: true);
    public void SerializeRune(Rune value) => WriteFormat(16, value, quote: true, escape: true);
    public void SerializeString(ReadOnlySpan<char> value) => WriteString(value, quote: true, escape: true);
    public void SerializeBytes(ReadOnlySpan<byte> value)
    {
        if (Formatter.Base64Bytes)
        {
            var init_len = Base64.GetMaxEncodedToUtf8Length(value.Length);
            var buf = ArrayPool<byte>.Shared.Rent(init_len);
            try
            {
                var r = Base64.EncodeToUtf8(value, buf, out _, out var len);
                Debug.Assert(r == OperationStatus.Done);
                Writer.WriteUtf8(buf.AsSpan(0, len), quote: true);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(buf);
            }
        }
        else
        {
            EnumerableStart((nuint)value.Length);
            foreach (var b in value)
            {
                SerializeByte(b);
            }
            EnumerableEnd();
        }
    }
    public void SerializeNull() => WriteShortString("null");
    public void SerializeNullableNotNull<T, S>(in T value, S serialize) where S : ISerialize<T> => serialize.Serialize(this, in value);
    public void SerializeEnum<E>(E e) where E : Enum
    {
        var type = Enum.GetUnderlyingType(typeof(E));
        if (type == typeof(int)) SerializeEnum(e, Convert.ToInt32(e), PrimitiveImpl.Instance);
        else if (type == typeof(uint)) SerializeEnum(e, Convert.ToUInt32(e), PrimitiveImpl.Instance);
        else if (type == typeof(sbyte)) SerializeEnum(e, Convert.ToSByte(e), PrimitiveImpl.Instance);
        else if (type == typeof(byte)) SerializeEnum(e, Convert.ToByte(e), PrimitiveImpl.Instance);
        else if (type == typeof(short)) SerializeEnum(e, Convert.ToInt16(e), PrimitiveImpl.Instance);
        else if (type == typeof(ushort)) SerializeEnum(e, Convert.ToUInt16(e), PrimitiveImpl.Instance);
        else if (type == typeof(long)) SerializeEnum(e, Convert.ToInt64(e), PrimitiveImpl.Instance);
        else if (type == typeof(ulong)) SerializeEnum(e, Convert.ToUInt64(e), PrimitiveImpl.Instance);
        else throw new ArgumentException($"Enum of type {type} are not supported");
    }
    public void SerializeEnum<E, T, S>(E e, in T int_value, S serialize) where E : Enum where S : ISerialize<T>
    {
        if (Formatter.EnumUseString) SerializeEnum(e, Enum.GetName(typeof(E), e), int_value, serialize);
        else serialize.Serialize(this, in int_value);
    }
    public void SerializeEnum<E, T, S>(E e, string? name, in T int_value, S serialize) where E : Enum where S : ISerialize<T>
    {
        if (Formatter.EnumUseString && !string.IsNullOrEmpty(name)) SerializeString(name);
        else serialize.Serialize(this, in int_value);
    }

    #region Enumerable

    public void EnumerableStart(nuint? len)
    {
        WriteShortString("[");
        state = State.None;
    }

    public void EnumerableSerializeElement<T, S>(in T value, S serialize) where S : ISerialize<T>
    {
        if (state == State.ArrayItem)
        {
            WriteShortString(",");
        }
        state = State.None;
        serialize.Serialize(this, in value);
        state = State.ArrayItem;
    }

    public void EnumerableEnd()
    {
        WriteShortString("]");
        state = State.None;
    }
    #endregion

    #region Tuple
    public void TupleStart(nuint? len) => EnumerableStart(len);
    public void TupleSerializeElement<T, S>(in T value, S serialize) where S : ISerialize<T> => EnumerableSerializeElement(value, serialize);
    public void TupleEnd() => EnumerableEnd();
    #endregion

    #region Dictionary

    public void DictionaryStart(nuint? len)
    {
        WriteShortString("[");
        objectKeyKind = ObjectKeyKind.Object;
        state = State.None;
    }
    public void DictionaryStart<K, KM>(nuint? len, KM k_mark) where KM : ITypeMark<K>
    {
        k_mark.MatchType(new DictionaryStartTypeMatch(Self));
        state = State.None;
    }
    private readonly record struct DictionaryStartTypeMatch(TSelf Self) : ITypeMatch
    {
        public void MatchDefault()
        {
            Self.WriteShortString("[");
            Self.objectKeyKind = ObjectKeyKind.Object;
        }

        private void OnStr()
        {
            Self.WriteShortString("{");
            Self.objectKeyKind = ObjectKeyKind.String;
        }

        public void MatchBool() => OnStr();
        public void MatchSByte() => OnStr();
        public void MatchInt16() => OnStr();
        public void MatchInt32() => OnStr();
        public void MatchInt64() => OnStr();
        public void MatchInt128() => OnStr();
        public void MatchByte() => OnStr();
        public void MatchUInt16() => OnStr();
        public void MatchUInt32() => OnStr();
        public void MatchUInt64() => OnStr();
        public void MatchUInt128() => OnStr();
        public void MatchIntPtr() => OnStr();
        public void MatchUIntPtr() => OnStr();
        public void MatchHalf() => OnStr();
        public void MatchSingle() => OnStr();
        public void MatchDouble() => OnStr();
        public void MatchDecimal() => OnStr();
        public void MatchBigInteger() => OnStr();
        public void MatchDateOnly() => OnStr();
        public void MatchDateTime() => OnStr();
        public void MatchDateTimeOffset() => OnStr();
        public void MatchGuid() => OnStr();
        public void MatchChar() => OnStr();
        public void MatchRune() => OnStr();
        public void MatchString() => OnStr();
        public void MatchNullable<T, M>(M mark) where M : ITypeMark<T> => mark.MatchType(this);
    }

    public void DictionarySerializeKey<T, S>(in T key, S serialize) where S : ISerialize<T>
    {
        if (state == State.ObjectValue)
        {
            WriteShortString(",");
        }
        state = State.None;
        if (objectKeyKind == ObjectKeyKind.Object)
        {
            TupleStart(2);
            TupleSerializeElement(key, serialize);
            WriteShortString(",");
        }
        else
        {
            WriteShortString("\"");
            serialize.Serialize(new SerializeAsStringValue(Self), in key);
            WriteShortString("\"");
            WriteShortString(":");
        }
        state = State.ObjectKey;
    }

    public void DictionarySerializeValue<T, S>(in T value, S serialize) where S : ISerialize<T>
    {
        if (objectKeyKind == ObjectKeyKind.Object)
        {
            TupleSerializeElement(value, serialize);
            TupleEnd();
        }
        else
        {
            serialize.Serialize(this, in value);
        }
        state = State.ObjectValue;
    }

    public void DictionaryEnd()
    {
        if (objectKeyKind == ObjectKeyKind.Object)
        {
            WriteShortString("]");
        }
        else
        {
            WriteShortString("}");
        }
        state = State.None;
    }
    #endregion

    #region Struct

    public void StructStart(string? name, nuint? len)
    {
        WriteShortString("{");
        objectKeyKind = ObjectKeyKind.String;
        state = State.None;
    }

    public void StructSerializeField<T, S>(string key, in T value, S serialize) where S : ISerialize<T>
    {
        if (state == State.ObjectValue)
        {
            WriteShortString(",");
        }
        state = State.None;
        WriteString(key, quote: true, escape: true);
        WriteShortString(":");
        serialize.Serialize(this, in value);
        state = State.ObjectValue;
    }
    public void StructSkipField(string key) { }

    public void StructEnd()
    {
        WriteShortString("}");
        state = State.None;
    }
    #endregion
}
