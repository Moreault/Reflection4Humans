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

        var searchOptions = properties.Distinct(new MemberInfoEqualityComparer<PropertyInfo>()).Select(x => new
        {
            PropertyInfo = x,
            Search = new PropertySearchOptions
            {
                IsPublic = x.IsPublic(),
                IsInternal = x.IsInternal(),
                IsProtected = x.IsProtected(),
                IsPrivate = x.IsPrivate(),
                IsStatic = x.IsStatic(),
                IsInstance = !x.IsStatic(),
                IsGet = x.GetMethod != null,
                IsSet = x.SetMethod != null
            }
        });
        return searchOptions.Where(x => predicate(x.Search)).Select(x => x.PropertyInfo);
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

public sealed record PropertySearchOptions : MemberSearcOptionsBase
{
    public bool IsGet { get; init; }
    public bool IsSet { get; init; }
}