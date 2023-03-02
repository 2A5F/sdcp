using System.Collections;
using System.Collections.Generic;

namespace SDcp.Collections;

public class EnumerableImpl<E, TS> : ISerialize<E> where E : IEnumerable where TS : ISerialize<object?>
{
    protected TS serialize;

    public EnumerableImpl(TS serialize)
    {
        this.serialize = serialize;
    }

    public void Serialize<S>(S serializer, in E value) where S : ISerializer
    {
        serializer.EnumerableStart(null);
        foreach (var item in value)
        {
            serializer.EnumerableSerializeElement(in item, serialize);
        }
        serializer.EnumerableEnd();
    }
}

public class EnumerableImpl<E, T, TS> : ISerialize<E> where E : IEnumerable<T> where TS : ISerialize<T>
{
    protected TS serialize;

    public EnumerableImpl(TS serialize)
    {
        this.serialize = serialize;
    }

    public void Serialize<S>(S serializer, in E value) where S : ISerializer
    {
        serializer.EnumerableStart(null);
        foreach (var item in value)
        {
            serializer.EnumerableSerializeElement(in item, serialize);
        }
        serializer.EnumerableEnd();
    }
}

public class EnumerableImpl<E, T, TS, TM> : ISerialize<E> where E : IEnumerable<T> where TS : ISerialize<T> where TM : ITypeMark<T>
{
    protected TS serialize;
    protected TM mark;

    public EnumerableImpl(TS serialize, TM mark)
    {
        this.serialize = serialize;
        this.mark = mark;
    }

    public void Serialize<S>(S serializer, in E value) where S : ISerializer
    {
        serializer.EnumerableStart<T, TM>(null, mark);
        foreach (var item in value)
        {
            serializer.EnumerableSerializeElement(in item, serialize);
        }
        serializer.EnumerableEnd();
    }
}
