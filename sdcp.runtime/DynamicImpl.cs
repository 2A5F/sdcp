using System.Threading.Tasks;
using SDcp.Misc;

namespace SDcp.Runtime;

public class DynamicImpl : ISerialize<object?>, IAsyncSerialize<object?>
{
    public static DynamicImpl Instance { get; } = new();

    public void Serialize<S>(S serializer, in object? value) where S : ISerializer
    {
        if (value == null)
        {
            serializer.SerializeNull();
            return;
        }

        var type = value.GetType();
        if (type == typeof(object)) ObjectImpl.Instance.Serialize(serializer, in value);
        else DynamicSerialize<S>.GetDynamicImpl(type)(serializer, value);
    }

    public async ValueTask SerializeAsync<S>(S serializer, object? value) where S : IAsyncSerializer
    {
        if (value == null)
        {
            await serializer.SerializeNullAsync();
            return;
        }

        var type = value.GetType();
        if (type == typeof(object)) await ObjectImpl.Instance.SerializeAsync(serializer, value);
        else await DynamicAsyncSerialize<S>.GetDynamicAsyncImpl(type)(serializer, value);
    }
}
