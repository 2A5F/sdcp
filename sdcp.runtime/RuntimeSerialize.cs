using System;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using SDcp.Primitives;
using SDcp.Misc;
using SDcp.Runtime.Internal;
using SDcp.Runtime.Utils;
using SDcp.Collections;

namespace SDcp.Runtime;

public partial class RuntimeImpl<T> : ISerialize<T>, IAsyncSerialize<T>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Serialize<S>(S serializer, in T value) where S : ISerializer
        => serialize.Value.Serialize(serializer, in value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTask SerializeAsync<S>(S serializer, T value) where S : IAsyncSerializer
        => asyncSerialize.Value.SerializeAsync(serializer, value);

    private readonly Lazy<ISerialize<T>> serialize = new(InitSerialize);
    private readonly Lazy<IAsyncSerialize<T>> asyncSerialize = new(InitAsyncSerialize);

    private static ISerialize<T> InitSerialize()
    {
        if (typeof(T) == typeof(object)) 
            return (ISerialize<T>)DynamicImpl.Instance;

        if (TypeMeta.IsPrimitive<T>())
            return (ISerialize<T>)PrimitiveImpl.Instance;

        if (TypeMeta.IsISerialize<T>())
            return (ISerialize<T>)typeof(IdentityImpl<>).MakeGenericType(typeof(T)).GetProperty("Instance")!.GetValue(null)!;

        if (TypeMeta.IsIGetSerialize<T>())
            return (ISerialize<T>)typeof(ByImpl<,>).MakeGenericType(TypeMeta.GetIGetSerializeGeneric<T>()!).GetProperty("Instance")!.GetValue(null)!;

        if (TypeMeta.IsICollection<T>())
            return (ISerialize<T>)typeof(DynamicCollectionImpl<>).MakeGenericType(typeof(T)).GetProperty("Instance")!.GetValue(null)!;

        // todo

        return RuntimeSerialize<T>.Instance;
    }

    private static IAsyncSerialize<T> InitAsyncSerialize()
    {
        if (typeof(T) == typeof(object)) 
            return (IAsyncSerialize<T>)DynamicImpl.Instance;

        if (TypeMeta.IsPrimitive<T>())
            return (IAsyncSerialize<T>)PrimitiveImpl.Instance;

        if (TypeMeta.IsIAsyncSerialize<T>())
            return (IAsyncSerialize<T>)typeof(AsyncIdentityImpl<>).MakeGenericType(typeof(T)).GetProperty("Instance")!.GetValue(null)!;

        if (TypeMeta.IsISerialize<T>())
            return (IAsyncSerialize<T>)typeof(AsyncByImpl<,>).MakeGenericType(TypeMeta.GetIsIGetAsyncSerializeGeneric<T>()!).GetProperty("Instance")!.GetValue(null)!;

        // todo

        return RuntimeAsyncSerialize<T>.Instance;
    }
}

internal static partial class RuntimeImpl
{

    public static readonly Lazy<(MethodInfo Val, MethodInfo In)> StructSerializeField = new(() =>
    {
        MethodInfo Val = null!;
        MethodInfo In = null!;
        foreach (
            var (m, isIn) in
                 from m in typeof(ISerializer).GetMethods()
                 where m.Name == nameof(ISerializer.StructSerializeField) && m.IsGenericMethodDefinition
                 let ps = m.GetParameters()
                 select (m, ps[1].IsIn)
            )
        {
            if (isIn) In = m;
            else Val = m;
        }
        return (Val, In);
    });
}

internal class RuntimeSerialize<T> : ISerialize<T>
{
    public static RuntimeSerialize<T> Instance { get; } = new();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Serialize<S>(S serializer, in T value) where S : ISerializer
    {
        RuntimeSerialize<S, T>.Delegate.Value(serializer, ref Unsafe.AsRef(value));
    }
}

internal class RuntimeAsyncSerialize<T> : IAsyncSerialize<T>
{
    public static RuntimeAsyncSerialize<T> Instance { get; } = new();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTask SerializeAsync<S>(S serializer, T value) where S : IAsyncSerializer
    {
        return RuntimeAsyncSerialize<S, T>.Delegate.Value(serializer, value);
    }
}

internal delegate void RuntimeSerializeDelegate<in S, T>(S serializer, ref T value) where S : ISerializer;

internal delegate ValueTask RuntimeAsyncSerializeDelegate<in S, in T>(S serializer, T value) where S : IAsyncSerializer;

internal static class RuntimeSerialize<S, T> where S : ISerializer
{
    public static Lazy<RuntimeSerializeDelegate<S, T>> Delegate = new(Init);

    private static RuntimeSerializeDelegate<S, T> Init()
    {
        var TS = typeof(S);
        var TT = typeof(T);
        var fn = new DynamicMethod($"RuntimeSerialize for {TT.GUID:N}", MethodAttributes.Public | MethodAttributes.Static,
            CallingConventions.Standard, typeof(void), new[] { TS, TT.MakeByRefType() }, TT.Module, true);
        var ilg = fn.GetILGenerator();

        GenStruct(ilg, TS, TT);
        //todo other type

        return (RuntimeSerializeDelegate<S, T>)fn.CreateDelegate(typeof(RuntimeSerializeDelegate<S, T>));
    }

