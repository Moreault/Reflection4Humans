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
        
        return members.Distinct(new MemberInfoEqualityComparer<MemberInfo>()).Select(x => new MemberSearchOptions(x)).Where(predicate).Select(x => x.MemberInfo);
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

public abstract record MemberSearcOptionsBase<T> where T : MemberInfo
{
    internal readonly T MemberInfo;

    public bool IsPublic => MemberInfo.IsPublic();
    public bool IsInternal => MemberInfo.IsInternal();
    public bool IsProtected => MemberInfo.IsProtected();
    public bool IsPrivate => MemberInfo.IsPrivate();

    public bool IsStatic => MemberInfo.IsStatic();
    public bool IsInstance => !IsStatic;

    protected MemberSearcOptionsBase(T memberInfo)
    {
        MemberInfo = memberInfo ?? throw new ArgumentNullException(nameof(memberInfo));
    }
}

public sealed record MemberSearchOptions : MemberSearcOptionsBase<MemberInfo>
{
    public bool IsMethod => MemberInfo is MethodInfo;
    public bool IsField => MemberInfo is FieldInfo;
    public bool IsConstructor => MemberInfo is ConstructorInfo;
    public bool IsProperty => MemberInfo is PropertyInfo;

    public MemberSearchOptions(MemberInfo memberInfo) : base(memberInfo)
    {
    }
}