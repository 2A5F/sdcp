using System.IO;
using System.Text;
using System.Threading.Tasks;
using SDcp.Json.Internal;

namespace SDcp.Json;

public static class SDJson
{
    public static BSerializer Serializer => default;

    public struct BSerializer
    {
        public BToStream<Stream, CompactJsonFormatter> ToStream(Stream stream)
            => ToStream<Stream>(stream);
        public BToStream<TStream, CompactJsonFormatter> ToStream<TStream>(TStream stream) where TStream : Stream
            => new(stream, CompactJsonFormatter.Default);

        public new BToString<CompactJsonFormatter> ToString() => new(CompactJsonFormatter.Default);

        public BToStringBuilder<CompactJsonFormatter> ToStringBuilder(StringBuilder builder) => new(builder, CompactJsonFormatter.Default);

        public record struct BToStream<TStream, TFormatter>(TStream stream, TFormatter formatter) where TStream : Stream where TFormatter : AJsonFormatter
        {
            public BToStream<TStream, TNewFormatter> UseFormatter<TNewFormatter>(TNewFormatter formatter) where TNewFormatter : AJsonFormatter
                => new(stream, formatter);

            #region Sync

            public void Serialize<TValue, TSerialize>(in TValue value) where TSerialize : ISerialize<TValue> where TValue : IGetSerialize<TValue, TSerialize>
                => value.GetSerialize().Serialize(new StreamJsonSerializer<TStream, TFormatter>(stream, formatter), in value);
            public void Serialize<TValue, TSerialize>(TValue value) where TSerialize : ISerialize<TValue> where TValue : IGetSerialize<TValue, TSerialize>
                => Serialize<TValue, TSerialize>(in value);

            public void Serialize<TValue, TSerialize>(in TValue value, TSerialize serialize) where TSerialize : ISerialize<TValue>
                => serialize.Serialize(new StreamJsonSerializer<TStream, TFormatter>(stream, formatter), in value);
            public void Serialize<TValue, TSerialize>(TValue value, TSerialize serialize) where TSerialize : ISerialize<TValue>
                => Serialize(in value, serialize);

            #endregion

            #region Async

            public async ValueTask SerializeAsync<TValue, TSerialize>(TValue value) where TSerialize : IAsyncSerialize<TValue> where TValue : IGetAsyncSerialize<TValue, TSerialize>
                => await (await value.GetAsyncSerialize()).SerializeAsync(new StreamJsonAsyncSerializer<TStream, TFormatter>(stream, formatter), value);

            public ValueTask SerializeAsync<TValue, TSerialize>(TValue value, TSerialize serialize) where TSerialize : IAsyncSerialize<TValue>
                => serialize.SerializeAsync(new StreamJsonAsyncSerializer<TStream, TFormatter>(stream, formatter), value);

            #endregion
        }

        public record struct BToString<TFormatter>(TFormatter formatter) where TFormatter : AJsonFormatter
        {
            public string Serialize<TValue, TSerialize>(TValue value) where TSerialize : ISerialize<TValue> where TValue : IGetSerialize<TValue, TSerialize>
                => Serialize<TValue, TSerialize>(in value);
            public string Serialize<TValue, TSerialize>(in TValue value) where TSerialize : ISerialize<TValue>
                where TValue : IGetSerialize<TValue, TSerialize>
            {
                var ser = new StringJsonSerializer<TFormatter>(new StringBuilder(), formatter);
                value.GetSerialize().Serialize(ser, in value);
                return ser.Builder.ToString();
            }

            public string Serialize<TValue, TSerialize>(TValue value, TSerialize serialize) where TSerialize : ISerialize<TValue>
                => Serialize(in value, serialize);
            public string Serialize<TValue, TSerialize>(in TValue value, TSerialize serialize) where TSerialize : ISerialize<TValue>
            {
                var ser = new StringJsonSerializer<TFormatter>(new StringBuilder(), formatter);
                serialize.Serialize(ser, in value);
                return ser.Builder.ToString();
            }
        }

        public record struct BToStringBuilder<TFormatter>(StringBuilder builder,  TFormatter formatter) where TFormatter : AJsonFormatter
        {
            public void Serialize<TValue, TSerialize>(TValue value) where TSerialize : ISerialize<TValue> where TValue : IGetSerialize<TValue, TSerialize>
                => Serialize<TValue, TSerialize>(in value);
            public void Serialize<TValue, TSerialize>(in TValue value) where TSerialize : ISerialize<TValue>
                where TValue : IGetSerialize<TValue, TSerialize>
            {
                var ser = new StringJsonSerializer<TFormatter>(builder, formatter);
                value.GetSerialize().Serialize(ser, in value);
            }

            public void Serialize<TValue, TSerialize>(TValue value, TSerialize serialize) where TSerialize : ISerialize<TValue>
                => Serialize(in value, serialize);
            public void Serialize<TValue, TSerialize>(in TValue value, TSerialize serialize) where TSerialize : ISerialize<TValue>
            {
                var ser = new StringJsonSerializer<TFormatter>(builder, formatter);
                serialize.Serialize(ser, in value);
            }
        }
    }
}
