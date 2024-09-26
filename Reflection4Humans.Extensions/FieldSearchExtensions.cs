namespace ToolBX.Reflection4Humans.Extensions;

public static class FieldSearchExtensions
{
    public static IReadOnlyList<FieldInfo> GetAllFields(this Type type, Func<FieldInfo, bool>? predicate = null) => type.GetAllFieldsInternal(predicate).ToList();

    private static IEnumerable<FieldInfo> GetAllFieldsInternal(this Type type, Func<FieldInfo, bool>? predicate = null)
    {
        if (type is null) throw new ArgumentNullException(nameof(type));

        IEnumerable<FieldInfo> fields = Array.Empty<FieldInfo>();

        var currentType = type;
        do
        {
            fields = fields.Concat(currentType.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance));
            currentType = currentType.BaseType;
        } while (currentType != null);

        fields = fields.Distinct(new MemberInfoEqualityComparer<FieldInfo>());
        return predicate is null ? fields : fields.Where(predicate);
    }

    public static FieldInfo GetSingleField(this Type type, string name, StringComparison stringComparison = StringComparison.Ordinal)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
        return type.GetAllFieldsInternal(x => x.Name.Equals(name, stringComparison)).Single();
    }

    public static FieldInfo? GetSingleFieldOrDefault(this Type type, string name, StringComparison stringComparison = StringComparison.Ordinal)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
        return type.GetAllFieldsInternal(x => x.Name.Equals(name, stringComparison)).SingleOrDefault();
    }

    public static FieldInfo GetSingleField(this Type type, Func<FieldInfo, bool>? predicate = null) => type.GetAllFieldsInternal(predicate).Single();

    public static FieldInfo? GetSingleFieldOrDefault(this Type type, Func<FieldInfo, bool>? predicate = null) => type.GetAllFieldsInternal(predicate).SingleOrDefault();

    public static bool HasField(this Type type, string name, StringComparison stringComparison = StringComparison.Ordinal)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
        return type.GetAllFieldsInternal(x => x.Name.Equals(name, stringComparison)).Any();
    }

    public static bool HasField(this Type type, Func<FieldInfo, bool>? predicate = null) => type.GetAllFieldsInternal(predicate).Any();

}