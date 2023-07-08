namespace ToolBX.Reflection4Humans.Extensions;

public static class PropertySearchExtensions
{
    public static IReadOnlyList<PropertyInfo> GetAllProperties(this Type type, Func<PropertySearchOptions, bool>? predicate = null) => type.GetAllPropertiesInternal(predicate).ToList();

    private static IEnumerable<PropertyInfo> GetAllPropertiesInternal(this Type type, Func<PropertySearchOptions, bool>? predicate = null)
    {
        if (type is null) throw new ArgumentNullException(nameof(type));

        IEnumerable<PropertyInfo> properties = Array.Empty<PropertyInfo>();

        var currentType = type;
        do
        {
            properties = properties.Concat(currentType.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance));
            currentType = currentType.BaseType;
        } while (currentType != null);

        if (predicate is null)
            return properties;
        
        return properties.Distinct(new MemberInfoEqualityComparer<PropertyInfo>()).Select(x => new PropertySearchOptions(x)).Where(predicate).Select(x => x.MemberInfo);
    }

    public static PropertyInfo GetSingleProperty(this Type type, string name, Func<PropertySearchOptions, bool>? predicate = null)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
        //TODO Better exception message... wait... can I have a Single that throws a custom message?
        return type.GetAllProperties(predicate).Single(x => x.Name == name);
    }

    public static PropertyInfo? GetSinglePropertyOrDefault(this Type type, string name, Func<PropertySearchOptions, bool>? predicate = null)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
        return type.GetAllProperties(predicate).SingleOrDefault(x => x.Name == name);
    }
}

public sealed record PropertySearchOptions : MemberSearcOptionsBase<PropertyInfo>
{
    public bool IsGet => MemberInfo.GetMethod != null;
    public bool IsSet => MemberInfo.SetMethod != null;

    public PropertySearchOptions(PropertyInfo memberInfo) : base(memberInfo)
    {
    }
}