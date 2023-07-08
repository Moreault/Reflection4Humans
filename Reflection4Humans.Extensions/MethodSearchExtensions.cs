namespace ToolBX.Reflection4Humans.Extensions;

public static class MethodSearchExtensions
{
    public static IReadOnlyList<MethodInfo> GetAllMethods(this Type type, Func<MethodSearchOptions, bool>? predicate = null) => type.GetAllMethodsInternal(predicate).ToList();

    private static IEnumerable<MethodInfo> GetAllMethodsInternal(this Type type, Func<MethodSearchOptions, bool>? predicate = null)
    {
        if (type is null) throw new ArgumentNullException(nameof(type));

        IEnumerable<MethodInfo> methods = Array.Empty<MethodInfo>();

        var currentType = type;
        do
        {
            methods = methods.Concat(currentType.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance));
            currentType = currentType.BaseType;
        } while (currentType != null);

        if (predicate is null)
            return methods;

        return methods.Distinct(new MemberInfoEqualityComparer<MethodInfo>()).Select(x => new MethodSearchOptions(x)).Where(predicate).Select(x => x.MemberInfo);
    }

    public static MethodInfo GetSingleMethod(this Type type, string name, Func<MethodSearchOptions, bool>? predicate = null)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
        return type.GetAllMethodsInternal(predicate).Single(x => x.Name == name);
    }

    public static MethodInfo? GetSingleMethodOrDefault(this Type type, string name, Func<MethodSearchOptions, bool>? predicate = null)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
        return type.GetAllMethods(predicate).SingleOrDefault(x => x.Name == name);
    }
}

public abstract record MethodSearchOptionsBase<T> : MemberSearcOptionsBase<T> where T : MethodBase
{
    internal ParameterInfo[] Parameters => MemberInfo.GetParameters();

    public bool HasParameters<T1>() => HasParameters(typeof(T1));
    public bool HasParameters<T1, T2>() => HasParameters(typeof(T1), typeof(T2));
    public bool HasParameters<T1, T2, T3>() => HasParameters(typeof(T1), typeof(T2), typeof(T3));
    public bool HasParameters<T1, T2, T3, T4>() => HasParameters(typeof(T1), typeof(T2), typeof(T3), typeof(T4));
    public bool HasParameters<T1, T2, T3, T4, T5>() => HasParameters(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5));
    public bool HasParameters<T1, T2, T3, T4, T5, T6>() => HasParameters(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6));
    public bool HasParameters<T1, T2, T3, T4, T5, T6, T7>() => HasParameters(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7));
    public bool HasParameters<T1, T2, T3, T4, T5, T6, T7, T8>() => HasParameters(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8));
    public bool HasParameters<T1, T2, T3, T4, T5, T6, T7, T8, T9>() => HasParameters(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9));
    public bool HasParameters<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>() => HasParameters(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10));
    public bool HasParameters(params Type[] parameters) => Parameters.Select(x => x.ParameterType).SequenceEqual(parameters);

    public bool HasNoParameter => HasParameters();

    public bool HasParameters(int count) => Parameters.Length == count;

    protected MethodSearchOptionsBase(T memberInfo) : base(memberInfo)
    {
    }
}

public sealed record MethodSearchOptions : MethodSearchOptionsBase<MethodInfo>
{
    internal Type[] GenericParameters => MemberInfo.GetGenericArguments();

    public bool HasGenericParameters(int count) => GenericParameters.Length == count;

    public bool IsGeneric => !HasGenericParameters(0);

    public MethodSearchOptions(MethodInfo memberInfo) : base(memberInfo)
    {
    }
}