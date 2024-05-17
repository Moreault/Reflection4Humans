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
            var result = typeof(Garbage).GetAllFields();

            //Assert
            result.Select(x => x.Name).Should().BeEquivalentTo(new List<string>
            {
                "<Id>k__BackingField", "ShadowedField", "<PrivateGetSetProperty>k__BackingField", "<GetOnlyProperty>k__BackingField", "OnPublic", "OnProtected", "OnInternal", "OnPrivate", "OnStatic", "ShadowedField", "_wasPoked", "_setOnlyValue", "_nextId"
            });
        }

        [TestMethod]
        public void WhenSearchingForAllStaticFields_ReturnAllStaticFields()
        {
            //Arrange

            //Act
            var result = typeof(Garbage).GetAllFields(x => x.IsStatic);

            //Assert
            result.Select(x => x.Name).Should().BeEquivalentTo(new List<string>
            {
                "OnStatic", "_nextId"
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
            var name = Dummy.Create<string>();

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
            var action = () => typeof(Garbage).GetSingleField(name);

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(name));
        }

        [TestMethod]
        public void WhenNoFieldWithName_Throw()
        {
            //Arrange

            //Act
            var action = () => typeof(Garbage).GetSingleField(Dummy.Create<string>());

            //Assert
            action.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public void WhenMultipleFieldsWithSameName_Throw()
        {
            //Arrange

            //Act
            var action = () => typeof(Garbage).GetSingleField("ShadowedField");

            //Assert
            action.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public void WhenExactlyOneFieldWithName_ReturnIt()
        {
            //Arrange

            //Act
            var result = typeof(Garbage).GetSingleField(x => x.Name == "ShadowedField" && x.IsPublic);

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
            var name = Dummy.Create<string>();

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
            var action = () => typeof(Garbage).GetSingleFieldOrDefault(name);

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(name));
        }

        [TestMethod]
        public void WhenNoFieldWithName_DoNotThrow()
        {
            //Arrange

            //Act
            var action = () => typeof(Garbage).GetSingleFieldOrDefault(Dummy.Create<string>());

            //Assert
            action.Should().NotThrow();
        }

        [TestMethod]
        public void WhenMultipleFieldsWithSameName_Throw()
        {
            //Arrange

            //Act
            var action = () => typeof(Garbage).GetSingleFieldOrDefault("ShadowedField");

            //Assert
            action.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public void WhenExactlyOneFieldWithName_ReturnIt()
        {
            //Arrange

            //Act
            var result = typeof(Garbage).GetSingleFieldOrDefault(x => x.Name == "ShadowedField" && x.IsPublic);

            //Assert
            result.Should().NotBeNull();
        }
    }
}