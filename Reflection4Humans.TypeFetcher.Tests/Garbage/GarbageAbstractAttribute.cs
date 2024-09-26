namespace Reflection4Humans.TypeFetcher.Tests.Garbage;

public abstract class GarbageAbstractAttribute : Attribute
{
    public Type? Type { get; set; }
}