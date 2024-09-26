namespace ToolBX.Reflection4Humans.Extensions;

public static class PropertyOrFieldExtensions
{
    public static IReadOnlyList<IPropertyOrField> GetAllPropertiesOrFields(this Type type, Func<IPropertyOrField, bool>? predicate = null) => type.GetAllPropertiesOrFieldsInternal(predicate).ToList();

    private static IEnumerable<IPropertyOrField> GetAllPropertiesOrFieldsInternal(this Type type, Func<IPropertyOrField, bool>? predicate = null)
    {
        if (type is null) throw new ArgumentNullException(nameof(type));

        IEnumerable<MemberInfo> members = Array.Empty<MemberInfo>();

        var currentType = type;
        do
        {
            members = members.Concat(currentType.GetMembers(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance));
            currentType = currentType.BaseType;
        } while (currentType != null);

        members = members.Distinct(new MemberInfoEqualityComparer<MemberInfo>());

        var propertiesOrFields = members.TryAsPropertyOrField();

        return predicate is null ? propertiesOrFields : propertiesOrFields.Where(predicate);
    }

    public static IPropertyOrField GetSinglePropertyOrField(this Type type, string name, StringComparison stringComparison = StringComparison.Ordinal)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
        return type.GetAllPropertiesOrFields(x => x.Name.Equals(name, stringComparison)).Single();
    }

    public static IPropertyOrField? GetSinglePropertyOrFieldOrDefault(this Type type, string name, StringComparison stringComparison = StringComparison.Ordinal)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
        return type.GetAllPropertiesOrFields(x => x.Name.Equals(name, stringComparison)).SingleOrDefault();
    }

    public static IPropertyOrField GetSinglePropertyOrField(this Type type, Func<IPropertyOrField, bool>? predicate = null) => type.GetAllPropertiesOrFields(predicate).Single();

    public static IPropertyOrField? GetSinglePropertyOrFieldOrDefault(this Type type, Func<IPropertyOrField, bool>? predicate = null) => type.GetAllPropertiesOrFields(predicate).SingleOrDefault();

    public static IEnumerable<IPropertyOrField> AsPropertyOrField(this IEnumerable<MemberInfo> source)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        foreach (var member in source)
            yield return member.AsPropertyOrField();
    }

    public static IPropertyOrField AsPropertyOrField(this MemberInfo member)
    {
        if (member is null) throw new ArgumentNullException(nameof(member));
        return new PropertyOrField(member);
    }

    public static IEnumerable<IPropertyOrField> TryAsPropertyOrField(this IEnumerable<MemberInfo> source)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        foreach (var member in source.Where(x => x is FieldInfo or PropertyInfo))
            yield return new PropertyOrField(member);
    }

    public static Result<IPropertyOrField> TryAsPropertyOrField(this MemberInfo? member)
    {
        if (member is null || member is not FieldInfo && member is not PropertyInfo)
            return Result<IPropertyOrField>.Failure();

        return Result<IPropertyOrField>.Success(member.AsPropertyOrField());
    }

    public static bool HasPropertyOrField(this Type type, Func<IPropertyOrField, bool>? predicate = null) => type.GetAllPropertiesOrFieldsInternal(predicate).Any();
}