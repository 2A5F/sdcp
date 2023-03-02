using System.IO;
using System.Threading.Tasks;
using SDcp.Runtime;

namespace SDcp.Json.Runtime;

public static class SDJsonRuntime
{
    public static void Serialize<TStream, TFormatter, TValue>(this SDJson.BSerializer.BToStream<TStream, TFormatter> self, TValue value)
        where TStream : Stream where TFormatter : AJsonFormatter
        => self.Serialize(in value);
    public static void Serialize<TStream, TFormatter, TValue>(this SDJson.BSerializer.BToStream<TStream, TFormatter> self, in TValue value)
        where TStream : Stream where TFormatter : AJsonFormatter
        => self.Serialize(value, RuntimeImpl<TValue>.Instance);

    public static ValueTask SerializeAsync<TStream, TFormatter, TValue>(this SDJson.BSerializer.BToStream<TStream, TFormatter> self, in TValue value)
        where TStream : Stream where TFormatter : AJsonFormatter
        => self.SerializeAsync(value, RuntimeImpl<TValue>.Instance);

    public static string Serialize<TFormatter, TValue>(this SDJson.BSerializer.BToString<TFormatter> self, TValue value)
        where TFormatter : AJsonFormatter
        => self.Serialize(in value);
    public static string Serialize<TFormatter, TValue>(this SDJson.BSerializer.BToString<TFormatter> self, in TValue value)
        where TFormatter : AJsonFormatter
        => self.Serialize(value, RuntimeImpl<TValue>.Instance);

    public static void Serialize<TFormatter, TValue>(this SDJson.BSerializer.BToStringBuilder<TFormatter> self, TValue value)
        where TFormatter : AJsonFormatter
        => self.Serialize(in value);
    public static void Serialize<TFormatter, TValue>(this SDJson.BSerializer.BToStringBuilder<TFormatter> self, in TValue value)
        where TFormatter : AJsonFormatter
        => self.Serialize(value, RuntimeImpl<TValue>.Instance);
}
