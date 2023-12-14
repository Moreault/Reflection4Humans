﻿using ToolBX.Eloquentest.Extensions;

namespace Reflection4Humans.ValueEquality.Tests;

[TestClass]
public sealed class ValueHashCodeExtensionsTests : Tester
{
    public sealed class SimpleDummy
    {
        public string Name { get; init; } = null!;
        public int Age { get; init; }
    }

    public sealed class ComplexDummy
    {
        public string Name { get; init; } = null!;
        public int Age { get; init; }
        public SimpleDummy SimpleDummy { get; init; } = null!;
        public char Field;
    }

    [TestMethod]
    public void WhenIsNullEnumerable_ReturnZero()
    {
        //Arrange
        IEnumerable<SimpleDummy> value = null!;

        //Act
        var result = value.GetValueHashCode();

        //Assert
        result.Should().Be(0);
    }

    [TestMethod]
    public void WhenIsEnumerableWithShallowDepthOnSimpleType_ReturnInconsistentBetweenEquivalentObjects()
    {
        //Arrange
        var value1 = Fixture.CreateMany<SimpleDummy>().ToList();
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
        var value1 = Fixture.CreateMany<SimpleDummy>().ToList();
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
        var value1 = Fixture.CreateMany<ComplexDummy>().ToList();
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
        var value1 = Fixture.CreateMany<ComplexDummy>().ToList();
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
        var value1 = Fixture.Create<SimpleDummy>();
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
        var value1 = Fixture.Create<SimpleDummy>();
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
        var value1 = Fixture.Create<ComplexDummy>();
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
        var value1 = Fixture.Create<ComplexDummy>();
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
        var value1 = Fixture.Create<string>();
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
        var value1 = Fixture.Create<string>().ToLower();
        var value2 = value1.ToUpper();

        //Act
        var result1 = value1.GetValueHashCode(depth);
        var result2 = value2.GetValueHashCode(depth);

        //Assert
        result1.Should().NotBe(result2);
    }

}