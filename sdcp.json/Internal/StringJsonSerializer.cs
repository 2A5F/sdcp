using System.Text;

namespace SDcp.Json.Internal;

public class StringJsonSerializer<TFormatter> : AJsonSerializer<StringJsonSerializer<TFormatter>, StringJsonWriter<TFormatter>, TFormatter>
    where TFormatter : AJsonFormatter
{
    public StringBuilder Builder => Writer.Builder;

    public StringJsonSerializer(StringBuilder builder, TFormatter formatter) : base(new StringJsonWriter<TFormatter>(builder, formatter)) {}

    protected override StringJsonSerializer<TFormatter> Self => this;
}
