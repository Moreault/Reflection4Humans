namespace ToolBX.Reflection4Humans.Extensions.Comparers;

internal sealed class MemberInfoEqualityComparer<T> : IEqualityComparer<T> where T : MemberInfo
{
    public bool Equals(T? x, T? y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (ReferenceEquals(x, null) || ReferenceEquals(y, null)) return false;

        return x.Name == y.Name &&
               x.DeclaringType == y.DeclaringType &&
               ParametersMatch(x, y);
    }

    private bool ParametersMatch(T first, T second)
    {
        if (first is not MethodBase firstMethod || second is not MethodBase secondMethod)
            return true;
        return firstMethod.GetParameters().Select(x => x.ParameterType).SequenceEqual(secondMethod.GetParameters().Select(x => x.ParameterType)) &&
               firstMethod.GetGenericArguments().SequenceEqual(secondMethod.GetGenericArguments());
    }

    public int GetHashCode(T obj)
    {
        return obj.Name.GetHashCode() ^ obj.DeclaringType?.GetHashCode() ?? 0;
    }
}