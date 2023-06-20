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
        throw new NotSupportedException(string.Format(Exceptions.MemberKindUnsupported, nameof(GetKind), memberInfo.DeclaringType?.GetHumanReadableName() ?? "(null)"));
    }

    public static bool IsPrivate(this MemberInfo memberInfo)
    {
        if (memberInfo == null) throw new ArgumentNullException(nameof(memberInfo));

        if (memberInfo is FieldInfo fieldInfo)
            return fieldInfo.IsPrivate;
        if (memberInfo is PropertyInfo propertyInfo)
            return propertyInfo.GetMethod?.IsPrivate ?? propertyInfo.SetMethod!.IsPrivate;
        if (memberInfo is MethodBase methodInfo)
            return methodInfo.IsPrivate;
        throw new NotSupportedException(string.Format(Exceptions.MemberKindUnsupported, nameof(GetKind), memberInfo.DeclaringType?.GetHumanReadableName() ?? "(null)"));
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
        throw new NotSupportedException(string.Format(Exceptions.MemberKindUnsupported, nameof(GetKind), memberInfo.DeclaringType?.GetHumanReadableName() ?? "(null)"));
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
        throw new NotSupportedException(string.Format(Exceptions.MemberKindUnsupported, nameof(GetKind), memberInfo.DeclaringType?.GetHumanReadableName() ?? "(null)"));
    }

    public static bool IsProtectedInternal(this MemberInfo memberInfo)
    {
        if (memberInfo == null) throw new ArgumentNullException(nameof(memberInfo));

        if (memberInfo is FieldInfo fieldInfo)
            return fieldInfo.IsFamilyAndAssembly;
        if (memberInfo is PropertyInfo propertyInfo)
            return propertyInfo.GetMethod?.IsFamilyAndAssembly ?? propertyInfo.SetMethod!.IsFamilyAndAssembly;
        if (memberInfo is MethodBase methodInfo)
            return methodInfo.IsFamilyAndAssembly;
        throw new NotSupportedException(string.Format(Exceptions.MemberKindUnsupported, nameof(GetKind), memberInfo.DeclaringType?.GetHumanReadableName() ?? "(null)"));
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
        throw new NotSupportedException(string.Format(Exceptions.MemberKindUnsupported, nameof(GetKind), memberInfo.DeclaringType?.GetHumanReadableName() ?? "(null)"));
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
        if (memberInfo.IsPublic()) return AccessModifier.Public;
        if (memberInfo.IsPrivate()) return AccessModifier.Private;
        if (memberInfo.IsProtected()) return AccessModifier.Protected;
        if (memberInfo.IsInternal()) return AccessModifier.Internal;
        if (memberInfo.IsProtectedInternal()) return AccessModifier.ProtectedInternal;
        return AccessModifier.PrivateProtected;
    }
}