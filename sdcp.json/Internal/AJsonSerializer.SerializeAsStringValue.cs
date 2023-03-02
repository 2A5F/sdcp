using System;

namespace SDcp.Json.Internal;

public abstract partial class AJsonSerializer<TSelf, TWriter, TFormatter>
{
    private readonly partial record struct SerializeAsStringValue(TSelf Self) : ISerializer
    {
        public bool IsHumanReadable => true;

        private void WriteShortString(ReadOnlySpan<char> str) => Self.Writer.WriteShortString(str);

        private void WriteString(ReadOnlySpan<char> str, bool quote = false, bool escape = false) =>
            Self.Writer.WriteString(str, quote: quote, escape: escape);

        private void WriteFormat<T>(int init_len, T value, bool quote = false, bool escape = false)
            where T : ISpanFormattable
            => Self.Writer.WriteFormat<T>(init_len, value, quote: quote, escape: escape);
    }
}
