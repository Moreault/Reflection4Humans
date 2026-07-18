namespace Reflection4Humans.ValueEquality.Tests;

/// <summary>
/// Pins down the record-like Shallow / deep Recursive semantics and the guarantee that a value compares the same whether it is standalone, 
/// a member, or an element of a collection (position independence).
/// </summary>
[TestClass]
public sealed class ValueEqualityConsistencyTests
{
    /// <summary>
    /// Native Equals already reflects value equality (record whose members are all value/atomic).
    /// </summary>
    public record ValuePoint
    {
        public int X { get; init; }
        public int Y { get; init; }
    }

    /// <summary>
    /// Native Equals does NOT reflect value equality: the record compares its List member by reference.
    /// </summary>
    public record Holder
    {
        public int Id { get; init; }
        public List<int> Values { get; init; } = [];
    }

    public record Wrapper
    {
        public Holder Holder { get; init; } = new();
    }

    /// <summary>
    /// A collection type that exposes no comparable members of its own (like a domain collection wrapper).
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class Bag<T> : IEnumerable<T>
    {
        private readonly List<T> _items;
        public Bag(params T[] items) => _items = items.ToList();
        public IEnumerator<T> GetEnumerator() => _items.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    //Same content, but the two Values lists are distinct instances.
    private static Holder Holder1() => new() { Id = 7, Values = [1, 2, 3] };
    private static Holder Holder2() => new() { Id = 7, Values = [1, 2, 3] };

    [TestClass]
    public sealed class RecordLikeSemantics
    {
        [TestMethod]
        [DataRow(Depth.Shallow)]
        [DataRow(Depth.Recursive)]
        public void WhenValueRecordHasEqualContent_AreEqualRegardlessOfDepth(Depth depth)
        {
            var a = new ValuePoint { X = 1, Y = 2 };
            var b = new ValuePoint { X = 1, Y = 2 };

            var result = a.ValueEquals(b, new ValueEqualityOptions { Depth = depth });

            result.Should().BeTrue();
        }

        [TestMethod]
        [DataRow(Depth.Shallow, false)]
        [DataRow(Depth.Recursive, true)]
        public void WhenRecordHoldsCopiedCollection_EqualityDependsOnDepth(Depth depth, bool expected)
        {
            var result = Holder1().ValueEquals(Holder2(), new ValueEqualityOptions { Depth = depth });

            result.Should().Be(expected);
        }

        [TestMethod]
        [DataRow(Depth.Shallow, false)]
        [DataRow(Depth.Recursive, true)]
        public void WhenNestedObjectMemberHasCopiedCollection_EqualityDependsOnDepth(Depth depth, bool expected)
        {
            var a = new Wrapper { Holder = Holder1() };
            var b = new Wrapper { Holder = Holder2() };

            var result = a.ValueEquals(b, new ValueEqualityOptions { Depth = depth });

            result.Should().Be(expected);
        }

        [TestMethod]
        [DataRow(Depth.Shallow)]
        [DataRow(Depth.Recursive)]
        public void WhenCollectionElementsShareReferences_AreEqualRegardlessOfDepth(Depth depth)
        {
            var holder = Holder1();
            var a = new Bag<Holder>(holder);
            var b = new Bag<Holder>(holder);

            var result = a.ValueEquals(b, new ValueEqualityOptions { Depth = depth });

            result.Should().BeTrue();
        }

        [TestMethod]
        [DataRow(Depth.Shallow)]
        [DataRow(Depth.Recursive)]
        public void WhenCollectionsHaveDifferentContent_AreNotEqualRegardlessOfDepth(Depth depth)
        {
            var a = new Bag<Holder>(new Holder { Id = 1, Values = [1] });
            var b = new Bag<Holder>(new Holder { Id = 2, Values = [2] });

            var result = a.ValueEquals(b, new ValueEqualityOptions { Depth = depth });

            result.Should().BeFalse();
        }
    }

    /// <summary>
    /// The regression at the heart of this change: comparing a value directly must give the same answer as comparing it wrapped in a single-element collection, for every depth.
    /// </summary>
    [TestClass]
    public sealed class PositionIndependence
    {
        [TestMethod]
        [DataRow(Depth.Shallow)]
        [DataRow(Depth.Recursive)]
        public void WhenReferenceHoldingRecord_StandaloneAndInCollectionAgree(Depth depth)
        {
            var options = new ValueEqualityOptions { Depth = depth };

            var standalone = Holder1().ValueEquals(Holder2(), options);
            var inCollection = new Bag<Holder>(Holder1()).ValueEquals(new Bag<Holder>(Holder2()), options);

            inCollection.Should().Be(standalone);
        }

        [TestMethod]
        [DataRow(Depth.Shallow)]
        [DataRow(Depth.Recursive)]
        public void WhenValueRecord_StandaloneAndInCollectionAgree(Depth depth)
        {
            var options = new ValueEqualityOptions { Depth = depth };
            var a = new ValuePoint { X = 1, Y = 2 };
            var b = new ValuePoint { X = 1, Y = 2 };

            var standalone = a.ValueEquals(b, options);
            var inCollection = new Bag<ValuePoint>(a).ValueEquals(new Bag<ValuePoint>(b), options);

            inCollection.Should().Be(standalone);
        }
    }

    [TestClass]
    public sealed class TopLevelPrimitives
    {
        [TestMethod]
        public void WhenStringsDifferInContent_AreNotEqual()
        {
            //Regression: strings must compare by text, not by an incidental member such as Length.
            "abc".ValueEquals("abd").Should().BeFalse();
        }

        [TestMethod]
        public void WhenStringsHaveSameContent_AreEqual()
        {
            "abc".ValueEquals("abc").Should().BeTrue();
        }

        [TestMethod]
        public void WhenStringsDifferOnlyInCasingAndCasingIsIgnored_AreEqual()
        {
            "abc".ValueEquals("ABC", new ValueEqualityOptions { StringComparison = StringComparison.OrdinalIgnoreCase }).Should().BeTrue();
        }

        [TestMethod]
        public void WhenNumbersOfDifferentTypesShareValue_AreEqual()
        {
            5.ValueEquals(5L).Should().BeTrue();
        }

        [TestMethod]
        public void WhenNumbersOfDifferentTypesDifferInValue_AreNotEqual()
        {
            5.ValueEquals(6L).Should().BeFalse();
        }
    }

    [TestClass]
    public sealed class EqualsAndHashCodeAgree
    {
        [TestMethod]
        public void WhenEqualByValueUnderRecursiveDepth_ProduceEqualHashCodes()
        {
            var a = Holder1();
            var b = Holder2();

            a.ValueEquals(b, new ValueEqualityOptions { Depth = Depth.Recursive }).Should().BeTrue();
            a.GetValueHashCode().Should().Be(b.GetValueHashCode());
        }

        [TestMethod]
        public void WhenNotEqualByValueUnderShallowDepth_ProduceDifferentHashCodes()
        {
            var a = Holder1();
            var b = Holder2();

            a.ValueEquals(b, new ValueEqualityOptions { Depth = Depth.Shallow }).Should().BeFalse();
            a.GetValueHashCode(Depth.Shallow).Should().NotBe(b.GetValueHashCode(Depth.Shallow));
        }

        [TestMethod]
        public void WhenNumbersOfDifferentTypesShareValue_ProduceEqualHashCodes()
        {
            5.GetValueHashCode().Should().Be(5L.GetValueHashCode());
        }
    }
}
