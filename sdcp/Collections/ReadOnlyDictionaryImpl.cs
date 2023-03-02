using System.Collections.Generic;

namespace SDcp.Collections;

public class ReadOnlyDictionaryImpl<D, K, V, KS, VS> : ISerialize<D> where D : IReadOnlyDictionary<K, V> where KS : ISerialize<K> where VS : ISerialize<V>
{
    protected KS k_serialize;
    protected VS v_serialize;

    public ReadOnlyDictionaryImpl(KS k_serialize, VS v_serialize)
    {
        this.k_serialize = k_serialize;
        this.v_serialize = v_serialize;
    }

    public void Serialize<S>(S serializer, in D value) where S : ISerializer
    {
        serializer.DictionaryStart((nuint)value.Count);
        foreach (var (k, v) in value)
        {
            serializer.DictionarySerializeEntry(in k, in v, k_serialize, v_serialize);
        }
        serializer.DictionaryEnd();
    }
}

public class ReadOnlyDictionaryImpl<D, K, V, KS, VS, KM> : ISerialize<D> where D : IReadOnlyDictionary<K, V> where KS : ISerialize<K> where VS : ISerialize<V> where KM : ITypeMark<K>
{
    protected KS k_serialize;
    protected VS v_serialize;
    protected KM k_mark;

    public ReadOnlyDictionaryImpl(KS k_serialize, VS v_serialize, KM k_mark)
    {
        this.k_serialize = k_serialize;
        this.v_serialize = v_serialize;
        this.k_mark = k_mark;
    }

    public void Serialize<S>(S serializer, in D value) where S : ISerializer
    {
        serializer.DictionaryStart<K, KM>((nuint)value.Count, k_mark);
        foreach (var (k, v) in value)
        {
            serializer.DictionarySerializeEntry(in k, in v, k_serialize, v_serialize);
        }
        serializer.DictionaryEnd();
    }
}

public class ReadOnlyDictionaryImpl<D, K, V, KS, VS, KM, VM> : ISerialize<D> where D : IReadOnlyDictionary<K, V> where KS : ISerialize<K> where VS : ISerialize<V> where KM : ITypeMark<K> where VM : ITypeMark<V>
{
    protected KS k_serialize;
    protected VS v_serialize;
    protected KM k_mark;
    protected VM v_mark;

    public ReadOnlyDictionaryImpl(KS k_serialize, VS v_serialize, KM k_mark, VM v_mark)
    {
        this.k_serialize = k_serialize;
        this.v_serialize = v_serialize;
        this.k_mark = k_mark;
        this.v_mark = v_mark;
    }

    public void Serialize<S>(S serializer, in D value) where S : ISerializer
    {
        serializer.DictionaryStart<K, V, KM, VM>((nuint)value.Count, k_mark, v_mark);
        foreach (var (k, v) in value)
        {
            serializer.DictionarySerializeEntry(in k, in v, k_serialize, v_serialize);
        }
        serializer.DictionaryEnd();
    }
}
