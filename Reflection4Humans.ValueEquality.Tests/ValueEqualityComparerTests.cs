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
        public void WhenValueIsString_ReturnHashCodeConsistentWithStringComparison()
        {
            //Arrange
            var instance = new ValueEqualityComparer { Options = new ValueEqualityOptions { StringComparison = StringComparison.OrdinalIgnoreCase } };
            var str = Dummy.Create<string>();

            //Act
            var result = instance.GetHashCode(str);

            //Assert
            result.Should().Be(StringComparer.OrdinalIgnoreCase.GetHashCode(str));
        }

        [TestMethod]
        public void WhenValueIsNumber_ReturnDecimalHashCode()
        {
            //Arrange
            var value = Dummy.Create<int>();

            //Act
            var result = Instance.GetHashCode(value);

            //Assert
            result.Should().Be(Convert.ToDecimal(value).GetHashCode());
        }

        [TestMethod]
        public void WhenTwoEqualStringsWithDifferentCasing_ReturnSameHashCodeWithCaseInsensitiveComparison()
        {
            //Arrange
            var instance = new ValueEqualityComparer { Options = new ValueEqualityOptions { StringComparison = StringComparison.OrdinalIgnoreCase } };
            var str1 = Dummy.Create<string>().ToUpperInvariant();
            var str2 = str1.ToLowerInvariant();

            //Act
            var result1 = instance.GetHashCode(str1);
            var result2 = instance.GetHashCode(str2);

            //Assert
            result1.Should().Be(result2);
        }

        [TestMethod]
        public void WhenEquivalentNumericTypes_ReturnSameHashCode()
        {
            //Arrange
            var intValue = Dummy.Create<int>();
            long longValue = intValue;

            //Act
            var result1 = Instance.GetHashCode(intValue);
            var result2 = Instance.GetHashCode(longValue);

            //Assert
            result1.Should().Be(result2);
        }
    }
}
