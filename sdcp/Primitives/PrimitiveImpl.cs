using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDcp.Primitives;

public partial class PrimitiveImpl
{
    public static PrimitiveImpl Instance { get; } = new();
}
