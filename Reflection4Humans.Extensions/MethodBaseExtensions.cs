namespace ToolBX.Reflection4Humans.Extensions;

public static class MethodBaseExtensions
{
    public static AccessModifier GetAccessModifier(this MethodBase methodInfo)
    {
        if (methodInfo == null) throw new ArgumentNullException(nameof(methodInfo));
        if (methodInfo.IsPublic) return AccessModifier.Public;
        if (methodInfo.IsPrivate) return AccessModifier.Private;
        if (methodInfo.IsFamily) return AccessModifier.Protected;
        if (methodInfo.IsAssembly) return AccessModifier.Internal;
        if (methodInfo.IsFamilyAndAssembly) return AccessModifier.ProtectedInternal;
        if (methodInfo.IsPrivateProtected()) return AccessModifier.PrivateProtected;
        throw new NotSupportedException(string.Format(Exceptions.AccessModifierUnsupported));
    }

    public static bool IsPrivateProtected(this MethodBase methodInfo)
    {
        if (methodInfo == null) throw new ArgumentNullException(nameof(methodInfo));
        return (methodInfo.Attributes & MethodAttributes.Private) != 0 && (methodInfo.Attributes & MethodAttributes.Family) != 0;
    }
}