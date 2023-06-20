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
        if (fieldInfo.IsFamilyOrAssembly) return AccessModifier.ProtectedInternal;
        return AccessModifier.PrivateProtected;
    }
}