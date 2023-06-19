namespace ToolBX.Reflection4Humans.Extensions;

public static class PropertyExtensions
{
    public static bool IsStatic(this PropertyInfo propertyInfo)
    {
        if (propertyInfo is null) throw new ArgumentNullException(nameof(propertyInfo));
        return propertyInfo.GetGetMethod()?.IsStatic ?? propertyInfo.GetSetMethod()?.IsStatic ?? false;
    }

    /// <summary>
    /// Returns whichever is the highest access modifier out of the property's get or set accessors.
    /// </summary>
    public static AccessModifier GetAccessModifier(this PropertyInfo propertyInfo)
    {
        if (propertyInfo == null) throw new ArgumentNullException(nameof(propertyInfo));

        var method = propertyInfo.GetMethod ?? propertyInfo.SetMethod;
        return method!.GetAccessModifier();
    }

    public static bool IsPrivateProtected(this PropertyInfo propertyInfo)
    {
        if (propertyInfo == null) throw new ArgumentNullException(nameof(propertyInfo));
        var method = propertyInfo.GetMethod ?? propertyInfo.SetMethod;
        return method!.IsPrivateProtected();
    }
}