namespace ToolBX.Reflection4Humans.Extensions.Comparers;

internal class MemberInfoEqualityComparer<T> : IEqualityComparer<T> where T : MemberInfo
{
    public bool Equals(T? first, T? second)
    {
        if (ReferenceEquals(first, second)) return true;
        if (ReferenceEquals(first, null) || ReferenceEquals(second, null)) return false;

        return first.Name == second.Name &&
               first.DeclaringType == second.DeclaringType &&
               ParametersMatch(first, second);
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
        return obj.Name.GetHashCode() ^ obj.DeclaringType.GetHashCode();
    }
}