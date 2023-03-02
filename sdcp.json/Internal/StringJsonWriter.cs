using System;
using System.Buffers;
using System.Text;

namespace SDcp.Json.Internal;

public class StringJsonWriter<TFormatter> : AJsonWriter<TFormatter> where TFormatter : AJsonFormatter
{
    public StringBuilder Builder;

    public StringJsonWriter(StringBuilder builder, TFormatter formatter) : base(formatter)
    {
        Builder = builder;
    }

    public override void WriteShortString(ReadOnlySpan<char> str)
    {
        Builder.Append(str);
    }

    public override void WriteStringPart(ReadOnlySpan<char> str)
    {
        Builder.Append(str);
    }

    public override void WriteUtf8(ReadOnlySpan<byte> buf, bool quote = false)
    {
        var char_count = Encoding.UTF8.GetCharCount(buf);
        var chars = ArrayPool<char>.Shared.Rent(char_count);
        try
        {
            Encoding.UTF8.GetChars(buf, chars);
            ((IJsonWriter<TFormatter>)this).WriteString(chars.AsSpan(0, char_count), quote: quote);
        }
        finally
        {
            ArrayPool<char>.Shared.Return(chars);
        }
    }
}
