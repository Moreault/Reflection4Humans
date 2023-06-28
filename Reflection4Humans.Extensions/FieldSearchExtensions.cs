namespace ToolBX.Reflection4Humans.Extensions;

public static class FieldSearchExtensions
{
    public static IReadOnlyList<FieldInfo> GetAllFields(this Type type, Func<FieldSearchOptions, bool>? predicate = null) => type.GetAllFieldsInternal(predicate).ToList();

    private static IEnumerable<FieldInfo> GetAllFieldsInternal(this Type type, Func<FieldSearchOptions, bool>? predicate = null)
    {
        if (type is null) throw new ArgumentNullException(nameof(type));

        IEnumerable<FieldInfo> fields = Array.Empty<FieldInfo>();

        var currentType = type;
        do
        {
            fields = fields.Concat(currentType.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance));
            currentType = currentType.BaseType;
        } while (currentType != null);

        if (predicate is null)
            return fields;

        var searchOptions = fields.Distinct(new MemberInfoEqualityComparer<FieldInfo>()).Select(x => new
        {
            FieldInfo = x,
            Search = new FieldSearchOptions
            {
                IsPublic = x.IsPublic,
                IsInternal = x.IsAssembly,
                IsProtected = x.IsFamily,
                IsPrivate = x.IsPrivate,
                IsStatic = x.IsStatic,
                IsInstance = !x.IsStatic
            }
        });
        return searchOptions.Where(x => predicate(x.Search)).Select(x => x.FieldInfo);
    }

    public static FieldInfo GetSingleField(this Type type, string name, Func<FieldSearchOptions, bool>? predicate = null)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
        return type.GetAllFieldsInternal(predicate).Single(x => x.Name == name);
    }

    public static FieldInfo? GetSingleFieldOrDefault(this Type type, string name, Func<FieldSearchOptions, bool>? predicate = null)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
        return type.GetAllFieldsInternal(predicate).SingleOrDefault(x => x.Name == name);
    }
}

public sealed record FieldSearchOptions : MemberSearcOptionsBase
{

}