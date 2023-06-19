namespace ToolBX.Reflection4Humans.Extensions;

public static class FieldInfoExtensions
{
    public static AccessModifier GetAccessModifier(this FieldInfo fieldInfo)
    {
        if (fieldInfo == null) throw new ArgumentNullException(nameof(fieldInfo));
        if (fieldInfo.IsPublic) return AccessModifier.Public;
        if (fieldInfo.IsPrivate) return AccessModifier.Private;
        if (fieldInfo.IsFamily) return AccessModifier.Protected;
        if (fieldInfo.IsAssembly) return AccessModifier.Internal;
        if (fieldInfo.IsFamilyAndAssembly) return AccessModifier.ProtectedInternal;
        if (fieldInfo.IsPrivateProtected()) return AccessModifier.PrivateProtected;
        throw new NotSupportedException(string.Format(Exceptions.AccessModifierUnsupported));
    }

    public static bool IsPrivateProtected(this FieldInfo fieldInfo)
    {
        if (fieldInfo == null) throw new ArgumentNullException(nameof(fieldInfo));
        return (fieldInfo.Attributes & FieldAttributes.Private) != 0 && (fieldInfo.Attributes & FieldAttributes.Family) != 0;
    }
}