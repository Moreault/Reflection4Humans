namespace Reflection4Humans.TypeFetcher.Tests.Dummies;

public record GenericDummy<T>(T Value) : IGenericDummy<T>;