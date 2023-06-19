namespace ToolBX.Reflection4Humans.Extensions;

public enum AccessModifier
{
    Public,
    /// <summary>
    /// Also known as "Assembly"
    /// </summary>
    Internal,

    /// <summary>
    /// Also known as "Family"
    /// </summary>
    Protected,

    /// <summary>
    /// Also known as "FamilyAndAssembly"
    /// </summary>
    ProtectedInternal,

    PrivateProtected,
    Private
}