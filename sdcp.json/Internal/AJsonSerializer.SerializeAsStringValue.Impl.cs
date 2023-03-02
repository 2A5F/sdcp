using System;
using System.Numerics;
using System.Text;

namespace SDcp.Json.Internal;

public abstract partial class AJsonSerializer<TSelf, TWriter, TFormatter>
{
    private readonly partial record struct SerializeAsStringValue
    {
        public void SerializeBool(bool value) => WriteShortString(value ? "true" : "false");
        public void SerializeSByte(sbyte value) => WriteFormat(4, value, escape: true);
        public void SerializeInt16(short value) => WriteFormat(8, value, escape: true);
        public void SerializeInt32(int value) => WriteFormat(16, value, escape: true);
        public void SerializeInt64(long value) => WriteFormat(24, value, escape: true);
        public void SerializeInt128(Int128 value) => WriteFormat(48, value, escape: true);
        public void SerializeByte(byte value) => WriteFormat(4, value, escape: true);
        public void SerializeUInt16(ushort value) => WriteFormat(8, value, escape: true);
        public void SerializeUInt32(uint value) => WriteFormat(16, value, escape: true);
        public void SerializeUInt64(ulong value) => WriteFormat(24, value, escape: true);
        public void SerializeUInt128(UInt128 value) => WriteFormat(48, value, escape: true);
        public void SerializeIntPtr(nint value) => WriteFormat(24, value, escape: true);
        public void SerializeUIntPtr(nuint value) => WriteFormat(24, value, escape: true);
        public void SerializeHalf(Half value) => WriteFormat(32, value, escape: true);
        public void SerializeSingle(float value) => WriteFormat(32, value, escape: true);
        public void SerializeDouble(double value) => WriteFormat(32, value, escape: true);
        public void SerializeDecimal(decimal value) => WriteFormat(32, value, escape: true);
        public void SerializeBigInteger(BigInteger value) => WriteFormat(128, value, escape: true);
        public void SerializeComplex(Complex value) => WriteFormat(0, value, escape: true);
        public void SerializeDateOnly(DateOnly value) => WriteFormat(16, value, escape: true);
        public void SerializeDateTime(DateTime value) => WriteFormat(16, value, escape: true);
        public void SerializeDateTimeOffset(DateTimeOffset value) => WriteFormat(16, value, escape: true);
        public void SerializeGuid(Guid value) => WriteFormat(36, value, escape: true);
        public void SerializeChar(char value) => WriteFormat(8, value, escape: true);
        public void SerializeRune(Rune value) => WriteFormat(16, value, escape: true);
        public void SerializeString(ReadOnlySpan<char> value)
        {
            WriteShortString("\\\"");
            WriteString(value, escape: true);
            WriteShortString("\\\"");
        }
        public void SerializeNull() => WriteShortString("null");
        public void SerializeNullableNotNull<T, S>(in T value, S serialize) where S : ISerialize<T> => serialize.Serialize(this, in value);
    }
}
