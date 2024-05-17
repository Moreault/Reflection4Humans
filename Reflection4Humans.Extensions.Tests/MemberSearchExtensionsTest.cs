namespace Reflection4Humans.Extensions.Tests;

[TestClass]
public partial class MemberSearchExtensionsTest
{
    public delegate void TestEventHandler(object sender, EventArgs e);

    public class Garbage : AbstractGarbage<Garbage>
    {
        public long Id { get; }

        public new int ShadowedField;

        private string PrivateGetSetProperty { get; set; }

        private protected string GetOnlyProperty { get; } = "abc";

        public static event TestEventHandler OnStatic;
        public event TestEventHandler OnPublic;
        protected event TestEventHandler OnProtected;
        internal event TestEventHandler OnInternal;
        private event TestEventHandler OnPrivate;

        public Garbage()
        {
            Id = NextId;
        }

        private void SomeoneTouchedMe(int numberOfTimes, string message)
        {

        }

        private void SomeoneTouchedMe<T>(int numberOfTimes, string message)
        {

        }

        private void SomeoneTouchedMe(int numberOfTimes, Garbage garbage)
        {

        }

        private void SomeoneTouchedMe<T>(int numberOfTimes, Garbage garbage)
        {

        }

        private void SomeoneTouchedMe<T>(string message, T other) where T : new()
        {

        }

        public void Overload() { }
        public void Overload(string arg1) { }
        public void Overload(string arg1, int arg2) { }
        public void Overload(string arg1, int arg2, long arg3) { }
        public void Overload(string arg1, int arg2, long arg3, char arg4) { }
        public void Overload(string arg1, int arg2, long arg3, char arg4, string arg5) { }
        public void Overload(string arg1, int arg2, long arg3, char arg4, string arg5, float arg6) { }
        public void Overload(string arg1, int arg2, long arg3, char arg4, string arg5, float arg6, double arg7) { }
        public void Overload(string arg1, int arg2, long arg3, char arg4, string arg5, float arg6, double arg7, string arg8) { }
        public void Overload(string arg1, int arg2, long arg3, char arg4, string arg5, float arg6, double arg7, string arg8, int arg9) { }
        public void Overload(string arg1, int arg2, long arg3, char arg4, string arg5, float arg6, double arg7, string arg8, int arg9, char arg10) { }
    }

    public abstract class AbstractGarbage<T>
    {
        public static long NextId => _nextId++;
        private static long _nextId;

        protected int ShadowedField;

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

        protected AbstractGarbage()
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
            var predicate = Dummy.Create<Func<MemberInfo, bool>>();

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
            var result = typeof(Garbage).GetAllMembers();

            //Assert
            result.Should().HaveCount(63);
        }

        [TestMethod]
        public void WhenOnlyStaticMembers_ReturnOnlyStaticMembers()
        {
            //Arrange

            //Act
            var result = typeof(Garbage).GetAllMembers(x => x.IsStatic());

            //Assert
            result.Select(x => x.Name).Should().BeEquivalentTo(new List<string>
            {
                "add_OnStatic", "remove_OnStatic", "OnStatic", "get_NextId", "NextId", "_nextId", "Equals", "ReferenceEquals"
            });
        }

        [TestMethod]
        public void WhenSearchingForPrivateStaticMembers_ReturnOnlyPrivateStaticMembers()
        {
            //Arrange

            //Act
            var result = typeof(Garbage).GetAllMembers(x => x.IsStatic() && x.IsPrivate());

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
            var result = typeof(Garbage).GetAllMembers(x => x.IsStatic() && x.IsPublic());

            //Assert
            result.Select(x => x.Name).Should().BeEquivalentTo(new List<string>
            {
                "add_OnStatic", "remove_OnStatic", "OnStatic", "get_NextId", "NextId", "Equals", "ReferenceEquals"
            });
        }

        [TestMethod]
        public void WhenSearchingForPrivateProperties_ReturnOnlyPrivateProperties()
        {
            //Arrange

            //Act
            var result = typeof(Garbage).GetAllMembers(x => x.IsPrivate() && x.IsProperty());

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
            var result = typeof(Garbage).GetAllMembers(x => x.IsPrivate() && x.IsField());

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
            var result = typeof(Garbage).GetAllMembers(x => x.IsInstance());

            var getHashCode1 = result.First(x => x.Name == "GetHashCode");
            var getHashCode2 = result.Last(x => x.Name == "GetHashCode");

            getHashCode1.Should().BeEquivalentTo(getHashCode2);

            //Assert
            result.Select(x => x.Name).Should().BeEquivalentTo(new List<string>
            {
                "get_Id", "get_PrivateGetSetProperty", "set_PrivateGetSetProperty", "get_GetOnlyProperty", "SomeoneTouchedMe", "SomeoneTouchedMe", "SomeoneTouchedMe",
                "SomeoneTouchedMe", "SomeoneTouchedMe", "set_SetOnlyProperty", "Poke", "GetType", "MemberwiseClone", "Finalize", "ToString", "Equals", "GetHashCode",
                ".ctor", "Id", "PrivateGetSetProperty", "GetOnlyProperty", "SetOnlyProperty", "<Id>k__BackingField", "ShadowedField", "<PrivateGetSetProperty>k__BackingField",
                "<GetOnlyProperty>k__BackingField", "ShadowedField", "SomeoneTouchedMeVeryPrivately", ".ctor", "_wasPoked", "_setOnlyValue", ".ctor",
                "Overload", "Overload","Overload","Overload","Overload","Overload","Overload","Overload","Overload","Overload","Overload", 
                "OnProtected", "OnInternal", "OnPrivate", "OnPublic",
                "add_OnProtected", "add_OnInternal", "add_OnPrivate", "add_OnPublic",
                "remove_OnProtected", "remove_OnInternal", "remove_OnPrivate", "remove_OnPublic",
            });
        }

