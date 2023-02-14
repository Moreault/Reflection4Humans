namespace ToolBX.Reflection4Humans.Extensions;

public record PropertyPath
{
    public required PropertyInfo Property { get; init; }
    public required Type Owner { get; init; }
}