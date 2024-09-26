namespace Reflection4Humans.ValueEquality.Tests;

[TestClass]
public sealed class ValueHashCodeExtensionsTests : Tester
{
    public sealed class SimpleGarbage
    {
        public string Name { get; init; } = null!;
        public int Age { get; init; }
    }

    public sealed class ComplexGarbage
    {
        public string Name { get; init; } = null!;
        public int Age { get; init; }
        public SimpleGarbage SimpleGarbage { get; init; } = null!;
        public char Field;
    }

    public sealed class Splitted<T>
    {
        public IReadOnlyList<T> Remaining { get; init; } = Array.Empty<T>();

        public IReadOnlyList<T> Excluded { get; init; } = Array.Empty<T>();
    }

    [TestMethod]
    public void WhenIsNullEnumerable_ReturnZero()
    {
        //Arrange
        IEnumerable<SimpleGarbage> value = null!;

        //Act
        var result = value.GetValueHashCode();

        //Assert
        result.Should().Be(0);
    }

    [TestMethod]
    public void WhenIsEnumerableWithShallowDepthOnSimpleType_ReturnInconsistentBetweenEquivalentObjects()
    {
        //Arrange
        var value1 = Dummy.CreateMany<SimpleGarbage>().ToList();
        var value2 = value1.Select(x => x.Clone()).ToList();

        //Act
        var result1 = value1.GetValueHashCode(Depth.Shallow);
        var result2 = value2.GetValueHashCode(Depth.Shallow);

        //Assert
        result1.Should().NotBe(result2);
    }

    [TestMethod]
    public void WhenIsEnumerableWithRecursiveDepthOnSimpleType_ReturnConsistentBetweenEquivalentObjects()
    {
        //Arrange
        var value1 = Dummy.CreateMany<SimpleGarbage>().ToList();
        var value2 = value1.Select(x => x.Clone()).ToList();

        //Act
        var result1 = value1.GetValueHashCode();
        var result2 = value2.GetValueHashCode();

        //Assert
        result1.Should().Be(result2);
    }

    [TestMethod]
    public void WhenIsEnumerableWithShallowDepthOnComplexType_ReturnInconsistentBetweenEquivalentObjects()
    {
        //Arrange
        var value1 = Dummy.CreateMany<ComplexGarbage>().ToList();
        var value2 = value1.Select(x => x.Clone()).ToList();

        //Act
        var result1 = value1.GetValueHashCode(Depth.Shallow);
        var result2 = value2.GetValueHashCode(Depth.Shallow);

        //Assert
        result1.Should().NotBe(result2);
    }

    [TestMethod]
    public void WhenIsEnumerableWithRecursiveDepthOnComplexType_ReturnConsistentBetweenEquivalentObjects()
    {
        //Arrange
        var value1 = Dummy.CreateMany<ComplexGarbage>().ToList();
        var value2 = value1.Select(x => x.Clone()).ToList();

        //Act
        var result1 = value1.GetValueHashCode();
        var result2 = value2.GetValueHashCode();

        //Assert
        result1.Should().Be(result2);
    }

    [TestMethod]
    public void WhenSimpleTypesWithShallowDepth_ReturnConsistentCode()
    {
        //Arrange
        var value1 = Dummy.Create<SimpleGarbage>();
        var value2 = value1.Clone();

        //Act
        var result1 = value1.GetValueHashCode(Depth.Shallow);
        var result2 = value2.GetValueHashCode(Depth.Shallow);

        //Assert
        result1.Should().Be(result2);
    }

    [TestMethod]
    public void WhenSimpleTypesWithRecursiveDepth_ReturnConsistentCode()
    {
        //Arrange
        var value1 = Dummy.Create<SimpleGarbage>();
        var value2 = value1.Clone();

        //Act
        var result1 = value1.GetValueHashCode();
        var result2 = value2.GetValueHashCode();

        //Assert
        result1.Should().Be(result2);
    }

    [TestMethod]
    public void WhenComplexTypesWithShallowDepth_ReturnInconsistentCode()
    {
        //Arrange
        var value1 = Dummy.Create<ComplexGarbage>();
        var value2 = value1.Clone();

        //Act
        var result1 = value1.GetValueHashCode(Depth.Shallow);
        var result2 = value2.GetValueHashCode(Depth.Shallow);

        //Assert
        result1.Should().NotBe(result2);
    }

    [TestMethod]
    public void WhenComplexTypesWithRecursiveDepth_ReturnConsistentCode()
    {
        //Arrange
        var value1 = Dummy.Create<ComplexGarbage>();
        var value2 = value1.Clone();

        //Act
        var result1 = value1.GetValueHashCode();
        var result2 = value2.GetValueHashCode();

        //Assert
        result1.Should().Be(result2);
    }

    [TestMethod]
    [DataRow(Depth.Recursive)]
    [DataRow(Depth.Shallow)]
    public void WhenTwoStringsWithSameCasing_ReturnConsistentCode(Depth depth)
    {
        //Arrange
        var value1 = Dummy.Create<string>();
        var value2 = value1;

        //Act
        var result1 = value1.GetValueHashCode(depth);
        var result2 = value2.GetValueHashCode(depth);

        //Assert
        result1.Should().Be(result2);
    }

    [TestMethod]
    [DataRow(Depth.Recursive)]
    [DataRow(Depth.Shallow)]
    public void WhenTwoStringsWithDifferentCasing_ReturnInconsistentCode(Depth depth)
    {
        //Arrange
        var value1 = Dummy.Create<string>().ToLower();
        var value2 = value1.ToUpper();

        //Act
        var result1 = value1.GetValueHashCode(depth);
        var result2 = value2.GetValueHashCode(depth);

        //Assert
        result1.Should().NotBe(result2);
    }

    [TestMethod]
    public void WhenTwoEquivalentObjectsWithTwoListsWithRecursiveDepth_ShouldBeConsistent()
    {
        //Arrange
        var value1 = Dummy.Create<Splitted<SimpleGarbage>>();
        var value2 = value1.Clone();

        //Act
        var result1 = value1.GetValueHashCode();
        var result2 = value2.GetValueHashCode();

        //Assert
        result1.Should().Be(result2);
    }

    [TestMethod]
    public void WhenTwoDifferentObjectsWithTwoListsWithRecursiveDepth_ShouldBeInconsistent()
    {
        //Arrange
        var value1 = Dummy.Create<Splitted<SimpleGarbage>>();
        var value2 = Dummy.Create<Splitted<SimpleGarbage>>();

        //Act
        var result1 = value1.GetValueHashCode();
        var result2 = value2.GetValueHashCode();

        //Assert
        result1.Should().NotBe(result2);
    }

}