namespace Reflection4Humans.TypeFetcher.Tests.Garbage;

public interface IGenericGarbage<out T>
{
    T Value { get; }
}