namespace Reflection4Humans.Extensions.Tests;

public partial class MemberSearchExtensionsTest
{
    [TestMethod]
    public void GetAllFields_WhenTypeIsNull_Throw()
    {
        //Arrange
        Type type = null!;

        //Act
        var action = () => type.GetAllFields();

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(type));
    }

    [TestMethod]
    public void GetAllFields_WhenTypeIsNotNull_ReturnAllFields()
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
    public void GetAllFields_WhenSearchingForAllStaticFields_ReturnAllStaticFields()
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

    [TestMethod]
    public void GetSingleField_WhenTypeIsNull_Throw()
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
    public void GetSingleField_WhenNameIsNullOrEmpty_Throw(string name)
    {
        //Arrange

        //Act
        var action = () => typeof(Garbage).GetSingleField(name);

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(name));
    }

    [TestMethod]
    public void GetSingleField_WhenNoFieldWithName_Throw()
    {
        //Arrange

        //Act
        var action = () => typeof(Garbage).GetSingleField(Dummy.Create<string>());

        //Assert
        action.Should().Throw<InvalidOperationException>();
    }

    [TestMethod]
    public void GetSingleField_WhenMultipleFieldsWithSameName_Throw()
    {
        //Arrange

        //Act
        var action = () => typeof(Garbage).GetSingleField("ShadowedField");

        //Assert
        action.Should().Throw<InvalidOperationException>();
    }

    [TestMethod]
    public void GetSingleField_WhenExactlyOneFieldWithName_ReturnIt()
    {
        //Arrange

        //Act
        var result = typeof(Garbage).GetSingleField(x => x.Name == "ShadowedField" && x.IsPublic);

        //Assert
        result.Should().NotBeNull();
    }

    [TestMethod]
    public void GetSingleFieldOrDefault_WhenTypeIsNull_Throw()
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
    public void GetSingleFieldOrDefault_WhenNameIsNullOrEmpty_Throw(string name)
    {
        //Arrange

        //Act
        var action = () => typeof(Garbage).GetSingleFieldOrDefault(name);

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(name));
    }

    [TestMethod]
    public void GetSingleFieldOrDefault_WhenNoFieldWithName_DoNotThrow()
    {
        //Arrange

        //Act
        var action = () => typeof(Garbage).GetSingleFieldOrDefault(Dummy.Create<string>());

        //Assert
        action.Should().NotThrow();
    }

    [TestMethod]
    public void GetSingleFieldOrDefault_WhenMultipleFieldsWithSameName_Throw()
    {
        //Arrange

        //Act
        var action = () => typeof(Garbage).GetSingleFieldOrDefault("ShadowedField");

        //Assert
        action.Should().Throw<InvalidOperationException>();
    }

    [TestMethod]
    public void GetSingleFieldOrDefault_WhenExactlyOneFieldWithName_ReturnIt()
    {
        //Arrange

        //Act
        var result = typeof(Garbage).GetSingleFieldOrDefault(x => x.Name == "ShadowedField" && x.IsPublic);

        //Assert
        result.Should().NotBeNull();
    }




    [TestMethod]
    public void HasField_WhenTypeIsNull_Throw()
    {
        //Arrange
        Type type = null!;
        var name = Dummy.Create<string>();

        //Act
        var action = () => type.HasField(name);

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(type));
    }

    [TestMethod]
    [DataRow("")]
    [DataRow(" ")]
    [DataRow(null)]
    public void HasField_WhenNameIsNullOrEmpty_Throw(string name)
    {
        //Arrange

        //Act
        var action = () => typeof(Garbage).HasField(name);

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(name));
    }

    [TestMethod]
    public void HasField_WhenNoFieldWithName_False()
    {
        //Arrange

        //Act
        var result = typeof(Garbage).HasField(Dummy.Create<string>());

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void HasField_WhenMultipleFieldsWithSameName_True()
    {
        //Arrange

        //Act
        var result = typeof(Garbage).HasField("ShadowedField");

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void HasField_WhenExactlyOneFieldWithName_True()
    {
        //Arrange

        //Act
        var result = typeof(Garbage).HasField(x => x.Name == "ShadowedField" && x.IsPublic);

        //Assert
        result.Should().BeTrue();
    }
}