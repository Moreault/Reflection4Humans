using System.Collections;

namespace Reflection4Humans.ValueEquality.Tests;

public class ValueEqualityExtensionsTests
{
    public record DummyParent : DummyChild
    {
        public int Id { get; init; }

        public string Name { get; init; } = string.Empty;
        //TODO Add public fields on parent AND child as well as protected, internal, private props and fields on both

        public List<string> Strings { get; init; } = new();

        public char Field;
    }

    public record DummyChild
    {
        public int Age { get; init; }

        public int Level
        {
            init => _level = value;
        }
        private int _level;

        public List<long> Longs { get; init; } = new();
    }

    public class DummyCollection<T> : IEnumerable<T>
    {
        public T this[int index]
        {
            get => _items[index];
            set => _items[index] = value;
        }

        private readonly List<T> _items = new();

        public DummyCollection()
        {

        }

        public DummyCollection(IEnumerable<T> items)
        {
            _items.AddRange(items);
        }

        public IEnumerator<T> GetEnumerator() => _items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
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
            var obj2 = obj1 with { Strings = obj1.Strings.ToList(), Longs = obj1.Longs.ToList() };

            //Act
            var result = obj1.ValueEquals(obj2);

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenTwoDifferentObjectsOfSameTypeAreEquivalentExceptForChildrenPropertiesWithShallowCompare_ReturnFalse()
        {
            //Arrange
            var obj1 = Fixture.Create<DummyParent>();
            var obj2 = obj1 with { Age = Fixture.Create<int>() };

            //Act
            var result = obj1.ValueEquals(obj2, new ValueEqualityOptions { Depth = Depth.Shallow });

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenTwoDifferentObjectsOfSameTypeAreEquivalentExceptForChildrenPropertiesWithRecursiveCompare_ReturnFalse()
        {
            //Arrange
            var obj1 = Fixture.Create<DummyParent>();
            var obj2 = obj1 with { Age = Fixture.Create<int>() };

            //Act
            var result = obj1.ValueEquals(obj2, new ValueEqualityOptions { Depth = Depth.Recursive });

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenNumericsOfDifferentTypesButSameValue_ReturnTrue()
        {
            //Arrange
            var value = Fixture.Create<int>();
            long value2 = value;

            //Act
            var result = value.ValueEquals(value2);

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenObjectsHaveStringPropertiesWithDifferentCasingAndCasingIsNotIgnored_ReturnFalse()
        {
            //Arrange
            var obj1 = Fixture.Build<DummyParent>().With(x => x.Name, "roger").Create();
            var obj2 = obj1 with { Name = "Roger" };

            //Act
            var result = obj1.ValueEquals(obj2, new ValueEqualityOptions { StringComparison = StringComparison.Ordinal });

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenObjectsHaveStringPropertiesWithDifferentCasingAndCasingIsIgnored_ReturnTrue()
        {
            //Arrange
            var obj1 = Fixture.Build<DummyParent>().With(x => x.Name, "roger").Create();
            var obj2 = obj1 with { Name = "Roger" };

            //Act
            var result = obj1.ValueEquals(obj2, new ValueEqualityOptions { StringComparison = StringComparison.InvariantCultureIgnoreCase });

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenTwoObjectAreSameReference_ReturnTrue()
        {
            //Arrange
            var obj1 = Fixture.Create<DummyParent>();

            //Act
            var result = obj1.ValueEquals(obj1);

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenBothObjectsAreNull_ReturnTrue()
        {
            //Arrange

            //Act
            var result = ((object)null!).ValueEquals(null);

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenObject1IsNullButObject2IsNot_ReturnFalse()
        {
            //Arrange
            var obj2 = Fixture.Create<DummyParent>();

            //Act
            var result = ((object)null!).ValueEquals(obj2);

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenObject2IsNullButObject1IsNot_ReturnFalse()
        {
            //Arrange
            var obj1 = Fixture.Create<DummyParent>();

            //Act
            var result = obj1.ValueEquals(null);

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenObjectsHaveSamePropertiesButDifferentFields_ReturnFalse()
        {
            //Arrange
            var obj1 = Fixture.Create<DummyParent>();
            var obj2 = obj1 with { Field = Fixture.Create<char>() };

            //Act
            var result = obj1.ValueEquals(obj2);

            //Assert
            result.Should().BeFalse();
        }

        //TODO Test equivalent objects with strings with different casings (also test it in collections)

        //TODO Fix issue with indexers (probably should be ignored)
        [TestMethod]
        public void WhenEquivalentComplexCollection_ShouldBeTrue()
        {
            //Arrange
            var items = Fixture.CreateMany<DummyChild>().ToList();

            var obj1 = new DummyCollection<DummyChild>(items);
            var obj2 = new DummyCollection<DummyChild>(items);

            //Act
            var result = obj1.ValueEquals(obj2);

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenEquivalentComplexCollection_ShouldBeFalse()
        {
            //Arrange
            var items1 = Fixture.CreateMany<DummyChild>().ToList();
            var items2 = Fixture.CreateMany<DummyChild>().ToList();

            var obj1 = new DummyCollection<DummyChild>(items1);
            var obj2 = new DummyCollection<DummyChild>(items2);

            //Act
            var result = obj1.ValueEquals(obj2);

            //Assert
            result.Should().BeFalse();
        }

    }


}