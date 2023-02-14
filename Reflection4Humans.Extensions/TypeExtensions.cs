namespace ToolBX.Reflection4Humans.Extensions;

public static class TypeExtensions
{
    public static string GetHumanReadableName(this Type type)
    {
        if (type == null) throw new ArgumentNullException(nameof(type), string.Format(Resource.CannotUseMethodBecauseParamaterIsMandatory, nameof(GetHumanReadableName), nameof(type)));

        var name = type.Name;
        var indexOfApostrophe = name.IndexOf('`');
        if (indexOfApostrophe <= -1) return name;

        var genericArguments = type.GetGenericArguments();
        var generics = genericArguments.Select(argument => argument.Name.Any(x => x == '`') ? argument.GetHumanReadableName() : argument.Name).ToList();
        return $"{name[..indexOfApostrophe]}<{string.Join(", ", generics)}>";
    }

    public static bool IsAttribute(this Type type)
    {
        return type == typeof(Attribute) || type.BaseType is { } && type.BaseType.IsAttribute();
    }

    /// <summary>
    /// Returns the full path of a property separated by dots. Ex : Object.Child.Name
    /// </summary>
    public static IReadOnlyList<PropertyPath> GetPropertyPath(this Type type, string propertyName, StringComparison comparison = StringComparison.InvariantCultureIgnoreCase)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));
        if (string.IsNullOrWhiteSpace(propertyName)) throw new ArgumentNullException(nameof(propertyName));

        var path = new List<PropertyPath>();
        var currentType = type;

        var bindingFlags = comparison is StringComparison.CurrentCultureIgnoreCase or StringComparison.InvariantCultureIgnoreCase ?
            BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase :
            BindingFlags.Public | BindingFlags.Instance;

        var splitted = propertyName.Split('.');
        foreach (var part in splitted)
        {
            var property = currentType.GetProperty(part, bindingFlags) ?? throw new ArgumentException(string.Format(Resource.PropertyNotFoundOnType, part, currentType.Name));
            path.Add(new PropertyPath { Property = property, Owner = currentType });
            currentType = property.PropertyType;
        }

        return path;
    }
}