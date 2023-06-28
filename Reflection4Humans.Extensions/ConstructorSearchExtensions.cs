﻿namespace ToolBX.Reflection4Humans.Extensions;

public static class ConstructorSearchExtensions
{
    public static IReadOnlyList<ConstructorInfo> GetAllConstructors(this Type type, Func<ConstructorSearchOptions, bool>? predicate = null) => type.GetAllConstructorsInternal(predicate).ToList();

    public static IEnumerable<ConstructorInfo> GetAllConstructorsInternal(this Type type, Func<ConstructorSearchOptions, bool>? predicate = null)
    {
        if (type is null) throw new ArgumentNullException(nameof(type));

        IEnumerable<ConstructorInfo> constructors = Array.Empty<ConstructorInfo>();

        var currentType = type;
        do
        {
            constructors = constructors.Concat(currentType.GetConstructors(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance));
            currentType = currentType.BaseType;
        } while (currentType != null);

        if (predicate is null)
            return constructors;

        var searchOptions = constructors.Distinct(new MemberInfoEqualityComparer<ConstructorInfo>()).Select(x => new
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
                Parameters = x.GetParameters()
            }
        });
        return searchOptions.Where(x => predicate(x.Search)).Select(x => x.ConstructorInfo);
    }


    public static ConstructorInfo GetSingleConstructor(this Type type, Func<ConstructorSearchOptions, bool>? predicate = null) => type.GetAllConstructors(predicate).Single();

    public static ConstructorInfo? GetSingleConstructorOrDefault(this Type type, Func<ConstructorSearchOptions, bool>? predicate = null) => type.GetAllConstructors(predicate).SingleOrDefault();
}

public sealed record ConstructorSearchOptions : MethodSearchOptionsBase
{

}