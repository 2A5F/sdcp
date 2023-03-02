using System.IO;

namespace SDcp.Json.Internal;

public class StreamJsonSerializer<TStream, TFormatter> : AJsonSerializer<StreamJsonSerializer<TStream, TFormatter>, StreamJsonWriter<TStream, TFormatter>, TFormatter>
    where TStream : Stream where TFormatter : AJsonFormatter
{
    public TStream OutputStream => Writer.OutputStream;

    public StreamJsonSerializer(TStream outputStream, TFormatter formatter) : base(new StreamJsonWriter<TStream, TFormatter>(outputStream, formatter)) {}

    protected override StreamJsonSerializer<TStream, TFormatter> Self => this;
}
