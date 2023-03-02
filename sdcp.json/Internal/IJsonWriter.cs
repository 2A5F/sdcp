using SDcp.Json.Utils;
using System;
using System.Buffers;
using System.Globalization;
using System.Text;
using SDcp.Exceptions;

namespace SDcp.Json.Internal;

// ReSharper disable once TypeParameterCanBeVariant
public interface IJsonWriter<TFormatter> where TFormatter : AJsonFormatter
{
    public TFormatter Formatter { get; }

    public void WriteShortString(ReadOnlySpan<char> str);

    public void WriteString(ReadOnlySpan<char> str, bool quote = false, bool escape = false)
    {
        if (quote) WriteShortString("\"");
        if (escape)
        {
            WriteEscape(str);
        }
        else
        {
            WriteStringPart(str);
        }
        if (quote) WriteShortString("\"");
    }
    public void WriteStringPart(ReadOnlySpan<char> str);

    public void WriteEscape(ReadOnlySpan<char> str)
    {
        for (var n = 0; ;)
        {
            var span = str[n..];
            if (span.IsEmpty)
            {
                if (n > 0) WriteStringPart(str[..n]);
                return;
            }
            var r = Rune.DecodeFromUtf16(span, out var rune, out var len);
            if (r != OperationStatus.Done) throw new FormatException();
            if (rune.IsAscii && EscapeTable.TryGet(span[0], out var esc))
            {
                if (n > 0) WriteStringPart(str[..n]);
                WriteStringPart("\\");
                WriteStringPart(new ReadOnlySpan<char>(in esc));
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
                if (n > 0) WriteStringPart(str[..n]);
                WriteEscapeHex(rune);
                str = str[(n + len)..];
                n = 0;
            }
            else
            {
                n += len;
            }
        }
    }

    public void WriteEscapeHex(Rune rune)
    {
        Span<char> span = stackalloc char[6];
        span[0] = '\\';
        span[1] = 'u';
        var hex = span[2..];
        var r = rune.Value.TryFormat(hex, out var len, "X4");
        if (!r || len != 4) throw new ArgumentOutOfRangeException($"{rune}");
        WriteStringPart(span);
    }

    public void WriteFormat<T>(int init_len, T value, bool quote = false, bool escape = false) where T : ISpanFormattable
    {
        var format = Formatter.GetNumberFormat<T>();
        var provider = Formatter.GetFormatProvider<T>();
        if (init_len is 0 or > 8) goto try_large;
        Span<char> span = stackalloc char[init_len];
        var r = value.TryFormat(span, out var len, format, provider);
        if (!r)
        {
            init_len = 512;
            goto try_large;
        }
        WriteString(span[..len], quote: quote, escape: escape);
        return;
    try_large:
        WriteFormatLarge(init_len, value, format, provider, quote: quote, escape: escape);
    }

    private void WriteFormatLarge<T>(int init_len, T value, ReadOnlySpan<char> format, IFormatProvider? provider, bool quote = false, bool escape = false) where T : ISpanFormattable
    {
        if (init_len <= 0) init_len = 256;
        for (; ; )
        {
            var buf = ArrayPool<char>.Shared.Rent(init_len);
            try
            {
                var r = value.TryFormat(buf.AsSpan(), out var len, format, provider);
                if (!r) goto try_more_large;
                WriteString(buf.AsSpan(0, len), quote: quote, escape: escape);
            }
            finally
            {
                ArrayPool<char>.Shared.Return(buf);
            }
            return;
        try_more_large:;
            if (init_len >= 524_288) throw new NumberFormatException<T>(value, format, provider);
            init_len *= 2;
            if (init_len < 128) init_len = 128;
        }
    }

    public void WriteUtf8(ReadOnlySpan<byte> buf, bool quote = false);

}
