namespace ToolBX.Reflection4Humans.ValueEquality;

public static class ValueEqualityExtensions
{
    /// <summary>
    /// Compares public instance fields and properties with getters of both objects.
    /// </summary>
    public static bool ValueEquals(this object? first, object? second, ValueEqualityOptions options = default)
    {
        if (ReferenceEquals(first, second)) return true;
        if (ReferenceEquals(first, null) || ReferenceEquals(second, null)) return false;

        var comparer = new ValueEqualityComparer { Options = options };

        var firstFields = first.GetType().GetAllFields(x => x.IsInstance && x.IsPublic);
        var secondFields = second.GetType().GetAllFields(x => x.IsInstance && x.IsPublic);

        if (!firstFields.Select(x => x.GetValue(first)).SequenceEqual(secondFields.Select(x => x.GetValue(second)), comparer)) return false;

        var firstProperties = first.GetType().GetAllProperties(x => x.IsInstance && x.IsPublic && x.IsGet);
        var secondProperties = second.GetType().GetAllProperties(x => x.IsInstance && x.IsPublic && x.IsGet);

        if (!firstFields.Any() && !secondFields.Any() && !firstProperties.Any() && !secondProperties.Any()) return comparer.Equals(first, second);

        return firstProperties.Select(x => x.GetValue(first)).SequenceEqual(secondProperties.Select(x => x.GetValue(second)), comparer);
    }

    internal static bool IsNumber(this object? value) => value is sbyte or byte or short or ushort or int or uint or long or ulong or float or double or decimal;
}

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