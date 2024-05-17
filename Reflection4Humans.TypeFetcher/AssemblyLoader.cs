namespace ToolBX.Reflection4Humans.TypeFetcher;

#pragma warning disable CS8604
internal static class AssemblyLoader
{
    private static bool _areAssembliesLoaded;

    // Source: https://dotnetstories.com/blog/Dynamically-pre-load-assemblies-in-a-ASPNET-Core-or-any-C-project-en-7155735300
    public static void EnsureAllLoaded(bool includeFramework = false)
    {
        if (_areAssembliesLoaded) return;

        var loaded = new ConcurrentDictionary<string, bool>();

        bool ShouldLoad(string assemblyName)
        {
            return (includeFramework || IsNotNetFramework(assemblyName)) && !loaded.ContainsKey(assemblyName);
        }

        bool IsNotNetFramework(string assemblyName)
        {
            return !assemblyName.StartsWith("Microsoft.")
                   && !assemblyName.StartsWith("System.")
                   && !assemblyName.StartsWith("Newtonsoft.")
                   && assemblyName != "netstandard";
        }

        void LoadReferencedAssembly(Assembly assembly)
        {
            try
            {
                foreach (var an in assembly.GetReferencedAssemblies().Where(a => ShouldLoad(a.FullName)))
                {
                    LoadReferencedAssembly(Assembly.Load(an));
                    loaded.TryAdd(an.FullName, true);
                }
            }
            catch (FileNotFoundException)
            {
                //Happens with some (usually unimportant) assemblies but still unsure of the reason as of 2023-06-28
            }
            catch (ReflectionTypeLoadException)
            {
                // Ignored (Same as above)
            }
        }

        foreach (var a in AppDomain.CurrentDomain.GetAssemblies().Where(a => ShouldLoad(a.FullName)))
        {
            loaded.TryAdd(a.FullName, true);
        }

        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies().Where(a => IsNotNetFramework(a.FullName)))
            LoadReferencedAssembly(assembly);

        _areAssembliesLoaded = true;
    }
}
#pragma warning restore CS8604