namespace ToolBX.Reflection4Humans.Extensions;

public static class MemberSearchExtensions
{
    private class MemberInfoEqualityComparer : IEqualityComparer<MemberInfo>
    {
        public bool Equals(MemberInfo? first, MemberInfo? second)
        {
            if (ReferenceEquals(first, second)) return true;
            if (ReferenceEquals(first, null) || ReferenceEquals(second, null)) return false;

            return first.Name == second.Name &&
                   first.DeclaringType == second.DeclaringType &&
                   ParametersMatch(first, second);
        }

        private bool ParametersMatch(MemberInfo first, MemberInfo second)
        {
            if (first is not MethodBase firstMethod || second is not MethodBase secondMethod)
                return true;
            return firstMethod.GetParameters().Select(x => x.ParameterType).SequenceEqual(secondMethod.GetParameters().Select(x => x.ParameterType)) &&
                   firstMethod.GetGenericArguments().SequenceEqual(secondMethod.GetGenericArguments());
        }

        public int GetHashCode(MemberInfo obj)
        {
            return obj.Name.GetHashCode() ^ obj.DeclaringType.GetHashCode();
        }
    }

    public static IReadOnlyList<MemberInfo> GetAllMembers(this Type type, Func<MemberSearchOptions, bool>? predicate = null)
    {
        if (type is null) throw new ArgumentNullException(nameof(type));

        IEnumerable<MemberInfo> members = Array.Empty<MemberInfo>();

        var currentType = type;
        do
        {
            members = members.Concat(currentType.GetMembers(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance));
            currentType = currentType.BaseType;
        } while (currentType != null);

        if (predicate is null)
            return members.ToArray();

        var searchOptions = members.Distinct(new MemberInfoEqualityComparer()).Select(x => new
        {
            MemberInfo = x,
            Search = new MemberSearchOptions
            {
                AccessModifier = x.GetAccessModifier(),
                Scope = x.GetAccessScope(),
                Kind = x.GetKind()
            }
        });

        return searchOptions.Where(x => predicate(x.Search)).Select(x => x.MemberInfo).ToList();
    }

    public static MemberInfo GetSingleMember(this Type type, Func<MemberSearchOptions, bool>? predicate = null) => type.GetAllMembers(predicate).Single();

    public static MemberInfo? GetSingleMemberOrDefault(this Type type, Func<MemberSearchOptions, bool>? predicate = null) => type.GetAllMembers(predicate).Single();

    public static IReadOnlyList<FieldInfo> GetAllFields(this Type type, Func<FieldSearchOptions, bool>? predicate = null)
    {
        if (type is null) throw new ArgumentNullException(nameof(type));

        var fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);
        if (predicate is null)
            return fields;

        var searchOptions = fields.Select(x => new
        {
            FieldInfo = x,
            Search = new FieldSearchOptions
            {
                AccessModifier = x.GetAccessModifier(),
                Scope = x.GetAccessScope()
            }
        });
        return searchOptions.Where(x => predicate(x.Search)).Select(x => x.FieldInfo).ToList();
    }

    public static FieldInfo GetSingleField(this Type type, string name, Func<FieldSearchOptions, bool>? predicate = null) => type.GetAllFields(predicate).Single(x => x.Name == name);

    public static FieldInfo? GetSingleFieldOrDefault(this Type type, string name, Func<FieldSearchOptions, bool>? predicate = null) => type.GetAllFields(predicate).Single(x => x.Name == name);

    public static IReadOnlyList<PropertyInfo> GetAllProperties(this Type type, Func<PropertySearchOptions, bool>? predicate = null)
    {
        if (type is null) throw new ArgumentNullException(nameof(type));

        var properties = type.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);
        if (predicate is null)
            return properties;

        var searchOptions = properties.Select(x =>
        {
            Accessor accessor;
            if (x.GetMethod is null)
                accessor = Accessor.SetOnly;
            else if (x.SetMethod is null)
                accessor = Accessor.GetOnly;
            else
                accessor = Accessor.GetAndSet;

            return new
            {
                PropertyInfo = x,
                Search = new PropertySearchOptions
                {
                    AccessModifier = x.GetAccessModifier(),
                    Scope = x.GetAccessScope(),
                    Accessor = accessor
                }
            };
        });
        return searchOptions.Where(x => predicate(x.Search)).Select(x => x.PropertyInfo).ToList();
    }

    public static PropertyInfo GetSingleProperty(this Type type, Func<PropertySearchOptions, bool>? predicate = null) => type.GetAllProperties(predicate).Single();

    public static PropertyInfo? GetSinglePropertyOrDefault(this Type type, Func<PropertySearchOptions, bool>? predicate = null) => type.GetAllProperties(predicate).Single();

    public static IReadOnlyList<MethodInfo> GetAllMethods(this Type type, Func<MethodSearchOptions, bool>? predicate = null) => type.GetAllMethodsInternal(predicate).ToList();

    private static IEnumerable<MethodInfo> GetAllMethodsInternal(this Type type, Func<MethodSearchOptions, bool>? predicate = null)
    {
        if (type is null) throw new ArgumentNullException(nameof(type));

        var methods = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);
        if (predicate is null)
            return methods;

        var searchOptions = methods.Select(x => new
        {
            MethodInfo = x,
            Search = new MethodSearchOptions
            {
                AccessModifier = x.GetAccessModifier(),
                Scope = x.GetAccessScope()
            }
        });
        return searchOptions.Where(x => predicate(x.Search)).Select(x => x.MethodInfo);
    }

    public static MethodInfo GetSingleMethod(this Type type, string name, Func<MethodSearchOptions, bool>? predicate = null) => type.GetAllMethodsInternal(predicate).Single(x => x.Name == name);

    public static MethodInfo? GetSingleMethodOrDefault(this Type type, string name, Func<MethodSearchOptions, bool>? predicate = null) => type.GetAllMethods(predicate).SingleOrDefault(x => x.Name == name);

    public static IReadOnlyList<ConstructorInfo> GetAllConstructors(this Type type, Func<ConstructorSearchOptions, bool>? predicate = null)
    {
        if (type is null) throw new ArgumentNullException(nameof(type));

        var constructors = type.GetConstructors(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);
        if (predicate is null)
            return constructors;

        var searchOptions = constructors.Select(x => new
        {
            ConstructorInfo = x,
            Search = new ConstructorSearchOptions
            {
                AccessModifier = x.GetAccessModifier(),
                Scope = x.GetAccessScope()
            }
        });
        return searchOptions.Where(x => predicate(x.Search)).Select(x => x.ConstructorInfo).ToList();
    }

    public static ConstructorInfo GetSingleConstructor(this Type type, Func<ConstructorSearchOptions, bool>? predicate = null) => type.GetAllConstructors(predicate).Single();

    public static ConstructorInfo? GetSingleConstructorOrDefault(this Type type, Func<ConstructorSearchOptions, bool>? predicate = null) => type.GetAllConstructors(predicate).Single();
}

public abstract record MemberSearcOptionsBase
{
    public AccessModifier AccessModifier { get; init; }
    public AccessScope Scope { get; init; }
}

public sealed record MemberSearchOptions : MemberSearcOptionsBase
{
    public MemberKind Kind { get; init; }
}

public sealed record FieldSearchOptions : MemberSearcOptionsBase
{

}

public sealed record PropertySearchOptions : MemberSearcOptionsBase
{
    public Accessor Accessor { get; init; }
}

public sealed record MethodSearchOptions : MemberSearcOptionsBase
{

}

public sealed record ConstructorSearchOptions : MemberSearcOptionsBase
{

}