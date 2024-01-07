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
public class MemberInfoEqualityComparerTest
{
    [TestClass]
    public class EqualsMethod : MemberInfoEqualityComparerTester<MemberInfo>
    {
        //TODO Test
        [TestMethod]
        public void WhenBothAreNull_ReturnTrue()
        {
            //Arrange
            //Act
            var result = Instance.Equals(null, null);

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenBothAreSameNonNullReference_ReturnTrue()
        {
            //Arrange
            var first = Fixture.Create<DummyMemberInfo>();

            //Act
            var result = Instance.Equals(first, first);

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenOnlyFirstIsNull_ReturnFalse()
        {
            //Arrange
            var second = Fixture.Create<DummyMemberInfo>();

            //Act
            var result = Instance.Equals(null, second);

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenOnlySecondIsNull_ReturnFalse()
        {
            //Arrange
            var first = Fixture.Create<DummyMemberInfo>();

            //Act
            var result = Instance.Equals(first, null);

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenBothAreDifferentInstanceButEquivalent_ReturnTrue()
        {
            //Arrange
            var first = Fixture.Create<DummyMemberInfo>();
            var second = Fixture.Create<DummyMemberInfo>();

            //Act
            var result = Instance.Equals(first, second);

            //Assert
            result.Should().BeTrue();
        }

        //TODO More tests
    }
}