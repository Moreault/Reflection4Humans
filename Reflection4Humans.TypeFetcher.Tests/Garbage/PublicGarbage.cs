namespace Reflection4Humans.TypeFetcher.Tests.Garbage;

public class PublicGarbage
{
    private enum PrivateGarbage
    {
        DummyOne,
        DummyTwo,
        DummyThree
    }

    protected struct ProtectedGarbage
    {

    }

    internal interface IInternalGarbage
    {

    }
}