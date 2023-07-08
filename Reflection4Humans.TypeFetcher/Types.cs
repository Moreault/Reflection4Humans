namespace ToolBX.Reflection4Humans.TypeFetcher;

public static class Types
{
    public static IEnumerable<Type> Where(Func<TypeSearchOptions, bool> predicate)
    {
        if (predicate is null) throw new ArgumentNullException(nameof(predicate));
        return GetAllTypes().Select(x => new TypeSearchOptions(x)).Where(predicate).Select(x => x.Type);
    }

    private static IEnumerable<Type> GetAllTypes()
    {
        AssemblyLoader.EnsureAllLoaded(true);
        return AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).DistinctBy(x => x.FullName);
    }

    public static Type First(Func<TypeSearchOptions, bool> predicate) => Where(predicate).First();
    public static Type? FirstOrDefault(Func<TypeSearchOptions, bool> predicate) => Where(predicate).FirstOrDefault();
    public static Type Single(Func<TypeSearchOptions, bool> predicate) => Where(predicate).Single();
    public static Type? SingleOrDefault(Func<TypeSearchOptions, bool> predicate) => Where(predicate).SingleOrDefault();

    public static List<Type> ToList() => GetAllTypes().ToList();
    public static Type[] ToArray() => GetAllTypes().ToArray();
}

public sealed record TypeSearchOptions
{
    internal readonly Type Type;

    public bool IsClass => Type.IsClass;
    public bool IsAbstract => Type.IsAbstract;
    public bool IsInterface => Type.IsInterface;
    public bool IsGeneric => Type.IsGenericType;
    public bool IsGenericTypeDefinition => Type.IsGenericTypeDefinition;
    public bool IsAttribute => Type.IsAttribute();
    public bool IsStruct => Type.IsValueType;
    public bool IsEnum => Type.IsEnum;
    public bool IsPublic => Type.IsPublic;
    public bool IsPrivate => Type.IsNestedPrivate;
    public bool IsInternal => Type.IsNestedAssembly;
    public bool IsProtected => Type.IsNestedFamily;
    public bool IsSealed => Type.IsSealed;
    public bool IsStatic => Type.IsStatic();
    public string Name => Type.Name;

    public TypeSearchOptions(Type type)
    {
        Type = type ?? throw new ArgumentNullException(nameof(type));
    }

    public bool HasNoAttribute => !Type.GetCustomAttributes().Any();

    public bool HasAttribute<T>(Func<T, bool>? predicate = null) where T : Attribute
    {
        if (Type.GetCustomAttribute(typeof(T), true) is not T attribute) return false;
        return predicate is null || predicate(attribute);
    }

    public bool HasAttribute(Type type)
    {
        if (type is null) throw new ArgumentNullException(nameof(type));
        return Type.GetCustomAttribute(type, true) != null;
    }

    public bool HasNoInterface => !Type.GetInterfaces().Any();

    public bool Implements<T>() => Type.Implements<T>();

    public bool Implements(Type type) => Type.Implements(type);

    public bool DirectlyImplements<T>() => DirectlyImplements(typeof(T));

    public bool DirectlyImplements(Type type)
    {
        if (type is null) throw new ArgumentNullException(nameof(type));
        return Type.GetDirectInterfaces().Any(x => x == type);
    }
}