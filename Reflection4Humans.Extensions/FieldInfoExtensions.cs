namespace ToolBX.Reflection4Humans.Extensions;

public static class FieldInfoExtensions
{
    /// <summary>
    /// True if field is a backing field that was created automatically by the compiler.
    /// </summary>
    public static bool IsAutomaticBackingField(this FieldInfo field)
    {
        if (field is null) throw new ArgumentNullException(nameof(field));
        return field.Name.StartsWith("<") && field.Name.EndsWith(">k__BackingField");
    }

    /// <summary>
    /// True if naming convention of field makes it a backing field of a property. Also checks if it's an automatic backing field.
    /// </summary>
    public static bool IsBackingField(this FieldInfo fieldInfo, params BackingFieldConvention[] conventions)
    {
        if (fieldInfo.IsAutomaticBackingField())
            return true;

        if (!conventions.Any())
            conventions = ReflectionConfig.BackingFieldConventions.ToArray();

        var declaringType = fieldInfo.DeclaringType!;

        var propertyNames = conventions.Select(x => fieldInfo.Name.TrimStart(x.Prefix).TrimEnd(x.Suffix)).ToList();

        return propertyNames.Any(property => declaringType.HasProperty(x => x.Name.Equals(property, StringComparison.InvariantCultureIgnoreCase)));
    }
}