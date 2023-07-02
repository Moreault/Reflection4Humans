namespace Reflection4Humans.Extensions.Tests;

public partial class MemberSearchExtensionsTest
{
    [TestClass]
    public class GetAllProperties : Tester
    {
        //TODO Test
        [TestMethod]
        public void WhenTypeIsNull_Throw()
        {
            //Arrange
            Type type = null!;

            //Act
            var action = () => type.GetAllProperties();

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(type));
        }

        [TestMethod]
        public void WhenGettingSetOnlies_ReturnOnlySetOnlies()
        {
            //Arrange

            //Act
            var result = typeof(Dummy).GetAllProperties(x => x.IsSet && !x.IsGet);

            //Assert
            result.Select(x => x.Name).Should().BeEquivalentTo(new List<string>
            {
                "SetOnlyProperty"
            });
        }

        [TestMethod]
        public void WhenGettingGetOnlies_ReturnOnlyGetOnlies()
        {
            //Arrange

            //Act
            var result = typeof(Dummy).GetAllProperties(x => x.IsGet && !x.IsSet);

            //Assert
            result.Select(x => x.Name).Should().BeEquivalentTo(new List<string>
            {
                "Id", "GetOnlyProperty", "NextId"
            });
        }

        [TestMethod]
        public void WhenGettingAnyWithSet_ReturnAllProprtiesWithSets()
        {
            //Arrange

            //Act

            //Assert
        }

        [TestMethod]
        public void WhenGettingAnyWithGet_ReturnAllProprtiesWithGets()
        {
            //Arrange

            //Act

            //Assert
        }

        [TestMethod]
        public void WhenGettingAllPropertiesWithGetAndSet_ReturnAllProprtiesWithBothGetAndSet()
        {
            //Arrange

            //Act

            //Assert
        }
    }

    [TestClass]
    public class GetSingleProperty : Tester
    {
        [TestMethod]
        public void WhenTypeIsNull_Throw()
        {
            //Arrange
            Type type = null!;
            var name = Fixture.Create<string>();

            //Act
            var action = () => type.GetSingleProperty(name);

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(type));
        }

        [TestMethod]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow(null)]
        public void WhenNameIsNullOrEmpty_Throw(string name)
        {
            //Arrange

            //Act
            var action = () => typeof(Dummy).GetSingleProperty(name);

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(name));
        }

        [TestMethod]
        public void WhenLookingForPropertyWithGetButThereIsNone_Throw()
        {
            //Arrange

            //Act
            var action = () => typeof(Dummy).GetSingleProperty("SetOnlyProperty", x => x.IsGet);

            //Assert
            action.Should().Throw<Exception>();
        }

        [TestMethod]
        public void WhenLookingForPropertyWithGetButThereIsExactlyOne_ReturnTheOne()
        {
            //Arrange

            //Act
            var result = typeof(Dummy).GetSingleProperty("GetOnlyProperty", x => x.IsGet && !x.IsSet);

            //Assert
            result.Should().NotBeNull();
        }

        [TestMethod]
        public void WhenLookingForPropertyWithSetButThereIsExactlyOne_Throw()
        {
            //Arrange

            //Act
            var result = typeof(Dummy).GetSingleProperty("SetOnlyProperty", x => x.IsSet);

            //Assert
            result.Should().NotBeNull();
        }
    }

    [TestClass]
    public class GetSinglePropertyOrDefault : Tester
    {
        [TestMethod]
        public void WhenTypeIsNull_Throw()
        {
            //Arrange
            Type type = null!;
            var name = Fixture.Create<string>();

            //Act
            var action = () => type.GetSinglePropertyOrDefault(name);

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(type));
        }

        [TestMethod]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow(null)]
        public void WhenNameIsNullOrEmpty_Throw(string name)
        {
            //Arrange

            //Act
            var action = () => typeof(Dummy).GetSinglePropertyOrDefault(name);

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(name));
        }

        [TestMethod]
        public void WhenLookingForPropertyWithGetButThereIsNone_Throw()
        {
            //Arrange

            //Act
            var result = typeof(Dummy).GetSinglePropertyOrDefault("SetOnlyProperty", x => x.IsGet);

            //Assert
            result.Should().BeNull();
        }

        [TestMethod]
        public void WhenLookingForPropertyWithGetButThereIsExactlyOne_ReturnTheOne()
        {
            //Arrange

            //Act
            var result = typeof(Dummy).GetSinglePropertyOrDefault("GetOnlyProperty", x => x.IsGet && !x.IsSet);

            //Assert
            result.Should().NotBeNull();
        }

        [TestMethod]
        public void WhenLookingForPropertyWithSetButThereIsExactlyOne_Throw()
        {
            //Arrange

            //Act
            var result = typeof(Dummy).GetSinglePropertyOrDefault("SetOnlyProperty", x => x.IsSet);

            //Assert
            result.Should().NotBeNull();
        }
    }
}