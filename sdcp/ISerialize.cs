using System.Threading.Tasks;

namespace SDcp;

// ReSharper disable once TypeParameterCanBeVariant
public interface ISerialize<T>
{
    public void Serialize<S>(S serializer, in T value) where S : ISerializer;
}

// ReSharper disable once TypeParameterCanBeVariant
public interface IAsyncSerialize<T>
{
    public ValueTask SerializeAsync<S>(S serializer, T value) where S : IAsyncSerializer;
}

// ReSharper disable once TypeParameterCanBeVariant
public interface IGetSerialize<T, S> where S : ISerialize<T>
{
    public S GetSerialize();
}

public interface IGetAsyncSerialize<T,  S> where S : IAsyncSerialize<T>
{
    public ValueTask<S> GetAsyncSerialize();
}
