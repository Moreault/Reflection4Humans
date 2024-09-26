namespace ToolBX.Reflection4Humans.Extensions;

public static class MemberInfoExtensions
{
    public static bool IsStatic(this MemberInfo memberInfo)
    {
        if (memberInfo == null) throw new ArgumentNullException(nameof(memberInfo));

        if (memberInfo is FieldInfo fieldInfo)
            return fieldInfo.IsStatic;
        if (memberInfo is PropertyInfo propertyInfo)
            return propertyInfo.IsStatic();
        if (memberInfo is MethodBase methodInfo)
            return methodInfo.IsStatic;
        if (memberInfo is Type type)
            return type.IsClass && type.IsAbstract && type.IsSealed;
        if (memberInfo is EventInfo eventInfo)
            return eventInfo.AddMethod?.IsStatic ?? eventInfo.AddMethod!.IsStatic;
        throw new NotSupportedException(string.Format(Exceptions.MemberKindUnsupported, nameof(IsStatic), memberInfo.DeclaringType?.GetHumanReadableName() ?? "(null)"));
    }

    public static bool IsInstance(this MemberInfo memberInfo) => !memberInfo.IsStatic();

    public static bool IsPrivate(this MemberInfo memberInfo)
    {
        if (memberInfo == null) throw new ArgumentNullException(nameof(memberInfo));

        if (memberInfo is FieldInfo fieldInfo)
            return fieldInfo.IsPrivate;
        if (memberInfo is PropertyInfo propertyInfo)
            return propertyInfo.GetMethod?.IsPrivate ?? propertyInfo.SetMethod!.IsPrivate;
        if (memberInfo is MethodBase methodInfo)
            return methodInfo.IsPrivate;
        if (memberInfo is Type type)
            return type.IsNestedPrivate;
        if (memberInfo is EventInfo eventInfo)
            return eventInfo.AddMethod?.IsPrivate ?? eventInfo.RemoveMethod!.IsPrivate;
        throw new NotSupportedException(string.Format(Exceptions.MemberKindUnsupported, nameof(IsPrivate), memberInfo.DeclaringType?.GetHumanReadableName() ?? "(null)"));
    }

    public static bool IsProtected(this MemberInfo memberInfo)
    {
        if (memberInfo == null) throw new ArgumentNullException(nameof(memberInfo));

        if (memberInfo is FieldInfo fieldInfo)
            return fieldInfo.IsFamily;
        if (memberInfo is PropertyInfo propertyInfo)
            return propertyInfo.GetMethod?.IsFamily ?? propertyInfo.SetMethod!.IsFamily;
        if (memberInfo is MethodBase methodInfo)
            return methodInfo.IsFamily;
        if (memberInfo is Type type)
            return type.IsNestedFamily;
        if (memberInfo is EventInfo eventInfo)
            return eventInfo.AddMethod?.IsFamily ?? eventInfo.RemoveMethod!.IsFamily;
        throw new NotSupportedException(string.Format(Exceptions.MemberKindUnsupported, nameof(IsProtected), memberInfo.DeclaringType?.GetHumanReadableName() ?? "(null)"));
    }

    public static bool IsInternal(this MemberInfo memberInfo)
    {
        if (memberInfo == null) throw new ArgumentNullException(nameof(memberInfo));

        if (memberInfo is FieldInfo fieldInfo)
            return fieldInfo.IsAssembly;
        if (memberInfo is PropertyInfo propertyInfo)
            return propertyInfo.GetMethod?.IsAssembly ?? propertyInfo.SetMethod!.IsAssembly;
        if (memberInfo is MethodBase methodInfo)
            return methodInfo.IsAssembly;
        if (memberInfo is Type type)
            return type.IsNestedAssembly;
        if (memberInfo is EventInfo eventInfo)
            return eventInfo.AddMethod?.IsAssembly ?? eventInfo.RemoveMethod!.IsAssembly;
        throw new NotSupportedException(string.Format(Exceptions.MemberKindUnsupported, nameof(IsInternal), memberInfo.DeclaringType?.GetHumanReadableName() ?? "(null)"));
    }

    public static bool IsPublic(this MemberInfo memberInfo)
    {
        if (memberInfo == null) throw new ArgumentNullException(nameof(memberInfo));

        if (memberInfo is FieldInfo fieldInfo)
            return fieldInfo.IsPublic;
        if (memberInfo is PropertyInfo propertyInfo)
            return propertyInfo.GetMethod?.IsPublic ?? propertyInfo.SetMethod!.IsPublic;
        if (memberInfo is MethodBase methodInfo)
            return methodInfo.IsPublic;
        if (memberInfo is EventInfo eventInfo)
            return eventInfo.AddMethod?.IsPublic ?? eventInfo.RemoveMethod!.IsPublic;
        throw new NotSupportedException(string.Format(Exceptions.MemberKindUnsupported, nameof(IsPublic), memberInfo.DeclaringType?.GetHumanReadableName() ?? "(null)"));
    }

    public static bool IsConstructor(this MemberInfo memberInfo)
    {
        if (memberInfo == null) throw new ArgumentNullException(nameof(memberInfo));
        return memberInfo is ConstructorInfo;
    }

    public static bool IsMethod(this MemberInfo memberInfo)
    {
        if (memberInfo == null) throw new ArgumentNullException(nameof(memberInfo));
        return memberInfo is MethodBase;
    }

    public static bool IsField(this MemberInfo memberInfo)
    {
        if (memberInfo == null) throw new ArgumentNullException(nameof(memberInfo));
        return memberInfo is FieldInfo;
    }

    public static bool IsProperty(this MemberInfo memberInfo)
    {
        if (memberInfo == null) throw new ArgumentNullException(nameof(memberInfo));
        return memberInfo is PropertyInfo;
    }

    /// <summary>
    /// Returns the member's <see cref="Type"/>.
    /// </summary>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="NotSupportedException"></exception>
    public static Type GetMemberType(this MemberInfo memberInfo)
    {
        if (memberInfo == null) throw new ArgumentNullException(nameof(memberInfo));

        if (memberInfo is PropertyInfo propertyInfo)
            return propertyInfo.PropertyType;
        if (memberInfo is FieldInfo fieldInfo)
            return fieldInfo.FieldType;
        if (memberInfo is MethodBase methodInfo)
            return methodInfo.GetMethodType();
        if (memberInfo is Type type)
            return type;
        if (memberInfo is EventInfo eventInfo)
            return eventInfo.EventHandlerType!;

        throw new NotSupportedException(string.Format(Exceptions.MemberInfoTypeNotSupported, memberInfo.GetType()));
    }

    public static bool HasAttribute(this MemberInfo member)
    {
        if (member is null) throw new ArgumentNullException(nameof(member));
        return member.GetCustomAttributes().Any();
    }

    public static bool HasAttribute<T>(this MemberInfo member) where T : Attribute => member.HasAttribute(typeof(T));

    public static bool HasAttribute(this MemberInfo member, Type attribute)
    {
        if (member is null) throw new ArgumentNullException(nameof(member));
        if (attribute is null) throw new ArgumentNullException(nameof(attribute));
        return member.GetCustomAttribute(attribute, true) != null;
    }

    public static bool HasAttribute<T>(this MemberInfo member, Func<T, bool> predicate) where T : Attribute
    {
        if (member is null) throw new ArgumentNullException(nameof(member));
        return member.GetCustomAttribute(typeof(T), true) is T attribute && predicate(attribute);
    }
}