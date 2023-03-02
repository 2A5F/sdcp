using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SDcp.Misc;

namespace SDcp.Runtime;

internal static class DynamicSerialize<S> where S : ISerializer
{
    private static readonly ConcurrentDictionary<Type, Action<S, object>> ser = new();

    public static Action<S, object> GetDynamicImpl(Type type) => ser.GetOrAdd(type, static type =>
    {
        if (type == typeof(object)) return ObjectImpl.Instance.Serialize;
        var rt = typeof(RuntimeImpl<>).MakeGenericType(type);
        var instance = Expression.Property(null, rt.GetProperty("Instance")!.GetMethod!);
        var m = rt.GetMethod("Serialize")!.MakeGenericMethod(typeof(S));
        var p1 = Expression.Parameter(typeof(S));
        var p2 = Expression.Parameter(type);
        var call = Expression.Call(instance, m, p1, p2);
        var f = Expression.Lambda<Action<S, object>>(call, p1, p2);
        return f.Compile();
    });
}

internal static class DynamicAsyncSerialize<S> where S : IAsyncSerializer
{
    private static readonly ConcurrentDictionary<Type, Func<S, object, ValueTask>> ser = new();

    public static Func<S, object, ValueTask> GetDynamicAsyncImpl(Type type) => ser.GetOrAdd(type, static type =>
    {
        if (type == typeof(object)) return ObjectImpl.Instance.SerializeAsync;
        var rt = typeof(RuntimeImpl<>).MakeGenericType(type);
        var instance = Expression.Property(null, rt.GetProperty("Instance")!.GetMethod!);
        var m = rt.GetMethod("SerializeAsync")!.MakeGenericMethod(typeof(S));
        var p1 = Expression.Parameter(typeof(S));
        var p2 = Expression.Parameter(type);
        var call = Expression.Call(instance, m, p1, p2);
        var f = Expression.Lambda<Func<S, object, ValueTask>>(call, p1, p2);
        return f.Compile();
    });
}
