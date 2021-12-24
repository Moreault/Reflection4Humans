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
}