    private static void GenStruct(ILGenerator ilg, Type TS, Type TT)
    {
        var members = TT
            .GetMembers(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.GetProperty)
            .Where(m => m is FieldInfo or PropertyInfo).ToArray();

        ilg.Emit(OpCodes.Ldarga, 0);
        ilg.Emit(OpCodes.Ldstr, TT.Name);
        ilg.Emit(OpCodes.Ldc_I4, members.Length);
        ilg.Emit(OpCodes.Conv_I);
        ilg.Emit(OpCodes.Newobj, typeof(nuint?).GetConstructor(new[] { typeof(nuint) })!);
        ilg.Emit(OpCodes.Constrained, TS);
        ilg.EmitCall(OpCodes.Callvirt,
            typeof(ISerializer).GetMethod(nameof(ISerializer.StructStart),
                new[] { typeof(string), typeof(nuint?) })!, null);

        var (StructSerializeField_v, StructSerializeField_in) = RuntimeImpl.StructSerializeField.Value;

        foreach (var m in members)
        {
            ilg.Emit(OpCodes.Ldarga, 0);
            ilg.Emit(OpCodes.Ldstr, m.Name);
            ilg.Emit(OpCodes.Ldarg_1);
            if (m is FieldInfo f)
            {
                if (!TT.IsValueType) ilg.Emit(OpCodes.Ldind_Ref);
                ilg.Emit(OpCodes.Ldflda, f);
                GenSubItem(ilg, TS, f.FieldType, t => StructSerializeField_in.MakeGenericMethod(f.FieldType, t));
            }
            else if (m is PropertyInfo p)
            {
                ilg.Emit(OpCodes.Constrained, TT);
                ilg.EmitCall(OpCodes.Callvirt, p.GetMethod!, null);
                GenSubItem(ilg, TS, p.PropertyType, t => StructSerializeField_v.MakeGenericMethod(p.PropertyType, t));
            }
            else throw new NotImplementedException("never");
        }

        ilg.Emit(OpCodes.Ldarga, 0);
        ilg.Emit(OpCodes.Constrained, TS);
        ilg.EmitCall(OpCodes.Callvirt, typeof(ISerializer).GetMethod(nameof(ISerializer.StructEnd), Array.Empty<Type>())!, null);
        ilg.Emit(OpCodes.Ret);
    }

    private static void GenSubItem(ILGenerator ilg, Type TS, Type item, Func<Type, MethodInfo> itemMethod)
    {
        if (TypeMeta.IsPrimitive(item))
        {
            var t = typeof(PrimitiveImpl);
            GenGetInstance(ilg, t, TS, itemMethod);
        }
        else if (TypeMeta.IsISerialize(item))
        {
            var t = typeof(IdentityImpl<>).MakeGenericType(item);
            GenGetInstance(ilg, t, TS, itemMethod);
        }
        else if (TypeMeta.IsIGetSerialize(item))
        {
            var t = typeof(ByImpl<,>).MakeGenericType(TypeMeta.GetIGetSerializeGeneric(item)!);
            GenGetInstance(ilg, t, TS, itemMethod);
        }
        else if (TypeMeta.IsICollection(item))
        {
            var t = typeof(DynamicCollectionImpl<>).MakeGenericType(item);
            GenGetInstance(ilg, t, TS, itemMethod);
        }
        else if (TypeMeta.IsICollectionT(item))
        {
            var t = typeof(CollectionImpl<,,>).MakeGenericType(item, TypeMeta.GetICollectionTGeneric(item)!);
            // todo
        }
        else
        {
            var t = typeof(RuntimeSerialize<>).MakeGenericType(item);
            GenGetInstance(ilg, t, TS, itemMethod);
        }
    }

    private static void GenGetInstance(ILGenerator ilg, Type rt)
    {
        ilg.EmitCall(OpCodes.Call, rt.GetProperty("Instance")!.GetMethod!, null);
    }

    private static void GenGetInstance(ILGenerator ilg, Type rt, Type TS, Func<Type, MethodInfo> itemMethod)
    {
        GenGetInstance(ilg, rt);
        ilg.Emit(OpCodes.Constrained, TS);
        ilg.EmitCall(OpCodes.Callvirt, itemMethod(rt), null);
    }
}

internal static class RuntimeAsyncSerialize<S, T> where S : IAsyncSerializer
{
    public static Lazy<RuntimeAsyncSerializeDelegate<S, T>> Delegate = new(Init);

    private static RuntimeAsyncSerializeDelegate<S, T> Init()
    {
        var TS = typeof(S);
        var TT = typeof(T);
        var fn = new DynamicMethod($"RuntimeAsyncSerialize for {TT.GUID:N}", MethodAttributes.Public | MethodAttributes.Static,
            CallingConventions.Standard, typeof(ValueTask), new[] { TS, TT }, TT.Module, true);
        var ilg = fn.GetILGenerator();

        throw new NotImplementedException("todo");
        //GenStruct(ilg, TS, TT);
        //todo other type

        ilg.EmitCall(OpCodes.Call, typeof(ValueTask).GetProperty(nameof(ValueTask.CompletedTask))!.GetMethod!, null);
        ilg.Emit(OpCodes.Ret);

        return (RuntimeAsyncSerializeDelegate<S, T>)fn.CreateDelegate(typeof(RuntimeAsyncSerializeDelegate<S, T>));
    }
}
