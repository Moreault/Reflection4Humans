namespace ToolBX.Reflection4Humans.ValueEquality;

public readonly record struct ValueEqualityOptions
{
    public StringComparison StringComparison { get; init; } = StringComparison.Ordinal;

    public Depth Depth { get; init; } = Depth.Shallow;

    public ValueEqualityOptions()
    {

    }
}