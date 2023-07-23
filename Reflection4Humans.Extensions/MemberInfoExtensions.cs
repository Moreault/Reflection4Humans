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
}