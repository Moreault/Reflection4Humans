namespace ToolBX.Reflection4Humans.Extensions;

public static class PropertyExtensions
{
    public static bool IsStatic(this PropertyInfo propertyInfo)
    {
        if (propertyInfo is null) throw new ArgumentNullException(nameof(propertyInfo));
        return propertyInfo.GetMethod?.IsStatic ?? propertyInfo.SetMethod?.IsStatic ?? false;
    }

    public static bool IsGet(this PropertyInfo propertyInfo)
    {
        if (propertyInfo is null) throw new ArgumentNullException(nameof(propertyInfo));
        return propertyInfo.GetMethod != null;
    }

    public static bool IsSet(this PropertyInfo propertyInfo)
    {
        if (propertyInfo is null) throw new ArgumentNullException(nameof(propertyInfo));
        return propertyInfo.SetMethod != null;
    }

    public static bool IsIndexer(this PropertyInfo propertyInfo)
    {
        if (propertyInfo is null) throw new ArgumentNullException(nameof(propertyInfo));
        return propertyInfo.GetIndexParameters().Length > 0;
    }
}