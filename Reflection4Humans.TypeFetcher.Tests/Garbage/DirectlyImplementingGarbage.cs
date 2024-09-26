namespace Reflection4Humans.TypeFetcher.Tests.Garbage;

public class DirectlyImplementingGarbage : IndirectlyImplementingGarbage, IGarbage
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public DateTimeOffset DateCreated { get; init; }
}

public abstract class IndirectlyImplementingGarbage : IDisposable
{
    public void Dispose()
    {
    }
}