using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace SDcp.Json.Internal;

public class StreamJsonWriter<TStream, TFormatter> : AJsonWriter<TFormatter> where TStream : Stream where TFormatter : AJsonFormatter
{
    public TStream OutputStream;

    public StreamJsonWriter(TStream outputStream, TFormatter formatter) : base(formatter)
    {
        OutputStream = outputStream;
    }

    public override void WriteShortString(ReadOnlySpan<char> str)
    {
        Debug.Assert(Encoding.GetByteCount(str) <= 8);
        Span<byte> span = stackalloc byte[8];
        var len = Encoding.GetBytes(str, span);
        OutputStream.Write(span[..len]);
    }

    public override void WriteStringPart(ReadOnlySpan<char> str)
    {
        var len = Encoding.GetByteCount(str);
        var buf = ArrayPool<byte>.Shared.Rent(len);
        try
        {
            Encoding.GetBytes(str, buf.AsSpan(0, len));
            OutputStream.Write(buf.AsSpan(0, len));
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buf);
        }
    }

    public override void WriteUtf8(ReadOnlySpan<byte> buf, bool quote = false)
    {
        if (EqualityComparer<Encoding>.Default.Equals(Encoding, Encoding.UTF8))
        {
            if (quote) WriteShortString("\"");
            OutputStream.Write(buf);
            if (quote) WriteShortString("\"");
        }
        else
        {
            var char_count = Encoding.UTF8.GetCharCount(buf);
            var chars = ArrayPool<char>.Shared.Rent(char_count);
            try
            {
                Encoding.UTF8.GetChars(buf, chars);
                ((IJsonWriter<TFormatter>)this).WriteString(chars.AsSpan(0, char_count), quote: true);
            }
            finally
            {
                ArrayPool<char>.Shared.Return(chars);
            }
        }
    }
}
