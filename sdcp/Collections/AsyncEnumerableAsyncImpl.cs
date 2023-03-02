using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SDcp.Collections;

public class EnumerableAsyncImpl<E, TS> : IAsyncSerialize<E> where E : IEnumerable where TS : IAsyncSerialize<object>
{
    protected TS serialize;

    public EnumerableAsyncImpl(TS serialize)
    {
        this.serialize = serialize;
    }

    public async ValueTask SerializeAsync<S>(S serializer, E value) where S : IAsyncSerializer
    {
        await serializer.EnumerableStartAsync(null);
        foreach (var item in value)
        {
            await serializer.EnumerableSerializeElementAsync(item, serialize);
        }
        await serializer.EnumerableEndAsync();
    }
}

public class EnumerableAsyncImpl<E, T, TS> : IAsyncSerialize<E> where E : IEnumerable<T> where TS : IAsyncSerialize<T>
{
    protected TS serialize;

    public EnumerableAsyncImpl(TS serialize)
    {
        this.serialize = serialize;
    }

    public async ValueTask SerializeAsync<S>(S serializer, E value) where S : IAsyncSerializer
    {
        await serializer.EnumerableStartAsync(null);
        foreach (var item in value)
        {
            await serializer.EnumerableSerializeElementAsync(item, serialize);
        }
        await serializer.EnumerableEndAsync();
    }
}

public class EnumerableAsyncImpl<E, T, TS, TM> : IAsyncSerialize<E> where E : IEnumerable<T> where TS : IAsyncSerialize<T> where TM : IAsyncTypeMark<T>
{
    protected TS serialize;
    protected TM mark;

    public EnumerableAsyncImpl(TS serialize, TM mark)
    {
        this.serialize = serialize;
        this.mark = mark;
    }

    public async ValueTask SerializeAsync<S>(S serializer, E value) where S : IAsyncSerializer
    {
        await serializer.EnumerableStartAsync<T, TM>(null, mark);
        foreach (var item in value)
        {
            await serializer.EnumerableSerializeElementAsync(item, serialize);
        }
        await serializer.EnumerableEndAsync();
    }
}
