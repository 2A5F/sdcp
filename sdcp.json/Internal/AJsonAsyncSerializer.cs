using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDcp.Json.Internal;

public abstract partial class AJsonAsyncSerializer<TSelf, TWriter, TFormatter> : AJsonSerializerBase, IAsyncSerializer
    where TWriter : IJsonAsyncWriter<TFormatter> where TFormatter : AJsonFormatter
    where TSelf : AJsonAsyncSerializer<TSelf, TWriter, TFormatter>
{
    protected AJsonAsyncSerializer(TWriter writer)
    {
        Writer = writer;
    }

    public TWriter Writer;
    public TFormatter Formatter => Writer.Formatter;

    protected abstract TSelf Self { get; }

    public bool IsHumanReadable => true;

    public ValueTask WriteShortString(ReadOnlyMemory<char> str) => Writer.WriteStringPart(str);
    public ValueTask WriteShortString(string str) => Writer.WriteStringPart(str);

    public ValueTask WriteString(ReadOnlyMemory<char> str, bool quote = false, bool escape = false) =>
        Writer.WriteString(str, quote: quote, escape: escape);

    public ValueTask WriteString(string str, bool quote = false, bool escape = false) =>
        Writer.WriteString(str.AsMemory(), quote: quote, escape: escape);

    public ValueTask WriteFormat<T>(int init_len, T value, bool quote = false, bool escape = false)
        where T : ISpanFormattable
        => Writer.WriteFormat<T>(init_len, value, quote: quote, escape: escape);
}
