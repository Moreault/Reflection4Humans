namespace Reflection4Humans.Extensions.Tests.Dummies
{
    public interface ITopDummy1 { }
    public interface ITopDummy2 { }

    public class DummyWithInterfaces : DummyWithInterfacesBase, ITopDummy1, ITopDummy2
    {


    }

    public abstract class DummyWithInterfacesBase : IDisposable
    {
        public void Dispose()
        {
        }
    }
}