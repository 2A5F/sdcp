using System.Collections.Generic;
using System.Threading.Tasks;

namespace SDcp.Collections;

public class AsyncEnumerableAsyncImpl<E, T, TS> : IAsyncSerialize<E> where E : IAsyncEnumerable<T> where TS : IAsyncSerialize<T>
{
    protected TS serialize;

    public AsyncEnumerableAsyncImpl(TS serialize)
    {
        this.serialize = serialize;
    }

    public async ValueTask SerializeAsync<S>(S serializer, E value) where S : IAsyncSerializer
    {
        await serializer.EnumerableStartAsync(null);
        await foreach (var item in value)
        {
            await serializer.EnumerableSerializeElementAsync(item, serialize);
        }
        await serializer.EnumerableEndAsync();
    }
}

public class AsyncEnumerableAsyncImpl<E, T, TS, TM> : IAsyncSerialize<E> where E : IAsyncEnumerable<T> where TS : IAsyncSerialize<T> where TM : IAsyncTypeMark<T>
{
    protected TS serialize;
    protected TM mark;

    public AsyncEnumerableAsyncImpl(TS serialize, TM mark)
    {
        this.serialize = serialize;
        this.mark = mark;
    }

    public async ValueTask SerializeAsync<S>(S serializer, E value) where S : IAsyncSerializer
    {
        await serializer.EnumerableStartAsync<T, TM>(null, mark);
        await foreach (var item in value)
        {
            await serializer.EnumerableSerializeElementAsync(item, serialize);
        }
        await serializer.EnumerableEndAsync();
    }
}
