namespace Reflection4Humans.Extensions.Tests;

[TestClass]
public partial class MemberSearchExtensionsTest
{
    public class Dummy : AbstractDummy<Dummy>
    {
        public long Id { get; }

        private string PrivateGetSetProperty { get; set; }

        private protected string GetOnlyProperty { get; } = "abc";

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

        protected internal int SetOnlyProperty
        {
            set => _setOnlyValue = value;
        }
        private int _setOnlyValue;

        protected string Poke()
        {
            SomeoneTouchedMeVeryPrivately();
            return "Hey!";
        }

        private void SomeoneTouchedMeVeryPrivately() => _wasPoked = true;

        protected AbstractDummy()
        {

        }
    }

    [TestClass]
    public class GetAllMembers : Tester
    {
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
            result.Should().HaveCount(50);
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
                "<Id>k__BackingField", "<PrivateGetSetProperty>k__BackingField", "<GetOnlyProperty>k__BackingField", "_wasPoked", "_setOnlyValue", "_nextId"
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
                "get_Id", "get_PrivateGetSetProperty", "set_PrivateGetSetProperty", "get_GetOnlyProperty", "SomeoneTouchedMe",
                "SomeoneTouchedMe", "SomeoneTouchedMe", "SomeoneTouchedMe", "SomeoneTouchedMe", "set_SetOnlyProperty", "Poke",
                "GetType", "MemberwiseClone", "Finalize", "ToString", "Equals", "GetHashCode", ".ctor", "Id", "PrivateGetSetProperty",
                "GetOnlyProperty", "SetOnlyProperty", "<Id>k__BackingField", "<PrivateGetSetProperty>k__BackingField",
                "<GetOnlyProperty>k__BackingField", "SomeoneTouchedMeVeryPrivately", ".ctor", "_wasPoked", "_setOnlyValue", ".ctor"
            });
        }
    }

    [TestClass]
    public class GetSingleMember : Tester
    {
        [TestMethod]
        public void WhenThereIsMoreThanOneResultWithName_Throw()
        {
            //Arrange

            //Act
            var action = () => typeof(Dummy).GetSingleMember("SomeoneTouchedMe");

            //Assert
            action.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public void WhenThereIsNoResultWithName_Throw()
        {
            //Arrange

            //Act
            var action = () => typeof(Dummy).GetSingleMember("SomeoneTouchedMeRightNow");

            //Assert
            action.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public void WhenThereIsExactlyOneResultWithName_Return()
        {
            //Arrange

            //Act
            var result = typeof(Dummy).GetSingleMember("GetOnlyProperty");

            //Assert
            result.Should().NotBeNull();
        }
    }

    [TestClass]
    public class GetSingleMemberOrDefault : Tester
    {
        [TestMethod]
        public void WhenThereIsMoreThanOneResultWithName_Throw()
        {
            //Arrange

            //Act
            var action = () => typeof(Dummy).GetSingleMemberOrDefault("SomeoneTouchedMe");

            //Assert
            action.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public void WhenThereIsNoResultWithName_Throw()
        {
            //Arrange

            //Act
            var result = typeof(Dummy).GetSingleMemberOrDefault("SomeoneTouchedMeRightNow");

            //Assert
            result.Should().BeNull();
        }

        [TestMethod]
        public void WhenThereIsExactlyOneResultWithName_Return()
        {
            //Arrange

            //Act
            var result = typeof(Dummy).GetSingleMemberOrDefault("GetOnlyProperty");

            //Assert
            result.Should().NotBeNull();
        }
    }
}