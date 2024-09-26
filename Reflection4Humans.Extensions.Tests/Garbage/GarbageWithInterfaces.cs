namespace Reflection4Humans.Extensions.Tests.Garbage;

public interface ITopGarbage1 { }
public interface ITopGarbage2 { }

public class GarbageWithInterfaces : GarbageWithInterfacesBase, ITopGarbage1, ITopGarbage2
{


}

public abstract class GarbageWithInterfacesBase : IDisposable
{
    public void Dispose()
    {
    }
}