using System;
using System.Threading.Tasks;

namespace SDcp.Json.Internal;

public abstract partial class AJsonAsyncSerializer<TSelf, TWriter, TFormatter>
{
    private readonly partial record struct SerializeAsStringValue(TSelf Self) : IAsyncSerializer
    {
        public bool IsHumanReadable => true;

        private ValueTask WriteShortString(ReadOnlyMemory<char> str) => Self.Writer.WriteStringPart(str);
        private ValueTask WriteShortString(string str) => Self.Writer.WriteStringPart(str);

        private ValueTask WriteString(ReadOnlyMemory<char> str, bool quote = false, bool escape = false) =>
            Self.Writer.WriteString(str, quote: quote, escape: escape);

        private ValueTask WriteString(string str, bool quote = false, bool escape = false) =>
            Self.Writer.WriteString(str.AsMemory(), quote: quote, escape: escape);

        private ValueTask WriteFormat<T>(int init_len, T value, bool quote = false, bool escape = false)
            where T : ISpanFormattable
            => Self.Writer.WriteFormat<T>(init_len, value, quote: quote, escape: escape);
    }
}
