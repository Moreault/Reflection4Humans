namespace ToolBX.Reflection4Humans.Extensions;

public static class MemberInfoExtensions
{
    public static bool IsStatic(this MemberInfo memberInfo)
    {
        ArgumentNullException.ThrowIfNull(memberInfo);
        return memberInfo switch
        {
            FieldInfo f => f.IsStatic,
            PropertyInfo p => p.IsStatic(),
            MethodBase m => m.IsStatic,
            Type t => t.IsClass && t.IsAbstract && t.IsSealed,
            EventInfo e => e.AddMethod?.IsStatic ?? e.AddMethod!.IsStatic,
            _ => throw new NotSupportedException(string.Format(Exceptions.MemberKindUnsupported, nameof(IsStatic), memberInfo.DeclaringType?.GetHumanReadableName() ?? "(null)"))
        };
    }

    public static bool IsInstance(this MemberInfo memberInfo) => !memberInfo.IsStatic();

    public static bool IsPrivate(this MemberInfo memberInfo)
    {
        ArgumentNullException.ThrowIfNull(memberInfo);
        return memberInfo switch
        {
            FieldInfo f => f.IsPrivate,
            PropertyInfo p => p.GetMethod?.IsPrivate ?? p.SetMethod!.IsPrivate,
            MethodBase m => m.IsPrivate,
            Type t => t.IsNestedPrivate,
            EventInfo e => e.AddMethod?.IsPrivate ?? e.RemoveMethod!.IsPrivate,
            _ => throw new NotSupportedException(string.Format(Exceptions.MemberKindUnsupported, nameof(IsPrivate), memberInfo.DeclaringType?.GetHumanReadableName() ?? "(null)"))
        };
    }

    public static bool IsProtected(this MemberInfo memberInfo)
    {
        ArgumentNullException.ThrowIfNull(memberInfo);
        return memberInfo switch
        {
            FieldInfo f => f.IsFamily,
            PropertyInfo p => p.GetMethod?.IsFamily ?? p.SetMethod!.IsFamily,
            MethodBase m => m.IsFamily,
            Type t => t.IsNestedFamily,
            EventInfo e => e.AddMethod?.IsFamily ?? e.RemoveMethod!.IsFamily,
            _ => throw new NotSupportedException(string.Format(Exceptions.MemberKindUnsupported, nameof(IsProtected), memberInfo.DeclaringType?.GetHumanReadableName() ?? "(null)"))
        };
    }

    public static bool IsInternal(this MemberInfo memberInfo)
    {
        ArgumentNullException.ThrowIfNull(memberInfo);
        return memberInfo switch
        {
            FieldInfo f => f.IsAssembly,
            PropertyInfo p => p.GetMethod?.IsAssembly ?? p.SetMethod!.IsAssembly,
            MethodBase m => m.IsAssembly,
            Type t => t.IsNestedAssembly,
            EventInfo e => e.AddMethod?.IsAssembly ?? e.RemoveMethod!.IsAssembly,
            _ => throw new NotSupportedException(string.Format(Exceptions.MemberKindUnsupported, nameof(IsInternal), memberInfo.DeclaringType?.GetHumanReadableName() ?? "(null)"))
        };
    }

    public static bool IsPublic(this MemberInfo memberInfo)
    {
        ArgumentNullException.ThrowIfNull(memberInfo);
        return memberInfo switch
        {
            FieldInfo f => f.IsPublic,
            PropertyInfo p => p.GetMethod?.IsPublic ?? p.SetMethod!.IsPublic,
            MethodBase m => m.IsPublic,
            EventInfo e => e.AddMethod?.IsPublic ?? e.RemoveMethod!.IsPublic,
            _ => throw new NotSupportedException(string.Format(Exceptions.MemberKindUnsupported, nameof(IsPublic), memberInfo.DeclaringType?.GetHumanReadableName() ?? "(null)"))
        };
    }

    public static bool IsConstructor(this MemberInfo memberInfo)
    {
        ArgumentNullException.ThrowIfNull(memberInfo);
        return memberInfo is ConstructorInfo;
    }

    public static bool IsMethod(this MemberInfo memberInfo)
    {
        ArgumentNullException.ThrowIfNull(memberInfo);
        return memberInfo is MethodBase;
    }

    public static bool IsField(this MemberInfo memberInfo)
    {
        ArgumentNullException.ThrowIfNull(memberInfo);
        return memberInfo is FieldInfo;
    }

    public static bool IsProperty(this MemberInfo memberInfo)
    {
        ArgumentNullException.ThrowIfNull(memberInfo);
        return memberInfo is PropertyInfo;
    }

    /// <summary>
    /// Returns the member's <see cref="Type"/>.
    /// </summary>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="NotSupportedException"></exception>
    public static Type GetMemberType(this MemberInfo memberInfo)
    {
        ArgumentNullException.ThrowIfNull(memberInfo);
        return memberInfo switch
        {
            PropertyInfo p => p.PropertyType,
            FieldInfo f => f.FieldType,
            MethodBase m => m.GetMethodType(),
            Type t => t,
            EventInfo e => e.EventHandlerType!,
            _ => throw new NotSupportedException(string.Format(Exceptions.MemberInfoTypeNotSupported, memberInfo.GetType()))
        };
    }

    public static bool HasAttribute(this MemberInfo member)
    {
        ArgumentNullException.ThrowIfNull(member);
        return member.GetCustomAttributes().Any();
    }

    public static bool HasAttribute<T>(this MemberInfo member) where T : Attribute => member.HasAttribute(typeof(T));

    public static bool HasAttribute(this MemberInfo member, Type attribute)
    {
        ArgumentNullException.ThrowIfNull(member);
        ArgumentNullException.ThrowIfNull(attribute);
        return member.GetCustomAttribute(attribute, true) is not null;
    }

    public static bool HasAttribute<T>(this MemberInfo member, Func<T, bool> predicate) where T : Attribute
    {
        ArgumentNullException.ThrowIfNull(member);
        return member.GetCustomAttribute(typeof(T), true) is T attribute && predicate(attribute);
    }
}