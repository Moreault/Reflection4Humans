namespace ToolBX.Reflection4Humans.TypeFetcher;

public static class Types
{
    /// <summary>
    /// Returns all types from the specified assembly.
    /// </summary>
    public static IEnumerable<Type> From(Assembly assembly)
    {
        if (assembly is null) throw new ArgumentNullException(nameof(assembly));
        return assembly.GetTypes().DistinctBy(x => x.FullName);
    }

    /// <summary>
    /// Queries all types from the all assemblies.
    /// </summary>
    public static IEnumerable<Type> Where(Func<Type, bool> predicate)
    {
        if (predicate is null) throw new ArgumentNullException(nameof(predicate));
        return GetAllTypes().Where(predicate);
    }

    private static IEnumerable<Type> GetAllTypes()
    {
        AssemblyLoader.EnsureAllLoaded(true);
        return AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).DistinctBy(x => x.FullName);
    }

    public static Type First(Func<Type, bool> predicate) => Where(predicate).First();
    public static Type? FirstOrDefault(Func<Type, bool> predicate) => Where(predicate).FirstOrDefault();
    public static Type Single(Func<Type, bool> predicate) => Where(predicate).Single();
    public static Type? SingleOrDefault(Func<Type, bool> predicate) => Where(predicate).SingleOrDefault();

    public static List<Type> ToList() => GetAllTypes().ToList();
    public static Type[] ToArray() => GetAllTypes().ToArray();
}