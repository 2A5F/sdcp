using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SDcp.Collections;

public class CollectionAsyncImpl<C, TS> : IAsyncSerialize<C> where C : ICollection where TS : IAsyncSerialize<object>
{
    protected TS serialize;

    public CollectionAsyncImpl(TS serialize)
    {
        this.serialize = serialize;
    }

    public async ValueTask SerializeAsync<S>(S serializer, C value) where S : IAsyncSerializer
    {
        await serializer.EnumerableStartAsync((nuint)value.Count);
        foreach (var item in value)
        {
            await serializer.EnumerableSerializeElementAsync(item, serialize);
        }
        await serializer.EnumerableEndAsync();
    }
}

public class CollectionAsyncImpl<C, T, TS> : IAsyncSerialize<C> where C : ICollection<T> where TS : IAsyncSerialize<T>
{
    protected TS serialize;

    public CollectionAsyncImpl(TS serialize)
    {
        this.serialize = serialize;
    }

    public async ValueTask SerializeAsync<S>(S serializer, C value) where S : IAsyncSerializer
    {
        await serializer.EnumerableStartAsync((nuint)value.Count);
        foreach (var item in value)
        {
            await serializer.EnumerableSerializeElementAsync(item, serialize);
        }
        await serializer.EnumerableEndAsync();
    }
}

public class CollectionAsyncImpl<C, T, TS, TM> : IAsyncSerialize<C> where C : ICollection<T> where TS : IAsyncSerialize<T> where TM : IAsyncTypeMark<T>
{
    protected TS serialize;
    protected TM mark;

    public CollectionAsyncImpl(TS serialize, TM mark)
    {
        this.serialize = serialize;
        this.mark = mark;
    }

    public async ValueTask SerializeAsync<S>(S serializer, C value) where S : IAsyncSerializer
    {
        await serializer.EnumerableStartAsync<T, TM>((nuint)value.Count, mark);
        foreach (var item in value)
        {
            await serializer.EnumerableSerializeElementAsync(item, serialize);
        }
        await serializer.EnumerableEndAsync();
    }
}
