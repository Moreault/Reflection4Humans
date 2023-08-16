namespace ToolBX.Reflection4Humans.TypeGenerator;

public sealed record TypeGenerationOptions
{
    public string AssemblyName
    {
        get => _assemblyName;
        init => _assemblyName = string.IsNullOrWhiteSpace(value) ? throw new ArgumentNullException(nameof(value)) : value;
    }
    private readonly string _assemblyName = "TemporaryAssembly";

    public string ModuleName
    {
        get => _moduleName;
        init => _moduleName = string.IsNullOrWhiteSpace(value) ? throw new ArgumentNullException(nameof(value)) : value;
    }
    private readonly string _moduleName = "TemporaryModule";

    public string TypeName
    {
        get => _typeName;
        init => _typeName = string.IsNullOrWhiteSpace(value) ? throw new ArgumentNullException(nameof(value)) : value;
    }
    private readonly string _typeName = "TemporaryType";
}