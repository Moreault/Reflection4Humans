namespace ToolBX.Reflection4Humans.Extensions;

public static class MethodSearchExtensions
{
    public static IReadOnlyList<MethodInfo> GetAllMethods(this Type type, Func<MethodInfo, bool>? predicate = null) => type.GetAllMethodsInternal(predicate).ToList();

    private static IEnumerable<MethodInfo> GetAllMethodsInternal(this Type type, Func<MethodInfo, bool>? predicate = null)
    {
        if (type is null) throw new ArgumentNullException(nameof(type));

        IEnumerable<MethodInfo> methods = Array.Empty<MethodInfo>();

        var currentType = type;
        do
        {
            methods = methods.Concat(currentType.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance));
            currentType = currentType.BaseType;
        } while (currentType != null);

        methods = methods.Distinct(new MemberInfoEqualityComparer<MethodInfo>());
        return predicate is null ? methods : methods.Where(predicate);
    }

    public static MethodInfo GetSingleMethod(this Type type, string name, StringComparison stringComparison = StringComparison.Ordinal)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
        return type.GetAllMethodsInternal(x => x.Name.Equals(name, stringComparison)).Single();
    }

    public static MethodInfo? GetSingleMethodOrDefault(this Type type, string name, StringComparison stringComparison = StringComparison.Ordinal)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
        return type.GetAllMethods(x => x.Name.Equals(name, stringComparison)).SingleOrDefault();
    }

    public static MethodInfo GetSingleMethod(this Type type, Func<MethodInfo, bool>? predicate = null)
    {
        return type.GetAllMethodsInternal(predicate).Single();
    }

    public static MethodInfo? GetSingleMethodOrDefault(this Type type, Func<MethodInfo, bool>? predicate = null)
    {
        return type.GetAllMethods(predicate).SingleOrDefault();
    }

    public static bool HasMethod(this Type type, Func<MethodInfo, bool>? predicate = null) => type.GetAllMethodsInternal(predicate).Any();

}