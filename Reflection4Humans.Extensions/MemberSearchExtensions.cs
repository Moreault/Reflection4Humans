namespace ToolBX.Reflection4Humans.Extensions;

public static class MemberSearchExtensions
{
    public static IReadOnlyList<MemberInfo> GetAllMembers(this Type type, Func<MemberInfo, bool>? predicate = null) => type.GetAllMembersInternal(predicate).ToList();

    private static IEnumerable<MemberInfo> GetAllMembersInternal(this Type type, Func<MemberInfo, bool>? predicate = null)
    {
        if (type is null) throw new ArgumentNullException(nameof(type));

        IEnumerable<MemberInfo> members = Array.Empty<MemberInfo>();

        var currentType = type;
        do
        {
            members = members.Concat(currentType.GetMembers(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance));
            currentType = currentType.BaseType;
        } while (currentType != null);

        members = members.Distinct(new MemberInfoEqualityComparer<MemberInfo>());
        return predicate is null ? members : members.Where(predicate);
    }

    public static MemberInfo GetSingleMember(this Type type, Func<MemberInfo, bool>? predicate = null) => type.GetAllMembersInternal(predicate).Single();

    public static MemberInfo? GetSingleMemberOrDefault(this Type type, Func<MemberInfo, bool>? predicate = null) => type.GetAllMembersInternal(predicate).SingleOrDefault();

    public static MemberInfo GetSingleMember(this Type type, string name, StringComparison stringComparison = StringComparison.Ordinal)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
        return type.GetAllMembersInternal(x => x.Name.Equals(name, stringComparison)).Single();
    }

    public static MemberInfo? GetSingleMemberOrDefault(this Type type, string name, StringComparison stringComparison = StringComparison.Ordinal)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
        return type.GetAllMembersInternal(x => x.Name.Equals(name, stringComparison)).SingleOrDefault();
    }

    public static bool HasMember(this Type type, string name, StringComparison stringComparison = StringComparison.Ordinal)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
        return type.GetAllMembersInternal(x => x.Name.Equals(name, stringComparison)).Any();
    }

    public static bool HasMember(this Type type, Func<MemberInfo, bool>? predicate = null) => type.GetAllMembersInternal(predicate).Any();

}