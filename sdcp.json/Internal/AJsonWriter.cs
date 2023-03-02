using System;
using System.Text;

namespace SDcp.Json.Internal;

public abstract class AJsonWriter<TFormatter> : IJsonWriter<TFormatter> where TFormatter : AJsonFormatter
{
    protected AJsonWriter(TFormatter formatter)
    {
        Formatter = formatter;
    }

    public TFormatter Formatter { get; set; }

    public Encoding Encoding => Formatter.Encoding;

    public abstract void WriteShortString(ReadOnlySpan<char> str);
    public abstract void WriteStringPart(ReadOnlySpan<char> str);
    public abstract void WriteUtf8(ReadOnlySpan<byte> buf, bool quote = false);
}
