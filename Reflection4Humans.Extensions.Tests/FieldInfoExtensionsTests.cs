namespace Reflection4Humans.Extensions.Tests;

[TestClass]
public sealed class FieldInfoExtensionsTests
{
    private string Property { get; set; }

    private string NotAutomatic => _notAutomatic;
    private string _notAutomatic;

    private string _notBacking;


    [TestMethod]
    public void IsAutomaticBackingField_WhenFieldIsNull_Throw()
    {
        //Arrange
        FieldInfo field = null!;

        //Act
        var action = () => field.IsAutomaticBackingField();

        //Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void IsAutomaticBackingField_WhenIsAnAutomaticBackField_True()
    {
        //Arrange
        var field = typeof(FieldInfoExtensionsTests).GetSingleField($"<{nameof(Property)}>k__BackingField");

        //Act
        var result = field.IsAutomaticBackingField();

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void IsAutomaticBackingField_WhenIsNotAnAutomaticBackingFieldButARegularBackingField_False()
    {
        //Arrange
        var field = typeof(FieldInfoExtensionsTests).GetSingleField(nameof(_notAutomatic));

        //Act
        var result = field.IsAutomaticBackingField();

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void IsAutomaticBackingField_WhenIsNotBackingFieldAtAll_False()
    {
        //Arrange
        var field = typeof(FieldInfoExtensionsTests).GetSingleField(nameof(_notBacking));

        //Act
        var result = field.IsAutomaticBackingField();

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void IsBackingField_WhenFieldIsNull_Throw()
    {
        //Arrange
        FieldInfo field = null!;

        //Act
        var action = () => field.IsBackingField();

        //Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void IsBackingField_WhenIsAutomaticBackingField_True()
    {
        //Arrange
        var field = typeof(FieldInfoExtensionsTests).GetSingleField($"<{nameof(Property)}>k__BackingField");

        //Act
        var result = field.IsBackingField();

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void IsBackingField_WhenHasBackingFieldConventionButNoPropertyOfSimilarName_False()
    {
        //Arrange
        var field = typeof(FieldInfoExtensionsTests).GetSingleField(nameof(_notBacking));

        //Act
        var result = field.IsBackingField();

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void IsBackingField_WhenHasBackingFieldConventionAndPropertyWithSameName_True()
    {
        //Arrange
        var field = typeof(FieldInfoExtensionsTests).GetSingleField(nameof(_notAutomatic));

        //Act
        var result = field.IsBackingField();

        //Assert
        result.Should().BeTrue();
    }

}