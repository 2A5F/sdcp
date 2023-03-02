using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using SDcp.Primitives;

namespace SDcp.Runtime.Utils;

public abstract class TypeMeta
{
    public static readonly HashSet<Type> PrimitiveTypes;
    static TypeMeta()
    {
        PrimitiveTypes = typeof(PrimitiveImpl).GetInterfaces()
            .Where(t => t.GetGenericTypeDefinition() == typeof(IPrimitiveMark<>))
            .Select(t => t.GenericTypeArguments[0])
            .ToHashSet();
    }

    private static readonly ConcurrentDictionary<Type, TypeMeta> metas = new();

    public static TypeMeta GetMeta(Type type) => metas.GetOrAdd(type,
        static type => (TypeMeta)typeof(TypeMeta<>).MakeGenericType(type).GetProperty(nameof(TypeMeta<byte>.Instance))!.GetValue(null)!
    );

    public abstract bool IsPrimitive();
    public abstract bool IsISerialize();
    public abstract bool IsIGetSerialize();
    public abstract Type[]? GetIGetSerializeGeneric();
    public abstract bool IsIAsyncSerialize();
    public abstract bool IsIGetAsyncSerialize();
    public abstract Type[]? GetIsIGetAsyncSerializeGeneric();
    public abstract bool IsICollection();
    public abstract bool IsICollectionT();
    public abstract Type? GetICollectionTGeneric();

    public static bool IsPrimitive(Type type) => GetMeta(type).IsPrimitive();
    public static bool IsPrimitive<T>() => TypeMeta<T>.Instance.IsPrimitive();

    public static bool IsISerialize(Type type) => GetMeta(type).IsISerialize();
    public static bool IsISerialize<T>() => TypeMeta<T>.Instance.IsISerialize();

    public static bool IsIGetSerialize(Type type) => GetMeta(type).IsIGetSerialize();
    public static bool IsIGetSerialize<T>() => TypeMeta<T>.Instance.IsIGetSerialize();

    public static Type[]? GetIGetSerializeGeneric(Type type) => GetMeta(type).GetIGetSerializeGeneric();
    public static Type[]? GetIGetSerializeGeneric<T>() => TypeMeta<T>.Instance.GetIGetSerializeGeneric();

    public static bool IsIAsyncSerialize(Type type) => GetMeta(type).IsIAsyncSerialize();
    public static bool IsIAsyncSerialize<T>() => TypeMeta<T>.Instance.IsIAsyncSerialize();

    public static bool IsIGetAsyncSerialize(Type type) => GetMeta(type).IsIGetAsyncSerialize();
    public static bool IsIGetAsyncSerialize<T>() => TypeMeta<T>.Instance.IsIGetAsyncSerialize();

    public static Type[]? GetIsIGetAsyncSerializeGeneric(Type type) => GetMeta(type).GetIsIGetAsyncSerializeGeneric();
    public static Type[]? GetIsIGetAsyncSerializeGeneric<T>() => TypeMeta<T>.Instance.GetIsIGetAsyncSerializeGeneric();

    public static bool IsICollection(Type type) => GetMeta(type).IsICollection();
    public static bool IsICollection<T>() => TypeMeta<T>.Instance.IsICollection();

    public static bool IsICollectionT(Type type) => GetMeta(type).IsICollectionT();
    public static bool IsICollectionT<T>() => TypeMeta<T>.Instance.IsICollectionT();

    public static Type? GetICollectionTGeneric(Type type) => GetMeta(type).GetICollectionTGeneric();
    public static Type? GetICollectionTGeneric<T>() => TypeMeta<T>.Instance.GetICollectionTGeneric();

}
public class TypeMeta<T> : TypeMeta
{
    public static TypeMeta<T> Instance { get; } = new();

    public override bool IsPrimitive() => _IsPrimitive.Value;
    private static readonly Lazy<bool> _IsPrimitive =
        new(static () => PrimitiveTypes.Contains(typeof(T)));

    public override bool IsISerialize() => _IsISerialize.Value;
    private static readonly Lazy<bool> _IsISerialize =
        new(static () => typeof(T).GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ISerialize<>)));

    public override bool IsIGetSerialize() => _IsIGetSerialize.Value;
    private static readonly Lazy<bool> _IsIGetSerialize =
        new(static () => typeof(T).GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IGetSerialize<,>)));

    public override Type[]? GetIGetSerializeGeneric() => _GetIGetSerializeGeneric.Value;
    private static readonly Lazy<Type[]?> _GetIGetSerializeGeneric =
        new(static () => typeof(T).GetInterfaces()
            .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IGetSerialize<,>))
            .Select(i => i.GenericTypeArguments)
            .FirstOrDefault());

    public override bool IsIAsyncSerialize() => _IsIAsyncSerialize.Value;
    private static readonly Lazy<bool> _IsIAsyncSerialize =
        new(static () => typeof(T).GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IAsyncSerialize<>)));

    public override bool IsIGetAsyncSerialize() => _IsIGetAsyncSerialize.Value;
    private static readonly Lazy<bool> _IsIGetAsyncSerialize =
        new(static () => typeof(T).GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IGetAsyncSerialize<,>)));

    public override Type[]? GetIsIGetAsyncSerializeGeneric() => _GetIsIGetAsyncSerializeGeneric.Value;
    private static readonly Lazy<Type[]?> _GetIsIGetAsyncSerializeGeneric =
        new(static () => typeof(T).GetInterfaces()
            .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IGetAsyncSerialize<,>))
            .Select(i => i.GenericTypeArguments)
            .FirstOrDefault());

    public override bool IsICollection() => _IsICollection.Value;
    private static readonly Lazy<bool> _IsICollection =
        new(static () => typeof(ICollection).IsAssignableFrom(typeof(T)));

    public override bool IsICollectionT() => _IsICollectionT.Value;
    private static readonly Lazy<bool> _IsICollectionT =
        new(static () => typeof(T).GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICollection<>)));

    public override Type? GetICollectionTGeneric() => _GetICollectionTGeneric.Value;
    private static readonly Lazy<Type?> _GetICollectionTGeneric =
        new(static () => typeof(T).GetInterfaces()
            .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICollection<>))
            .Select(i => i.GenericTypeArguments)
            .FirstOrDefault()?[0]);
}
