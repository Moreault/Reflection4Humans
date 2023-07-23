using System.Xml.Linq;

namespace Reflection4Humans.Extensions.Tests;

public partial class MemberSearchExtensionsTest
{
    [TestClass]
    public class GetAllFields : Tester
    {
        [TestMethod]
        public void WhenTypeIsNull_Throw()
        {
            //Arrange
            Type type = null!;

            //Act
            var action = () => type.GetAllFields();

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(type));
        }

        [TestMethod]
        public void WhenTypeIsNotNull_ReturnAllFields()
        {
            //Arrange

            //Act
            var result = typeof(Dummy).GetAllFields();

            //Assert
            result.Select(x => x.Name).Should().BeEquivalentTo(new List<string>
            {
                "<Id>k__BackingField", "ShadowedField", "<PrivateGetSetProperty>k__BackingField", "<GetOnlyProperty>k__BackingField", "ShadowedField", "_wasPoked", "_setOnlyValue", "_nextId"
            });
        }

        [TestMethod]
        public void WhenSearchingForAllStaticFields_ReturnAllStaticFields()
        {
            //Arrange

            //Act
            var result = typeof(Dummy).GetAllFields(x => x.IsStatic);

            //Assert
            result.Select(x => x.Name).Should().BeEquivalentTo(new List<string>
            {
                "_nextId"
            });
        }
    }

    [TestClass]
    public class GetSingleField : Tester
    {
        [TestMethod]
        public void WhenTypeIsNull_Throw()
        {
            //Arrange
            Type type = null!;
            var name = Fixture.Create<string>();

            //Act
            var action = () => type.GetSingleField(name);

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
            var action = () => typeof(Dummy).GetSingleField(name);

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(name));
        }

        [TestMethod]
        public void WhenNoFieldWithName_Throw()
        {
            //Arrange

            //Act
            var action = () => typeof(Dummy).GetSingleField(Fixture.Create<string>());

            //Assert
            action.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public void WhenMultipleFieldsWithSameName_Throw()
        {
            //Arrange

            //Act
            var action = () => typeof(Dummy).GetSingleField("ShadowedField");

            //Assert
            action.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public void WhenExactlyOneFieldWithName_ReturnIt()
        {
            //Arrange

            //Act
            var result = typeof(Dummy).GetSingleField(x => x.Name == "ShadowedField" && x.IsPublic);

            //Assert
            result.Should().NotBeNull();
        }
    }

    [TestClass]
    public class GetSingleFieldOrDefault : Tester
    {
        [TestMethod]
        public void WhenTypeIsNull_Throw()
        {
            //Arrange
            Type type = null!;
            var name = Fixture.Create<string>();

            //Act
            var action = () => type.GetSingleFieldOrDefault(name);

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
            var action = () => typeof(Dummy).GetSingleFieldOrDefault(name);

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(name));
        }

        [TestMethod]
        public void WhenNoFieldWithName_DoNotThrow()
        {
            //Arrange

            //Act
            var action = () => typeof(Dummy).GetSingleFieldOrDefault(Fixture.Create<string>());

            //Assert
            action.Should().NotThrow();
        }

        [TestMethod]
        public void WhenMultipleFieldsWithSameName_Throw()
        {
            //Arrange

            //Act
            var action = () => typeof(Dummy).GetSingleFieldOrDefault("ShadowedField");

            //Assert
            action.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public void WhenExactlyOneFieldWithName_ReturnIt()
        {
            //Arrange

            //Act
            var result = typeof(Dummy).GetSingleFieldOrDefault(x => x.Name == "ShadowedField" && x.IsPublic);

            //Assert
            result.Should().NotBeNull();
        }
    }
}