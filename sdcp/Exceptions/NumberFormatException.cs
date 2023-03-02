using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDcp.Exceptions;

public class NumberFormatException<T> : Exception
{
    public T Value { get; set; }
    public IFormatProvider? Provider = null;

    public NumberFormatException(T value, ReadOnlySpan<char> format = default, IFormatProvider? provider = null) : 
        base($"Format {typeof(T)} {{ {value} }} Failed; Format: {format}; Provider: {provider}")
    {
        Value = value;
        Provider = provider;
    }
}
