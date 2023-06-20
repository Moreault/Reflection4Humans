using System.Collections.Immutable;

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

        var searchOptions = members.Distinct(new MemberInfoEqualityComparer()).Select(x =>
        {
            var isStatic = x.IsStatic();
            var isField = x is FieldInfo;
            var isProperty = !isField && x is PropertyInfo;
            var isMethod = !isProperty && x is MethodInfo;
            var isConstructor = !isMethod && x is ConstructorInfo;

            return new
            {
                MemberInfo = x,
                Search = new MemberSearchOptions
                {
                    IsPublic = x.IsPublic(),
                    IsInternal = x.IsInternal(),
                    IsProtected = x.IsProtected(),
                    IsPrivate = x.IsPrivate(),
                    IsStatic = isStatic,
                    IsInstance = !isStatic,
                    IsField = isField,
                    IsProperty = isProperty,
                    IsMethod = isMethod,
                    IsConstructor = isConstructor,
                }
            };
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
                IsPublic = x.IsPublic,
                IsInternal = x.IsAssembly,
                IsProtected = x.IsFamily,
                IsPrivate = x.IsPrivate,
                IsStatic = x.IsStatic,
                IsInstance = !x.IsStatic,
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

        var searchOptions = properties.Select(x => new
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
                IsSet = x.SetMethod != null,
            }
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
                IsPublic = x.IsPublic,
                IsInternal = x.IsAssembly,
                IsProtected = x.IsFamily,
                IsPrivate = x.IsPrivate,
                IsStatic = x.IsStatic,
                IsInstance = !x.IsStatic,
                Parameters = x.GetParameters(),
                GenericParameters = x.GetGenericArguments()
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
                IsPublic = x.IsPublic,
                IsInternal = x.IsAssembly,
                IsProtected = x.IsFamily,
                IsPrivate = x.IsPrivate,
                IsStatic = x.IsStatic,
                IsInstance = !x.IsStatic,
            }
        });
        return searchOptions.Where(x => predicate(x.Search)).Select(x => x.ConstructorInfo).ToList();
    }

    public static ConstructorInfo GetSingleConstructor(this Type type, Func<ConstructorSearchOptions, bool>? predicate = null) => type.GetAllConstructors(predicate).Single();

    public static ConstructorInfo? GetSingleConstructorOrDefault(this Type type, Func<ConstructorSearchOptions, bool>? predicate = null) => type.GetAllConstructors(predicate).Single();
}

public abstract record MemberSearcOptionsBase
{
    public bool IsPublic { get; init; }
    public bool IsInternal { get; init; }
    public bool IsProtected { get; init; }
    public bool IsPrivate { get; init; }

    public bool IsStatic { get; init; }
    public bool IsInstance { get; init; }
}

public sealed record MemberSearchOptions : MemberSearcOptionsBase
{
    public bool IsMethod { get; init; }
    public bool IsField { get; init; }
    public bool IsConstructor { get; init; }
    public bool IsProperty { get; init; }
}

public sealed record FieldSearchOptions : MemberSearcOptionsBase
{

}

public sealed record PropertySearchOptions : MemberSearcOptionsBase
{
    public bool IsGet { get; init; }
    public bool IsSet { get; init; }
}

public abstract record MethodSearchOptionsBase : MemberSearcOptionsBase
{
    internal ParameterInfo[] Parameters { get; init; } = Array.Empty<ParameterInfo>();
    internal Type[] GenericParameters { get; init; } = Array.Empty<Type>();

    public bool HasParameters<T1>() => HasParameters(typeof(T1));
    public bool HasParameters<T1, T2>() => HasParameters(typeof(T1), typeof(T2));
    public bool HasParameters<T1, T2, T3>() => HasParameters(typeof(T1), typeof(T2), typeof(T3));
    public bool HasParameters<T1, T2, T3, T4>() => HasParameters(typeof(T1), typeof(T2), typeof(T3), typeof(T4));
    public bool HasParameters<T1, T2, T3, T4, T5>() => HasParameters(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5));
    public bool HasParameters<T1, T2, T3, T4, T5, T6>() => HasParameters(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6));
    public bool HasParameters<T1, T2, T3, T4, T5, T6, T7>() => HasParameters(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7));
    public bool HasParameters<T1, T2, T3, T4, T5, T6, T7, T8>() => HasParameters(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8));
    public bool HasParameters<T1, T2, T3, T4, T5, T6, T7, T8, T9>() => HasParameters(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9));
    public bool HasParameters<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>() => HasParameters(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10));
    public bool HasParameters(params Type[] parameters) => Parameters.Select(x => x.ParameterType).SequenceEqual(parameters);

    public bool HasGenericParameters(int count) => GenericParameters.Length == count;

    public bool IsGeneric => !HasGenericParameters(0);

}

public sealed record MethodSearchOptions : MethodSearchOptionsBase
{

}

public sealed record ConstructorSearchOptions : MethodSearchOptionsBase
{

}