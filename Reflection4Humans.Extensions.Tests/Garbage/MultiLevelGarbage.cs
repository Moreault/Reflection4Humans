namespace Reflection4Humans.Extensions.Tests.Garbage;

public record MultiLevelGarbage
{
    public int Id { get; init; }
    public GarbageChild? Child { get; init; }
}

public record GarbageChild
{
    public string Name { get; init; } = string.Empty;
    public GarbageGrandChild? GrandChild { get; init; }
}

public record GarbageGrandChild
{
    public int Age { get; init; }
}