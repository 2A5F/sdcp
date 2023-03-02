using SDcp.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SDcp.Misc;

public class IdentityImpl<T> : ISerialize<T> where T : ISerialize<T>
{
    public static IdentityImpl<T> Instance { get; } = new();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Serialize<S>(S serializer, in T value) where S : ISerializer 
        => value.Serialize(serializer, in value);
}

public class AsyncIdentityImpl<T> : IAsyncSerialize<T> where T : IAsyncSerialize<T>
{
    public static AsyncIdentityImpl<T> Instance { get; } = new();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTask SerializeAsync<S>(S serializer, T value) where S : IAsyncSerializer 
        => value.SerializeAsync(serializer, value);
}
