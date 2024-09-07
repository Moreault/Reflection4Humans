namespace ToolBX.Reflection4Humans.Extensions.Configuration;

public static class ReflectionConfig
{
    internal static List<BackingFieldConvention> BackingFieldConventions { get; } = new()
    {
        BackingFieldConvention.Csharp
    };

    /// <summary>
    /// Adds a <see cref="BackingFieldConvention"/> to the list.
    /// </summary>
    public static void Add(params BackingFieldConvention[] convention)
    {
        BackingFieldConventions.AddRange(convention);
    }

    /// <summary>
    /// Wipes any pre-existing <see cref="BackingFieldConvention"/> from the list (such as the default csharp convention) and adds your default conventions instead.
    /// </summary>
    public static void Set(params BackingFieldConvention[] convention)
    {
        BackingFieldConventions.Clear();
        Add(convention);
    }
}