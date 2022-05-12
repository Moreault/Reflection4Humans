namespace Reflection4Humans.TypeFetcher.Tests.Dummies;

public abstract class DummyAbstractAttribute : Attribute
{
    public Type Type { get; set; }
}