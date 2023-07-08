namespace Reflection4Humans.TypeFetcher.Tests.Dummies;

public class DirectlyImplementingDummy : IndirectlyImplementingDummy, IDummy
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public DateTimeOffset DateCreated { get; init; }
}

public abstract class IndirectlyImplementingDummy : IDisposable
{
    public void Dispose()
    {
    }
}