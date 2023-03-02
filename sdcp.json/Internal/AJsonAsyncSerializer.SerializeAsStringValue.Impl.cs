using System.Numerics;
using System.Text;
using System;
using System.Threading.Tasks;

namespace SDcp.Json.Internal;

public abstract partial class AJsonAsyncSerializer<TSelf, TWriter, TFormatter>
{
    private readonly partial record struct SerializeAsStringValue
    {
        public ValueTask SerializeBoolAsync(bool value) => WriteShortString(value ? "true" : "false");
        public ValueTask SerializeSByteAsync(sbyte value) => WriteFormat(4, value, escape: true);
        public ValueTask SerializeInt16Async(short value) => WriteFormat(8, value, escape: true);
        public ValueTask SerializeInt32Async(int value) => WriteFormat(16, value, escape: true);
        public ValueTask SerializeInt64Async(long value) => WriteFormat(24, value, escape: true);
        public ValueTask SerializeInt128Async(Int128 value) => WriteFormat(48, value, escape: true);
        public ValueTask SerializeByteAsync(byte value) => WriteFormat(4, value, escape: true);
        public ValueTask SerializeUInt16Async(ushort value) => WriteFormat(8, value, escape: true);
        public ValueTask SerializeUInt32Async(uint value) => WriteFormat(16, value, escape: true);
        public ValueTask SerializeUInt64Async(ulong value) => WriteFormat(24, value, escape: true);
        public ValueTask SerializeUInt128Async(UInt128 value) => WriteFormat(48, value, escape: true);
        public ValueTask SerializeIntPtrAsync(nint value) => WriteFormat(24, value, escape: true);
        public ValueTask SerializeUIntPtrAsync(nuint value) => WriteFormat(24, value, escape: true);
        public ValueTask SerializeHalfAsync(Half value) => WriteFormat(32, value, escape: true);
        public ValueTask SerializeSingleAsync(float value) => WriteFormat(32, value, escape: true);
        public ValueTask SerializeDoubleAsync(double value) => WriteFormat(32, value, escape: true);
        public ValueTask SerializeDecimalAsync(decimal value) => WriteFormat(32, value, escape: true);
        public ValueTask SerializeBigIntegerAsync(BigInteger value) => WriteFormat(128, value, escape: true);
        public ValueTask SerializeComplexAsync(Complex value) => WriteFormat(0, value, escape: true);
        public ValueTask SerializeDateOnlyAsync(DateOnly value) => WriteFormat(16, value, escape: true);
        public ValueTask SerializeDateTimeAsync(DateTime value) => WriteFormat(16, value, escape: true);
        public ValueTask SerializeDateTimeOffsetAsync(DateTimeOffset value) => WriteFormat(16, value, escape: true);
        public ValueTask SerializeGuidAsync(Guid value) => WriteFormat(36, value, escape: true);
        public ValueTask SerializeCharAsync(char value) => WriteFormat(8, value, escape: true);
        public ValueTask SerializeRuneAsync(Rune value) => WriteFormat(16, value, escape: true);
        public async ValueTask SerializeStringAsync(ReadOnlyMemory<char> value)
        {
            await WriteShortString("\\\"");
            await WriteString(value, escape: true);
            await WriteShortString("\\\"");
        }
        public ValueTask SerializeNullAsync() => WriteShortString("null");
        public ValueTask SerializeNullableNotNullAsync<T, S>(T value, S serialize) where S : IAsyncSerialize<T> => serialize.SerializeAsync(this, value);
    }
}
