namespace ToolBX.Reflection4Humans.ValueEquality;

public readonly record struct ValueEqualityOptions
{
    public enum EqualityDepth
    {
        /// <summary>
        /// Provides value equality for the current type's property but not beyond that.
        /// </summary>
        Shallow,

        /// <summary>
        /// Provides value equality for the current type's properties and their properties. May be slower on more complex types.
        /// </summary>
        Recursive
    }

    public StringComparison StringComparison { get; init; } = StringComparison.Ordinal;

    public EqualityDepth Depth { get; init; } = EqualityDepth.Shallow;

    public ValueEqualityOptions()
    {

    }
}