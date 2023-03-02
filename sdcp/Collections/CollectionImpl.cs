using System.Collections;
using System.Collections.Generic;

namespace SDcp.Collections;

public class CollectionImpl<C, TS> : ISerialize<C> where C : ICollection where TS : ISerialize<object?>
{
    protected TS serialize;

    public CollectionImpl(TS serialize)
    {
        this.serialize = serialize;
    }

    public void Serialize<S>(S serializer, in C value) where S : ISerializer
    {
        serializer.EnumerableStart((nuint)value.Count);
        foreach (var item in value)
        {
            serializer.EnumerableSerializeElement(in item, serialize);
        }
        serializer.EnumerableEnd();
    }
}

public class CollectionImpl<C, T, TS> : ISerialize<C> where C : ICollection<T> where TS : ISerialize<T>
{
    protected TS serialize;

    public CollectionImpl(TS serialize)
    {
        this.serialize = serialize;
    }

    public void Serialize<S>(S serializer, in C value) where S : ISerializer
    {
        serializer.EnumerableStart((nuint)value.Count);
        foreach (var item in value)
        {
            serializer.EnumerableSerializeElement(in item, serialize);
        }
        serializer.EnumerableEnd();
    }
}

public class CollectionImpl<C, T, TS, TM> : ISerialize<C> where C : ICollection<T> where TS : ISerialize<T> where TM : ITypeMark<T>
{
    protected TS serialize;
    protected TM mark;

    public CollectionImpl(TS serialize, TM mark)
    {
        this.serialize = serialize;
        this.mark = mark;
    }

    public void Serialize<S>(S serializer, in C value) where S : ISerializer
    {
        serializer.EnumerableStart<T, TM>((nuint)value.Count, mark);
        foreach (var item in value)
        {
            serializer.EnumerableSerializeElement(in item, serialize);
        }
        serializer.EnumerableEnd();
    }
}
