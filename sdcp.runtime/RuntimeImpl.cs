using System;
using System.Reflection;
using System.Reflection.Emit;

namespace SDcp.Runtime;

public partial class RuntimeImpl<T>
{
    public static RuntimeImpl<T> Instance { get; } = new();
}

internal static partial class RuntimeImpl
{
    public static Guid AssemblyID = Guid.NewGuid();
    public static Lazy<AssemblyName> AssemblyName = new(() => new($"SDcp.RuntimeImpl.{AssemblyID:N}"));
    public static Lazy<AssemblyBuilder> Assembly = new(() =>
        AssemblyBuilder.DefineDynamicAssembly(AssemblyName.Value, AssemblyBuilderAccess.Run));
    public static Lazy<ModuleBuilder> Module = new(() => Assembly.Value.DefineDynamicModule(AssemblyName.Value.Name!));

}
