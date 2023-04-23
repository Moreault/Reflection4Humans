namespace Reflection4Humans.Extensions.Tests.Dummies;

public record MultiLevelDummy
{
    public int Id { get; init; }
    public DummyChild? Child { get; init; }
}

public record DummyChild
{
    public string Name { get; init; } = string.Empty;
    public DummyGrandChild? GrandChild { get; init; }
}

public record DummyGrandChild
{
    public int Age { get; init; }
}