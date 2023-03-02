using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDcp.Json.Internal;

public abstract class AJsonAsyncWriter<TFormatter> : IJsonAsyncWriter<TFormatter> where TFormatter : AJsonFormatter
{
    protected AJsonAsyncWriter(TFormatter formatter)
    {
        Formatter = formatter;
    }

    public TFormatter Formatter { get; set; }

    public Encoding Encoding => Formatter.Encoding;
    
    public abstract ValueTask WriteStringPart(ReadOnlyMemory<char> str);
    public virtual ValueTask WriteStringPart(string str) => WriteStringPart(str.AsMemory());
    public abstract ValueTask WriteStringPart(char str);
    public abstract ValueTask WriteUtf8(ReadOnlyMemory<byte> buf, bool quote = false);
}
