namespace ToolBX.Reflection4Humans.Extensions;

public static class MemberSearchExtensions
{
    public static IReadOnlyList<MemberInfo> GetAllMembers(this Type type, Func<MemberSearchOptions, bool>? predicate = null) => type.GetAllMembersInternal(predicate).ToList();

    private static IEnumerable<MemberInfo> GetAllMembersInternal(this Type type, Func<MemberSearchOptions, bool>? predicate = null)
    {
        if (type is null) throw new ArgumentNullException(nameof(type));

        IEnumerable<MemberInfo> members = Array.Empty<MemberInfo>();

        var currentType = type;
        do
        {
            members = members.Concat(currentType.GetMembers(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance));
            currentType = currentType.BaseType;
        } while (currentType != null);

        if (predicate is null)
            return members;

        var searchOptions = members.Distinct(new MemberInfoEqualityComparer<MemberInfo>()).Select(x =>
        {
            var isStatic = x.IsStatic();
            var isField = x is FieldInfo;
            var isProperty = !isField && x is PropertyInfo;
            var isMethod = !isProperty && x is MethodInfo;
            var isConstructor = !isMethod && x is ConstructorInfo;

            return new
            {
                MemberInfo = x,
                Search = new MemberSearchOptions
                {
                    IsPublic = x.IsPublic(),
                    IsInternal = x.IsInternal(),
                    IsProtected = x.IsProtected(),
                    IsPrivate = x.IsPrivate(),
                    IsStatic = isStatic,
                    IsInstance = !isStatic,
                    IsField = isField,
                    IsProperty = isProperty,
                    IsMethod = isMethod,
                    IsConstructor = isConstructor
                }
            };
        });

        return searchOptions.Where(x => predicate(x.Search)).Select(x => x.MemberInfo);
    }

    public static MemberInfo GetSingleMember(this Type type, string name, Func<MemberSearchOptions, bool>? predicate = null)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
        return type.GetAllMembersInternal(predicate).Single(x => x.Name == name);
    }

    public static MemberInfo? GetSingleMemberOrDefault(this Type type, string name, Func<MemberSearchOptions, bool>? predicate = null)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
        return type.GetAllMembersInternal(predicate).SingleOrDefault(x => x.Name == name);
    }
}

public abstract record MemberSearcOptionsBase
{
    public bool IsPublic { get; init; }
    public bool IsInternal { get; init; }
    public bool IsProtected { get; init; }
    public bool IsPrivate { get; init; }

    public bool IsStatic { get; init; }
    public bool IsInstance { get; init; }
}

public sealed record MemberSearchOptions : MemberSearcOptionsBase
{
    public bool IsMethod { get; init; }
    public bool IsField { get; init; }
    public bool IsConstructor { get; init; }
    public bool IsProperty { get; init; }
}