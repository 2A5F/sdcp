using System;
using System.Text;

namespace SDcp.Json;

public abstract class AJsonFormatter
{
    public Encoding Encoding { get; set; } = Encoding.UTF8;
    public virtual IFormatProvider? GetFormatProvider<T>() => null;
    public virtual ReadOnlySpan<char> GetNumberFormat<T>() => default;
    public bool LargeNumberUseString { get; set; } = true;
    public bool DecimalUseString { get; set; } = true;
    public bool EnumUseString { get; set; } = true;
    public bool Base64Bytes { get; set; } = false;
    public bool EscapeAllNonAsciiChar { get; set; } = false;
}

public class CompactJsonFormatter : AJsonFormatter
{
    public static readonly CompactJsonFormatter Default = new();
}
