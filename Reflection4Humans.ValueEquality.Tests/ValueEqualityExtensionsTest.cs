using ToolBX.Reflection4Humans.ValueEquality;

namespace Reflection4Humans.ValueEquality.Tests;

public class ValueEqualityExtensionsTest
{
    public record DummyParent : DummyChild
    {
        public int Id { get; init; }

        public string Name { get; init; }
        //TODO Add public fields on parent AND child as well as protected, internal, private props and fields on both


    }

    public record DummyChild
    {
        public int Age { get; init; }


    }

    [TestClass]
    public class ValueEquals : Tester
    {
        //TODO Test
        [TestMethod]
        public void WhenTwoDifferentObjectsOfSameTypeAreEquivalent_ReturnTrue()
        {
            //Arrange
            var obj1 = Fixture.Create<DummyParent>();
            var obj2 = obj1 with { };

            //Act
            var result = obj1.ValueEquals(obj2);

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenTwoDifferentObjectsOfSameTypeAreEquivalentExceptForChildrenProperties_ReturnFalse()
        {
            //Arrange
            var obj1 = Fixture.Create<DummyParent>();
            var obj2 = obj1 with { Age = Fixture.Create<int>() };

            //Act
            var result = obj1.ValueEquals(obj2);

            //Assert
            result.Should().BeFalse();
        }
    }
}