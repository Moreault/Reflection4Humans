namespace ToolBX.Reflection4Humans.Extensions;

public static class PropertySearchExtensions
{
    public static IReadOnlyList<PropertyInfo> GetAllProperties(this Type type, Func<PropertyInfo, bool>? predicate = null) => type.GetAllPropertiesInternal(predicate).ToList();

    private static IEnumerable<PropertyInfo> GetAllPropertiesInternal(this Type type, Func<PropertyInfo, bool>? predicate = null)
    {
        if (type is null) throw new ArgumentNullException(nameof(type));

        IEnumerable<PropertyInfo> properties = Array.Empty<PropertyInfo>();

        var currentType = type;
        do
        {
            properties = properties.Concat(currentType.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance));
            currentType = currentType.BaseType;
        } while (currentType != null);

        properties = properties.Distinct(new MemberInfoEqualityComparer<PropertyInfo>());
        return predicate is null ? properties : properties.Where(predicate);
    }

    public static PropertyInfo GetSingleProperty(this Type type, string name, StringComparison stringComparison = StringComparison.Ordinal)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
        return type.GetAllProperties(x => x.Name.Equals(name, stringComparison)).Single();
    }

    public static PropertyInfo? GetSinglePropertyOrDefault(this Type type, string name, StringComparison stringComparison = StringComparison.Ordinal)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
        return type.GetAllProperties(x => x.Name.Equals(name, stringComparison)).SingleOrDefault();
    }

    public static PropertyInfo GetSingleProperty(this Type type, Func<PropertyInfo, bool>? predicate = null) => type.GetAllProperties(predicate).Single();

    public static PropertyInfo? GetSinglePropertyOrDefault(this Type type, Func<PropertyInfo, bool>? predicate = null) => type.GetAllProperties(predicate).SingleOrDefault();
}