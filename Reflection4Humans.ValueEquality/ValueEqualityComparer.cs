namespace ToolBX.Reflection4Humans.ValueEquality;

internal sealed record ValueEqualityComparer : IEqualityComparer<object?>
{
    public ValueEqualityOptions Options { get; init; } = new();

    public new bool Equals(object? x, object? y) => ValueEqualityExtensions.AreEqual(x, y, Options, isPart: false);

    public int GetHashCode(object obj)
    {
        if (obj is string str)
            return StringComparer.FromComparison(Options.StringComparison).GetHashCode(str);

        if (obj.IsNumber())
            return Convert.ToDecimal(obj).GetHashCode();

        return obj.GetValueHashCode(Options.Depth);
    }
}
