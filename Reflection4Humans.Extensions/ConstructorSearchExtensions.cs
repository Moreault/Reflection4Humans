namespace ToolBX.Reflection4Humans.Extensions;

public static class ConstructorSearchExtensions
{
    public static IReadOnlyList<ConstructorInfo> GetAllConstructors(this Type type, Func<ConstructorInfo, bool>? predicate = null) => type.GetAllConstructorsInternal(predicate).ToList();

    private static IEnumerable<ConstructorInfo> GetAllConstructorsInternal(this Type type, Func<ConstructorInfo, bool>? predicate = null)
    {
        if (type is null) throw new ArgumentNullException(nameof(type));

        IEnumerable<ConstructorInfo> constructors = Array.Empty<ConstructorInfo>();

        var currentType = type;
        do
        {
            constructors = constructors.Concat(currentType.GetConstructors(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance));
            currentType = currentType.BaseType;
        } while (currentType != null);

        constructors = constructors.Distinct(new MemberInfoEqualityComparer<ConstructorInfo>());
        return predicate is null ? constructors : constructors.Where(predicate);
    }

    public static ConstructorInfo GetSingleConstructor(this Type type, Func<ConstructorInfo, bool>? predicate = null) => type.GetAllConstructorsInternal(predicate).Single();

    public static ConstructorInfo? GetSingleConstructorOrDefault(this Type type, Func<ConstructorInfo, bool>? predicate = null) => type.GetAllConstructorsInternal(predicate).SingleOrDefault();
}