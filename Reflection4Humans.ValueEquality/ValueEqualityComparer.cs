namespace ToolBX.Reflection4Humans.ValueEquality;

internal sealed record ValueEqualityComparer : IEqualityComparer<object?>
{
    public ValueEqualityOptions Options { get; init; } = new();

    public new bool Equals(object? x, object? y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (ReferenceEquals(x, null) || ReferenceEquals(y, null)) return false;

        if (x is string string1 && y is string string2)
            return string.Equals(string1, string2, Options.StringComparison);

        if (x is IEnumerable collection1 && y is IEnumerable collection2)
            return collection1.Cast<object>().SequenceEqual(collection2.Cast<object>(), Options.Depth == ValueEqualityOptions.EqualityDepth.Recursive ? this : null);

        if (x.IsNumber() && y.IsNumber())
        {
            return Convert.ToDecimal(x).Equals(Convert.ToDecimal(y));
        }

        return Options.Depth == ValueEqualityOptions.EqualityDepth.Recursive ? x.ValueEquals(y) : x.Equals(y);
    }

    public int GetHashCode(object obj) => obj.GetHashCode();
}