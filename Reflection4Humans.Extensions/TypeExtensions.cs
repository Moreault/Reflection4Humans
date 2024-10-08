﻿namespace ToolBX.Reflection4Humans.Extensions;

public static class TypeExtensions
{
    public static string GetHumanReadableName(this Type type)
    {
        if (type == null) throw new ArgumentNullException(nameof(type), string.Format(Exceptions.CannotUseMethodBecauseParamaterIsMandatory, nameof(GetHumanReadableName), nameof(type)));

        var name = type.Name;
        var indexOfApostrophe = name.IndexOf('`');
        if (indexOfApostrophe <= -1) return name;

        var genericArguments = type.GetGenericArguments();
        var generics = genericArguments.Select(argument => argument.Name.Any(x => x == '`') ? argument.GetHumanReadableName() : argument.Name).ToList();
        return $"{name[..indexOfApostrophe]}<{string.Join(", ", generics)}>";
    }

    public static bool IsAttribute(this Type type)
    {
        return type == typeof(Attribute) || type.BaseType is not null && type.BaseType.IsAttribute();
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
            var property = currentType.GetProperty(part, bindingFlags) ?? throw new ArgumentException(string.Format(Exceptions.PropertyNotFoundOnType, part, currentType.Name), nameof(propertyName));
            path.Add(new PropertyPath { Property = property, Owner = currentType });
            currentType = property.PropertyType;
        }

        return path;
    }

    /// <summary>
    /// Returns all interfaces on the type excluding those that are inherited.
    /// </summary>
    public static IReadOnlyList<Type> GetDirectInterfaces(this Type type)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));

        var allInterfaces = new List<Type>();
        var childInterfaces = new List<Type>();

        foreach (var i in type.GetInterfaces())
        {
            allInterfaces.Add(i);
            childInterfaces.AddRange(i.GetInterfaces());
        }

        var baseTypeInterfaces = type.BaseType?.GetInterfaces() ?? Array.Empty<Type>();
        childInterfaces.AddRange(baseTypeInterfaces);
        return allInterfaces.Except(childInterfaces).ToList();
    }

    public static bool Implements<T>(this Type type) => type.Implements(typeof(T));

    public static bool Implements(this Type type, Type value)
    {
        if (type is null) throw new ArgumentNullException(nameof(type));
        if (value is null) throw new ArgumentNullException(nameof(value));
        return type.GetInterfaces().Any(x => x == value);
    }

    public static bool HasInterface(this Type type)
    {
        if (type is null) throw new ArgumentNullException(nameof(type));
        return type.GetInterfaces().Any();
    }

    public static bool DirectlyImplements<T>(this Type type) => type.DirectlyImplements(typeof(T));

    public static bool DirectlyImplements(this Type type, Type @interface)
    {
        if (type is null) throw new ArgumentNullException(nameof(type));
        if (@interface is null) throw new ArgumentNullException(nameof(@interface));
        return type.GetDirectInterfaces().Any(x => x == @interface);
    }

    public static object? GetDefaultValue(this Type type)
    {
        if (type is null) throw new ArgumentNullException(nameof(type));
        return type.IsValueType ? Activator.CreateInstance(type) : null;
    }
}