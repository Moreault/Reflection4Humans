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
        if (memberInfo is MethodInfo methodInfo)
            return methodInfo.IsStatic;
        if (memberInfo is ConstructorInfo constructorInfo)
            return constructorInfo.IsStatic;

        return false;
    }

    public static bool IsPrivate(this MemberInfo memberInfo)
    {
        if (memberInfo == null) throw new ArgumentNullException(nameof(memberInfo));

        if (memberInfo is FieldInfo fieldInfo)
            return fieldInfo.IsPrivate;
        if (memberInfo is PropertyInfo propertyInfo)
            return propertyInfo.GetMethod?.IsPrivate ?? propertyInfo.SetMethod!.IsPrivate;
        if (memberInfo is MethodInfo methodInfo)
            return methodInfo.IsPrivate;
        if (memberInfo is ConstructorInfo constructorInfo)
            return constructorInfo.IsPrivate;

        return false;
    }

    public static bool IsProtected(this MemberInfo memberInfo)
    {
        if (memberInfo == null) throw new ArgumentNullException(nameof(memberInfo));

        if (memberInfo is FieldInfo fieldInfo)
            return fieldInfo.IsFamily;
        if (memberInfo is PropertyInfo propertyInfo)
            return propertyInfo.GetMethod?.IsFamily ?? propertyInfo.SetMethod!.IsFamily;
        if (memberInfo is MethodInfo methodInfo)
            return methodInfo.IsFamily;
        if (memberInfo is ConstructorInfo constructorInfo)
            return constructorInfo.IsFamily;

        return false;
    }

    public static bool IsInternal(this MemberInfo memberInfo)
    {
        if (memberInfo == null) throw new ArgumentNullException(nameof(memberInfo));

        if (memberInfo is FieldInfo fieldInfo)
            return fieldInfo.IsAssembly;
        if (memberInfo is PropertyInfo propertyInfo)
            return propertyInfo.GetMethod?.IsAssembly ?? propertyInfo.SetMethod!.IsAssembly;
        if (memberInfo is MethodInfo methodInfo)
            return methodInfo.IsAssembly;
        if (memberInfo is ConstructorInfo constructorInfo)
            return constructorInfo.IsAssembly;

        return false;
    }

    public static bool IsProtectedInternal(this MemberInfo memberInfo)
    {
        if (memberInfo == null) throw new ArgumentNullException(nameof(memberInfo));

        if (memberInfo is FieldInfo fieldInfo)
            return fieldInfo.IsFamilyAndAssembly;
        if (memberInfo is PropertyInfo propertyInfo)
            return propertyInfo.GetMethod?.IsFamilyAndAssembly ?? propertyInfo.SetMethod!.IsFamilyAndAssembly;
        if (memberInfo is MethodInfo methodInfo)
            return methodInfo.IsFamilyAndAssembly;
        if (memberInfo is ConstructorInfo constructorInfo)
            return constructorInfo.IsFamilyAndAssembly;

        return false;
    }

    public static bool IsPublic(this MemberInfo memberInfo)
    {
        if (memberInfo == null) throw new ArgumentNullException(nameof(memberInfo));

        if (memberInfo is FieldInfo fieldInfo)
            return fieldInfo.IsPublic;
        if (memberInfo is PropertyInfo propertyInfo)
            return propertyInfo.GetMethod?.IsPublic ?? propertyInfo.SetMethod!.IsPublic;
        if (memberInfo is MethodInfo methodInfo)
            return methodInfo.IsPublic;
        if (memberInfo is ConstructorInfo constructorInfo)
            return constructorInfo.IsPublic;

        return false;
    }

    public static MemberKind GetKind(this MemberInfo memberInfo)
    {
        if (memberInfo == null) throw new ArgumentNullException(nameof(memberInfo));
        if (memberInfo is PropertyInfo) return MemberKind.Property;
        if (memberInfo is FieldInfo) return MemberKind.Field;
        if (memberInfo is MethodInfo methodInfo) return MemberKind.Method;
        if (memberInfo is ConstructorInfo constructorInfo) return MemberKind.Constructor;
        throw new NotSupportedException(string.Format(Exceptions.MemberKindUnsupported, nameof(GetKind), memberInfo.DeclaringType?.GetHumanReadableName() ?? "(null)"));
    }

    public static AccessScope GetAccessScope(this MemberInfo memberInfo)
    {
        return memberInfo.IsStatic() ? AccessScope.Static : AccessScope.Instance;
    }

    public static AccessModifier GetAccessModifier(this MemberInfo memberInfo)
    {
        if (memberInfo == null) throw new ArgumentNullException(nameof(memberInfo));
        if (memberInfo is FieldInfo fieldInfo) return fieldInfo.GetAccessModifier();
        if (memberInfo is PropertyInfo propertyInfo) return propertyInfo.GetAccessModifier();
        if (memberInfo is MethodInfo methodInfo) return methodInfo.GetAccessModifier();
        if (memberInfo is ConstructorInfo constructorInfo) return constructorInfo.GetAccessModifier();
        throw new NotSupportedException(string.Format(Exceptions.MemberKindUnsupported, nameof(GetKind), memberInfo.DeclaringType?.GetHumanReadableName() ?? "(null)"));
    }
}