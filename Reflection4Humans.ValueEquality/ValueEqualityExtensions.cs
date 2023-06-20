namespace ToolBX.Reflection4Humans.ValueEquality;

public static class ValueEqualityExtensions
{
    /// <summary>
    /// Compares public instance fields and properties with getters of both objects.
    /// </summary>
    public static bool ValueEquals(this object first, object second, ValueEqualityOptions options = default)
    {
        if (ReferenceEquals(first, second)) return true;
        if (ReferenceEquals(first, null) || ReferenceEquals(second, null)) return false;

        var firstFields = first.GetType().GetAllFields(x => x.IsInstance && x.IsPublic);
        var secondFields = second.GetType().GetAllFields(x => x.IsInstance && x.IsPublic);

        if (!firstFields.Select(x => x.GetValue(first)).SequenceEqual(secondFields.Select(x => x.GetValue(second)))) return false;

        var firstProperties = first.GetType().GetAllProperties(x => x.IsInstance && x.IsPublic).Where(x => x.GetMethod != null);
        var secondProperties = second.GetType().GetAllProperties(x => x.IsInstance && x.IsPublic).Where(x => x.GetMethod != null);

        return firstProperties.Select(x => x.GetValue(first)).SequenceEqual(secondProperties.Select(x => x.GetValue(second)));
    }

    //TODO Add an overload and/or option to use ValueEquals recursively (ValueEqualsRecursive?)
    //TODO Add an option to compare strings
}

public readonly record struct ValueEqualityOptions
{
    public StringComparison StringComparison { get; init; } = StringComparison.Ordinal;

    public ValueEqualityOptions()
    {

    }
}