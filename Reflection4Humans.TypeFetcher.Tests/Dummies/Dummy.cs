namespace Reflection4Humans.TypeFetcher.Tests.Dummies;

public interface IDummy
{
    public int Id { get; }
    public string Name { get; }
    public string Description { get; }
    public DateTimeOffset DateCreated { get; }
}

public record Dummy : IDummy
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public DateTimeOffset DateCreated { get; init; } = ToolBX.TimeProvider.TimeProvider.Now;
}