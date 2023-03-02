using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDcp;

// ReSharper disable once TypeParameterCanBeVariant
public interface ITypeMark<T>
{
    public void MatchType<M>(M matcher, T _ = default!) where M : ITypeMatch;
}

// ReSharper disable once TypeParameterCanBeVariant
public interface IAsyncTypeMark<T>
{
    public ValueTask MatchTypeAsync<M>(M matcher, T _ = default!) where M : IAsyncTypeMatch;
}
