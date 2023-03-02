using System.Collections;
using System.Runtime.CompilerServices;
using SDcp.Collections;

namespace SDcp.Runtime.Internal;

public class DynamicCollectionImpl<C> : ISerialize<C> where C : ICollection
{
    public static DynamicCollectionImpl<C> Instance { get; } = new();

    private static readonly CollectionImpl<C, DynamicImpl> Impl = new(DynamicImpl.Instance);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Serialize<S>(S serializer, in C value) where S : ISerializer
        => Impl.Serialize(serializer, in value);
}
