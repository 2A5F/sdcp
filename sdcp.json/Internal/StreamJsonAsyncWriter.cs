using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDcp.Json.Internal;

public class StreamJsonAsyncWriter<TStream, TFormatter> : AJsonAsyncWriter<TFormatter> where TStream : Stream where TFormatter : AJsonFormatter
{
    public TStream OutputStream;

    public StreamJsonAsyncWriter(TStream outputStream, TFormatter formatter) : base(formatter)
    {
        OutputStream = outputStream;
    }

    public override async ValueTask WriteStringPart(ReadOnlyMemory<char> str)
    {
        var len = Encoding.GetByteCount(str.Span);
        var buf = ArrayPool<byte>.Shared.Rent(len);
        try
        {
            Encoding.GetBytes(str.Span, buf.AsSpan(0, len));
            await OutputStream.WriteAsync(buf.AsMemory(0, len));
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buf);
        }
    }

    public override async ValueTask WriteStringPart(char str)
    {
        var len = Encoding.GetByteCount(new ReadOnlySpan<char>(in str));
        var buf = ArrayPool<byte>.Shared.Rent(len);
        try
        {
            Encoding.GetBytes(new ReadOnlySpan<char>(in str), buf.AsSpan(0, len));
            await OutputStream.WriteAsync(buf.AsMemory(0, len));
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buf);
        }
    }

    public override async ValueTask WriteUtf8(ReadOnlyMemory<byte> buf, bool quote = false)
    {
        if (EqualityComparer<Encoding>.Default.Equals(Encoding, Encoding.UTF8))
        {
            if (quote) await WriteStringPart("\"");
            await OutputStream.WriteAsync(buf);
            if (quote) await WriteStringPart("\"");
        }
        else
        {
            var char_count = Encoding.UTF8.GetCharCount(buf.Span);
            var chars = ArrayPool<char>.Shared.Rent(char_count);
            try
            {
                Encoding.UTF8.GetChars(buf.Span, chars);
                await ((IJsonAsyncWriter<TFormatter>)this).WriteString(chars.AsMemory(0, char_count), quote: true);
            }
            finally
            {
                ArrayPool<char>.Shared.Return(chars);
            }
        }
    }
}
