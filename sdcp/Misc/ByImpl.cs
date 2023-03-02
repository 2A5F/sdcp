using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SDcp.Misc;

public class ByImpl<T, S> : ISerialize<T> where T : IGetSerialize<T, S> where S : ISerialize<T>
{
    public static ByImpl<T, S> Instance { get; } = new();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Serialize<S1>(S1 serializer, in T value) where S1 : ISerializer
        => value.GetSerialize().Serialize(serializer, in value);
}

public class AsyncByImpl<T, S> : IAsyncSerialize<T> where T : IGetAsyncSerialize<T, S> where S : IAsyncSerialize<T>
{
    public static AsyncByImpl<T, S> Instance { get; } = new();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public async ValueTask SerializeAsync<S1>(S1 serializer, T value) where S1 : IAsyncSerializer
        => await (await value.GetAsyncSerialize()).SerializeAsync(serializer, value);
}
