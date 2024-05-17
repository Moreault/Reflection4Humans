namespace Reflection4Humans.Extensions.Tests.Comparers;

public abstract class MemberInfoEqualityComparerTester<T> : Tester where T : MemberInfo
{
    internal MemberInfoEqualityComparer<T> Instance { get; private set; } = null!;

    protected override void InitializeTest()
    {
        base.InitializeTest();
        Instance = new MemberInfoEqualityComparer<T>();
    }
}

[TestClass]
public class MemberInfoEqualityComparerTests : MemberInfoEqualityComparerTester<MemberInfo>
{
    [TestMethod]
    public void Equals_WhenBothAreNull_ReturnTrue()
    {
        //Arrange
        //Act
        var result = Instance.Equals(null, null);

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void Equals_WhenBothAreSameNonNullReference_ReturnTrue()
    {
        //Arrange
        var first = Dummy.Create<GarbageMemberInfo>();

        //Act
        var result = Instance.Equals(first, first);

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void Equals_WhenOnlyFirstIsNull_ReturnFalse()
    {
        //Arrange
        var second = Dummy.Create<GarbageMemberInfo>();

        //Act
        var result = Instance.Equals(null, second);

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void Equals_WhenOnlySecondIsNull_ReturnFalse()
    {
        //Arrange
        var first = Dummy.Create<GarbageMemberInfo>();

        //Act
        var result = Instance.Equals(first, null);

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void Equals_WhenBothAreDifferentInstanceButEquivalent_ReturnTrue()
    {
        //Arrange
        var first = Dummy.Create<GarbageMemberInfo>();
        var second = Dummy.Create<GarbageMemberInfo>();

        //Act
        var result = Instance.Equals(first, second);

        //Assert
        result.Should().BeTrue();
    }

    //TODO More tests
}