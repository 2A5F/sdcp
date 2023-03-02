using SDcp.Exceptions;
using SDcp.Json.Utils;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDcp.Json.Internal;

// ReSharper disable once TypeParameterCanBeVariant
public interface IJsonAsyncWriter<TFormatter> where TFormatter : AJsonFormatter
{
    public TFormatter Formatter { get; }

    public async ValueTask WriteString(ReadOnlyMemory<char> str, bool quote = false, bool escape = false)
    {
        if (quote) await WriteStringPart("\"");
        if (escape)
        {
            await WriteEscape(str);
        }
        else
        {
            await WriteStringPart(str);
        }
        if (quote) await WriteStringPart("\"");
    }

    public ValueTask WriteStringPart(ReadOnlyMemory<char> str);
    public ValueTask WriteStringPart(string str);
    public ValueTask WriteStringPart(char str);

    public async ValueTask WriteEscape(ReadOnlyMemory<char> str)
    {
        for (var n = 0; ;)
        {
            var span = str[n..];
            if (span.IsEmpty)
            {
                if (n > 0) await WriteStringPart(str[..n]);
                return;
            }
            var r = Rune.DecodeFromUtf16(span.Span, out var rune, out var len);
            if (r != OperationStatus.Done) throw new FormatException();
            if (rune.IsAscii && EscapeTable.TryGet(span.Span[0], out var esc))
            {
                if (n > 0) await WriteStringPart(str[..n]);
                await WriteStringPart("\\");
                await WriteStringPart(esc);
                str = str[(n + len)..];
                n = 0;
            }
            else if (
                Formatter.EscapeAllNonAsciiChar && !rune.IsAscii ||
                Rune.GetUnicodeCategory(rune) is
                    UnicodeCategory.Control or
                    UnicodeCategory.Format or
                    UnicodeCategory.LineSeparator or
                    UnicodeCategory.OtherNotAssigned or
                    UnicodeCategory.PrivateUse
            )
            {
                if (n > 0) await WriteStringPart(str[..n]);
                await WriteEscapeHex(rune);
                str = str[(n + len)..];
                n = 0;
            }
            else
            {
                n += len;
            }
        }
    }

    public async ValueTask WriteEscapeHex(Rune rune)
    {
        var buf = ArrayPool<char>.Shared.Rent(6);

        static void ToEscapeHex(in Rune rune, Span<char> span)
        {
            span[0] = '\\';
            span[1] = 'u';
            var hex = span[2..];
            var r = rune.Value.TryFormat(hex, out var len, "X4");
            if (!r || len != 4) throw new ArgumentOutOfRangeException($"{rune}");
        }

        ToEscapeHex(rune, buf.AsSpan(0, 6));
        await WriteStringPart(buf.AsMemory(0, 6));
    }

    public async ValueTask WriteFormat<T>(int init_len, T value, bool quote = false, bool escape = false) where T : ISpanFormattable
    {
        var provider = Formatter.GetFormatProvider<T>();
        if (init_len <= 0) init_len = 256;
        for (; ; )
        {
            var buf = ArrayPool<char>.Shared.Rent(init_len);
            try
            {
                var r = value.TryFormat(buf.AsSpan(), out var len, Formatter.GetNumberFormat<T>(), provider);
                if (!r) goto try_more_large;
                await WriteString(buf.AsMemory(0, len), quote: quote, escape: escape);
            }
            finally
            {
                ArrayPool<char>.Shared.Return(buf);
            }
            return;
            try_more_large:;
            if (init_len >= 524_288) throw new NumberFormatException<T>(value, Formatter.GetNumberFormat<T>(), provider);
            init_len *= 2;
            if (init_len < 128) init_len = 128;
        }
    }

    public ValueTask WriteUtf8(ReadOnlyMemory<byte> buf, bool quote = false);

}
