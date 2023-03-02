namespace SDcp.Json.Internal;

public abstract class AJsonSerializerBase
{
    protected enum State
    {
        None = 0,
        ArrayItem,
        ObjectKey,
        ObjectValue,
    }

    protected State state = State.None;

    protected enum ObjectKeyKind
    {
        String,
        Object,
    }
    protected ObjectKeyKind objectKeyKind = ObjectKeyKind.Object;
}
