namespace ToolBX.Reflection4Humans.ValueEquality;

public static class ValueEqualityExtensions
{
    /// <summary>
    /// Compares public instance fields and properties with getters of both objects.
    /// </summary>
    public static bool ValueEquals(this object? first, object? second, ValueEqualityOptions options = default)
        => AreEqual(first, second, options, isPart: false);

    /// <summary>
    /// Compares two values by value.
    /// </summary>
    /// <param name="options"></param>
    /// <param name="isPart">
    /// <see langword="false"/> when <paramref name="first"/>/<paramref name="second"/> are the comparison subject : they are
    /// always decomposed into their public members (or, for collections, their elements) one level down.
    /// <see langword="true"/> when they are a member value or a collection element : under <see cref="Depth.Shallow"/> nested
    /// objects and collections are compared using their own <see cref="object.Equals(object)"/> (record-like), while
    /// <see cref="Depth.Recursive"/> keeps comparing by value all the way down. Strings and numbers always compare by value.
    /// </param>
    /// <param name="first"></param>
    /// <param name="second"></param>
    internal static bool AreEqual(object? first, object? second, ValueEqualityOptions options, bool isPart)
    {
        if (ReferenceEquals(first, second)) return true;
        if (first is null || second is null) return false;

        if (first is string firstString && second is string secondString)
            return string.Equals(firstString, secondString, options.StringComparison);

        if (first.IsNumber() && second.IsNumber())
            return Convert.ToDecimal(first).Equals(Convert.ToDecimal(second));

        if (isPart && options.Depth == Depth.Shallow)
            return first.Equals(second);

        if (first is IEnumerable firstEnumerable && second is IEnumerable secondEnumerable)
            return SequenceEqual(firstEnumerable, secondEnumerable, options);

        var (firstFields, firstProperties) = MemberCache.GetPublicInstanceMembers(first.GetType());
        var (secondFields, secondProperties) = MemberCache.GetPublicInstanceMembers(second.GetType());

        if (firstFields.Count == 0 && secondFields.Count == 0 && firstProperties.Count == 0 && secondProperties.Count == 0)
            return first.Equals(second);

        if (firstFields.Count != secondFields.Count || firstProperties.Count != secondProperties.Count)
            return false;

        for (var i = 0; i < firstFields.Count; i++)
            if (!AreEqual(firstFields[i].GetValue(first), secondFields[i].GetValue(second), options, isPart: true))
                return false;

        for (var i = 0; i < firstProperties.Count; i++)
            if (!AreEqual(firstProperties[i].GetValue(first), secondProperties[i].GetValue(second), options, isPart: true))
                return false;

        return true;
    }

    private static bool SequenceEqual(IEnumerable first, IEnumerable second, ValueEqualityOptions options)
    {
        var firstEnumerator = first.GetEnumerator();
        var secondEnumerator = second.GetEnumerator();
        try
        {
            while (true)
            {
                var firstHasNext = firstEnumerator.MoveNext();
                var secondHasNext = secondEnumerator.MoveNext();
                if (firstHasNext != secondHasNext) return false;
                if (!firstHasNext) return true;
                if (!AreEqual(firstEnumerator.Current, secondEnumerator.Current, options, isPart: true))
                    return false;
            }
        }
        finally
        {
            (firstEnumerator as IDisposable)?.Dispose();
            (secondEnumerator as IDisposable)?.Dispose();
        }
    }

    internal static bool IsNumber(this object? value) => value is sbyte or byte or short or ushort or int or uint or long or ulong or nint or nuint or float or double or decimal or Half or Int128 or UInt128;
}
