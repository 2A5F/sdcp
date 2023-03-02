using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace SDcp.Misc;

public class ObjectImpl : ISerialize<object>, IAsyncSerialize<object>
{
    public static ObjectImpl Instance { get; } = new();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Serialize<S>(S serializer, in object value) where S : ISerializer
    {
        serializer.StructStart("Object", 0);
        serializer.StructEnd();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Serialize<S>(S serializer, object value) where S : ISerializer => Serialize(serializer, in value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public async ValueTask SerializeAsync<S>(S serializer, object value) where S : IAsyncSerializer
    {
        await serializer.StructStartAsync("Object", 0);
        await serializer.StructEndAsync();
    }
}
