namespace Reflection4Humans.TypeFetcher.Tests.Garbage;

public record GenericGarbage<T>(T Value) : IGenericGarbage<T>;