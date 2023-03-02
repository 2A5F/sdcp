using System.IO;

namespace SDcp.Json.Internal;

public class StreamJsonAsyncSerializer<TStream, TFormatter> : AJsonAsyncSerializer<StreamJsonAsyncSerializer<TStream, TFormatter>, StreamJsonAsyncWriter<TStream, TFormatter>, TFormatter>
    where TStream : Stream where TFormatter : AJsonFormatter
{
    public TStream OutputStream => Writer.OutputStream;

    public StreamJsonAsyncSerializer(TStream outputStream, TFormatter formatter) : base(new StreamJsonAsyncWriter<TStream, TFormatter>(outputStream, formatter)) { }

    protected override StreamJsonAsyncSerializer<TStream, TFormatter> Self => this;
}

