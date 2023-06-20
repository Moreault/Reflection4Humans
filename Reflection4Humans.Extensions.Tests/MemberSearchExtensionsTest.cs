namespace Reflection4Humans.Extensions.Tests;

[TestClass]
public class MemberSearchExtensionsTest
{
    public class Dummy : AbstractDummy<Dummy>
    {
        public long Id { get; }

        private string PrivateGetSetProperty { get; set; }

        public Dummy()
        {
            Id = NextId;
        }

        private void SomeoneTouchedMe(int numberOfTimes, string message)
        {

        }

        private void SomeoneTouchedMe<T>(int numberOfTimes, string message)
        {

        }

        private void SomeoneTouchedMe(int numberOfTimes, Dummy dummy)
        {

        }

        private void SomeoneTouchedMe<T>(int numberOfTimes, Dummy dummy)
        {

        }

        private void SomeoneTouchedMe<T>(string message, T other) where T : new()
        {

        }
    }

    public abstract class AbstractDummy<T>
    {
        public static long NextId => _nextId++;
        private static long _nextId;

        private bool _wasPoked;

        protected string Poke()
        {
            SomeoneTouchedMeVeryPrivately();
            return "Hey!";
        }

        private void SomeoneTouchedMeVeryPrivately() => _wasPoked = true;
    }

    [TestClass]
    public class GetAllMembers : Tester
    {
        //TODO Test
        [TestMethod]
        public void WhenTypeIsNull_Throw()
        {
            //Arrange
            Type type = null!;
            var predicate = Fixture.Create<Func<MemberSearchOptions, bool>>();

            //Act
            var action = () => type.GetAllMembers(predicate);

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(type));
        }

        [TestMethod]
        public void WhenNoPredicate_ReturnAllMembers()
        {
            //Arrange

            //Act
            var result = typeof(Dummy).GetAllMembers();

            //Assert
            result.Should().HaveCount(42);
        }

        [TestMethod]
        public void WhenOnlyStaticMembers_ReturnOnlyStaticMembers()
        {
            //Arrange

            //Act
            var result = typeof(Dummy).GetAllMembers(x => x.IsStatic);

            //Assert
            result.Select(x => x.Name).Should().BeEquivalentTo(new List<string>
            {
                "get_NextId", "NextId", "_nextId", "Equals", "ReferenceEquals"
            });
        }

        [TestMethod]
        public void WhenSearchingForPrivateStaticMembers_ReturnOnlyPrivateStaticMembers()
        {
            //Arrange

            //Act
            var result = typeof(Dummy).GetAllMembers(x => x.IsStatic && x.IsPrivate);

            //Assert
            result.Select(x => x.Name).Should().BeEquivalentTo(new List<string>
            {
                "_nextId"
            });
        }

        [TestMethod]
        public void WhenSearchingForPublicStaticMembers_ReturnOnlyPublicStaticMembers()
        {
            //Arrange

            //Act
            var result = typeof(Dummy).GetAllMembers(x => x.IsStatic && x.IsPublic);

            //Assert
            result.Select(x => x.Name).Should().BeEquivalentTo(new List<string>
            {
                "get_NextId", "NextId", "Equals", "ReferenceEquals"
            });
        }

        [TestMethod]
        public void WhenSearchingForPrivateProperties_ReturnOnlyPrivateProperties()
        {
            //Arrange

            //Act
            var result = typeof(Dummy).GetAllMembers(x => x.IsPrivate && x.IsProperty);

            //Assert
            result.Select(x => x.Name).Should().BeEquivalentTo(new List<string>
            {
                "PrivateGetSetProperty"
            });
        }

        [TestMethod]
        public void WhenSearchingForPrivateFields_ReturnOnlyPrivateFields()
        {
            //Arrange

            //Act
            var result = typeof(Dummy).GetAllMembers(x => x.IsPrivate && x.IsField);

            //Assert
            result.Select(x => x.Name).Should().BeEquivalentTo(new List<string>
            {
                "<Id>k__BackingField", "<PrivateGetSetProperty>k__BackingField", "_wasPoked", "_nextId"
            });
        }

        [TestMethod]
        public void WhenOnlyInstanceMembers_ReturnOnlyInstanceMembers()
        {
            //Arrange

            //Act
            var result = typeof(Dummy).GetAllMembers(x => x.IsInstance);

            var getHashCode1 = result.First(x => x.Name == "GetHashCode");
            var getHashCode2 = result.Last(x => x.Name == "GetHashCode");

            getHashCode1.Should().BeEquivalentTo(getHashCode2);

            //Assert
            result.Select(x => x.Name).Should().BeEquivalentTo(new List<string>
            {
                "get_Id", "get_PrivateGetSetProperty", "set_PrivateGetSetProperty", "SomeoneTouchedMe", "SomeoneTouchedMe", "SomeoneTouchedMe",
                "SomeoneTouchedMe", "SomeoneTouchedMe", "Poke", "GetType", "MemberwiseClone", "Finalize", "ToString", "Equals", "GetHashCode", ".ctor",
                "Id", "PrivateGetSetProperty", "<Id>k__BackingField", "<PrivateGetSetProperty>k__BackingField", "SomeoneTouchedMeVeryPrivately", ".ctor",
                "_wasPoked", ".ctor"
            });
        }
    }

    [TestClass]
    public class GetSingleMethod : Tester
    {
        //TODO Test
        [TestMethod]
        public void WhenGetFirstTouchWithCorrectParameters_ReturnFirstTouchMethod()
        {
            //Arrange

            //Act
            var result = typeof(Dummy).GetSingleMethod("SomeoneTouchedMe", x => x.HasParameters<int, string>() && !x.IsGeneric);

            //Assert
            result.Should().NotBeNull();
        }
    }
}