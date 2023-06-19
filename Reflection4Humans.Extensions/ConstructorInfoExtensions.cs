using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolBX.Reflection4Humans.Extensions;

public static class ConstructorInfoExtensions
{
    public static AccessModifier GetAccessModifier(this ConstructorInfo constructorInfo)
    {
        if (constructorInfo == null) throw new ArgumentNullException(nameof(constructorInfo));
        if (constructorInfo.IsPublic) return AccessModifier.Public;
        if (constructorInfo.IsPrivate) return AccessModifier.Private;
        if (constructorInfo.IsFamily) return AccessModifier.Protected;
        if (constructorInfo.IsAssembly) return AccessModifier.Internal;
        if (constructorInfo.IsFamilyAndAssembly) return AccessModifier.ProtectedInternal;
        if (constructorInfo.IsPrivateProtected()) return AccessModifier.PrivateProtected;
        throw new NotSupportedException(string.Format(Exceptions.AccessModifierUnsupported));
    }

    public static bool IsPrivateProtected(this ConstructorInfo constructorInfo)
    {
        if (constructorInfo == null) throw new ArgumentNullException(nameof(constructorInfo));
        return (constructorInfo.Attributes & MethodAttributes.Private) != 0 && (constructorInfo.Attributes & MethodAttributes.Family) != 0;
    }
}