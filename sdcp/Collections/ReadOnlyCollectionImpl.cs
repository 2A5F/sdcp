using System.Collections.Generic;
namespace SDcp.Collections;

public class ReadOnlyCollectionImpl<C, T, TS> : ISerialize<C> where C : IReadOnlyCollection<T> where TS : ISerialize<T>
{
    protected TS serialize;

    public ReadOnlyCollectionImpl(TS serialize)
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

public class ReadOnlyCollectionImpl<C, T, TS, TM> : ISerialize<C> where C : IReadOnlyCollection<T> where TS : ISerialize<T> where TM : ITypeMark<T>
{
    protected TS serialize;
    protected TM mark;

    public ReadOnlyCollectionImpl(TS serialize, TM mark)
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
