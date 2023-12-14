namespace ToolBX.Reflection4Humans.ValueEquality;

public enum Depth
{
    /// <summary>
    /// Provides consistent value hash code for the current type's property but not beyond that.
    /// </summary>
    Shallow,

    /// <summary>
    /// Provides consistent value hash code for the current type's properties recursively.
    /// </summary>
    Recursive
}