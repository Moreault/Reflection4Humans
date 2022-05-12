namespace Reflection4Humans.TypeFetcher.Tests.Dummies;

public interface IGenericDummy<out T>
{
    T Value { get; }
}