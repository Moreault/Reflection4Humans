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
            return collection1.Cast<object>().SequenceEqual(collection2.Cast<object>(), this);

        if (x.IsNumber() && y.IsNumber())
        {
            return Convert.ToDecimal(x).Equals(Convert.ToDecimal(y));
        }

        if (Options.Depth == Depth.Recursive)
        {
            var (xFields, xProps) = MemberCache.GetPublicInstanceMembers(x.GetType());
            var (yFields, yProps) = MemberCache.GetPublicInstanceMembers(y.GetType());
            if (xFields.Any() || xProps.Any() || yFields.Any() || yProps.Any())
                return x.ValueEquals(y, Options);
        }

        return x.Equals(y);
    }

    public int GetHashCode(object obj)
    {
        if (obj is string str)
            return StringComparer.FromComparison(Options.StringComparison).GetHashCode(str);

        if (obj is IEnumerable enumerable)
        {
            unchecked
            {
                var hash = 17;
                foreach (var item in enumerable.Cast<object>())
                    hash = hash * 31 + (item is null ? 0 : GetHashCode(item));
                return hash;
            }
        }

        if (obj.IsNumber())
            return Convert.ToDecimal(obj).GetHashCode();

        return Options.Depth == Depth.Recursive ? obj.GetValueHashCode() : obj.GetHashCode();
    }
}
