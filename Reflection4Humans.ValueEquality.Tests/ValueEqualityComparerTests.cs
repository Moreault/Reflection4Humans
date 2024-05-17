namespace Reflection4Humans.ValueEquality.Tests;

public abstract class ValueEqualityComparerTester : Tester
{
    internal ValueEqualityComparer Instance { get; private set; } = null!;

    protected override void InitializeTest()
    {
        base.InitializeTest();
        Instance = new ValueEqualityComparer { Options = Dummy.Create<ValueEqualityOptions>() };

    }
}

[TestClass]
public class ValueEqualityComparerTests
{
    [TestClass]
    public class EqualsMethod : ValueEqualityComparerTester
    {
        [TestMethod]
        public void WhenBothAreNull_ReturnTrue()
        {
            //Arrange

            //Act
            var result = Instance.Equals(null!, null!);

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenXIsNullButYIsNot_ReturnFalse()
        {
            //Arrange
            object x = null!;
            var y = Dummy.Create<object>();

            //Act
            var result = Instance.Equals(x, y);

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenYIsNullButXIsNot_ReturnFalse()
        {
            //Arrange
            var x = Dummy.Create<object>();
            object y = null!;

            //Act
            var result = Instance.Equals(x, y);

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenBothAreStringAndEqual_ReturnTrue()
        {
            //Arrange
            var x = Dummy.Create<string>();

            //Act
            var result = Instance.Equals(x, x);

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        [DataRow(StringComparison.Ordinal, false)]
        [DataRow(StringComparison.OrdinalIgnoreCase, true)]
        [DataRow(StringComparison.CurrentCulture, false)]
        [DataRow(StringComparison.CurrentCultureIgnoreCase, true)]
        [DataRow(StringComparison.InvariantCulture, false)]
        [DataRow(StringComparison.InvariantCultureIgnoreCase, true)]
        public void WhenBothAreStringWithSameTextButDifferentCasing_ReturnTueOrFalseDependingOnStringComparison(StringComparison comparison, bool expected)
        {
            //Arrange
            var x = Dummy.Create<string>().ToUpperInvariant();
            var y = x.ToLowerInvariant();

            //Act
            var result = (Instance with { Options = new ValueEqualityOptions { StringComparison = comparison } }).Equals(x, y);

            //Assert
            result.Should().Be(expected);
        }
    }

    [TestClass]
    public class GetHashCodeMethod : ValueEqualityComparerTester
    {
        [TestMethod]
        public void Always_ReturnObjectHashCode()
        {
            //Arrange
            var obj = Dummy.Create<object>();

            //Act
            var result = Instance.GetHashCode(obj);

            //Assert
            result.Equals(obj.GetHashCode());
        }
    }
}