        [TestMethod]
        public void WhenGettingOnlyConstructors_ReturnAllConstructors()
        {
            //Arrange

            //Act
            var result = typeof(Garbage).GetAllMembers(x => x.IsConstructor());

            //Assert
            result.Should().HaveCount(3);
        }

        [TestMethod]
        public void WhenGettingOnlyPrivateMethods_ReturnAllPrivateMethods()
        {
            //Arrange

            //Act
            var result = typeof(Garbage).GetAllMembers(x => x.IsPrivate() && x.IsMethod());

            //Assert
            result.Select(x => x.Name).Should().BeEquivalentTo(new List<string>
            {
                "get_PrivateGetSetProperty", "set_PrivateGetSetProperty", "add_OnPrivate", "remove_OnPrivate", "SomeoneTouchedMe", "SomeoneTouchedMe", 
                "SomeoneTouchedMe", "SomeoneTouchedMe", "SomeoneTouchedMe", "SomeoneTouchedMeVeryPrivately"
            });
        }
    }

    [TestClass]
    public class GetSingleMember_Name : Tester
    {
        [TestMethod]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow(null)]
        public void WhenNameIsEmpty_Throw(string name)
        {
            //Arrange

            //Act
            var action = () => typeof(Garbage).GetSingleMember(name);

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void WhenThereIsMoreThanOneResultWithName_Throw()
        {
            //Arrange

            //Act
            var action = () => typeof(Garbage).GetSingleMember("SomeoneTouchedMe");

            //Assert
            action.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public void WhenThereIsNoResultWithName_Throw()
        {
            //Arrange

            //Act
            var action = () => typeof(Garbage).GetSingleMember("SomeoneTouchedMeRightNow");

            //Assert
            action.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public void WhenThereIsExactlyOneResultWithName_Return()
        {
            //Arrange

            //Act
            var result = typeof(Garbage).GetSingleMember("GetOnlyProperty");

            //Assert
            result.Should().NotBeNull();
        }
    }

    [TestClass]
    public class GetSingleMemberOrDefault_Name : Tester
    {
        [TestMethod]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow(null)]
        public void WhenNameIsEmpty_Throw(string name)
        {
            //Arrange

            //Act
            var action = () => typeof(Garbage).GetSingleMemberOrDefault(name);

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void WhenThereIsMoreThanOneResultWithName_Throw()
        {
            //Arrange

            //Act
            var action = () => typeof(Garbage).GetSingleMemberOrDefault("SomeoneTouchedMe");

            //Assert
            action.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public void WhenThereIsNoResultWithName_Throw()
        {
            //Arrange

            //Act
            var result = typeof(Garbage).GetSingleMemberOrDefault("SomeoneTouchedMeRightNow");

            //Assert
            result.Should().BeNull();
        }

        [TestMethod]
        public void WhenThereIsExactlyOneResultWithName_Return()
        {
            //Arrange

            //Act
            var result = typeof(Garbage).GetSingleMemberOrDefault("GetOnlyProperty");

            //Assert
            result.Should().NotBeNull();
        }
    }


    [TestClass]
    public class GetSingleMember_Predicate : Tester
    {
        [TestMethod]
        public void WhenPredicateIsEmpty_ThrowBecauseMoreThanOneMember()
        {
            //Arrange

            //Act
            var action = () => typeof(Garbage).GetSingleMember();

            //Assert
            action.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public void WhenThereIsMoreThanOneResultWithName_Throw()
        {
            //Arrange

            //Act
            var action = () => typeof(Garbage).GetSingleMember(x => x.Name == "SomeoneTouchedMe");

            //Assert
            action.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public void WhenThereIsNoResultWithName_Throw()
        {
            //Arrange

            //Act
            var action = () => typeof(Garbage).GetSingleMember(x => x.Name == "SomeoneTouchedMeRightNow");

            //Assert
            action.Should().Throw<InvalidOperationException>();
        }


        [TestMethod]
        public void WhenThereIsExactlyOneResultWithName_Return()
        {
            //Arrange

            //Act
            var result = typeof(Garbage).GetSingleMember(x => x.Name == "GetOnlyProperty");

            //Assert
            result.Should().NotBeNull();
        }
    }

    [TestClass]
    public class GetSingleMemberOrDefault_Predicate : Tester
    {
        [TestMethod]
        public void WhenPredicateIsEmpty_ThrowBecauseMoreThanOneMember()
        {
            //Arrange
            Func<MemberInfo, bool>? predicate = null;

            //Act
            var action = () => typeof(Garbage).GetSingleMemberOrDefault(predicate);

            //Assert
            action.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public void WhenThereIsMoreThanOneResultWithName_Throw()
        {
            //Arrange

            //Act
            var action = () => typeof(Garbage).GetSingleMemberOrDefault(x => x.Name == "SomeoneTouchedMe");

            //Assert
            action.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public void WhenThereIsNoResultWithName_Throw()
        {
            //Arrange

            //Act
            var result = typeof(Garbage).GetSingleMemberOrDefault(x => x.Name == "SomeoneTouchedMeRightNow");

            //Assert
            result.Should().BeNull();
        }

        [TestMethod]
        public void WhenThereIsExactlyOneResultWithName_Return()
        {
            //Arrange

            //Act
            var result = typeof(Garbage).GetSingleMemberOrDefault(x => x.Name == "GetOnlyProperty");

            //Assert
            result.Should().NotBeNull();
        }
    }
}