using System;

namespace SDcp.Json.Internal;

public abstract partial class AJsonSerializer<TSelf, TWriter, TFormatter> : AJsonSerializerBase, ISerializer
    where TWriter : IJsonWriter<TFormatter> where TFormatter : AJsonFormatter
    where TSelf : AJsonSerializer<TSelf, TWriter, TFormatter>
{
    protected AJsonSerializer(TWriter writer)
    {
        Writer = writer;
    }

    public TWriter Writer;
    public TFormatter Formatter => Writer.Formatter;

    protected abstract TSelf Self { get; }

    public bool IsHumanReadable => true;

    public void WriteShortString(ReadOnlySpan<char> str) => Writer.WriteShortString(str);

    public void WriteString(ReadOnlySpan<char> str, bool quote = false, bool escape = false) =>
        Writer.WriteString(str, quote: quote, escape: escape);

    public void WriteFormat<T>(int init_len, T value, bool quote = false, bool escape = false)
        where T : ISpanFormattable
    => Writer.WriteFormat<T>(init_len, value, quote: quote, escape: escape);

}
