namespace Reflection4Humans.UnitTesting;

public abstract class Tester
{
    protected Dummy Dummy { get; private set; } = null!;

    [TestInitialize]
    public void TestInitializeBase()
    {
        Dummy = new Dummy();
        InitializeTest();
    }

    protected virtual void InitializeTest()
    {

    }
}