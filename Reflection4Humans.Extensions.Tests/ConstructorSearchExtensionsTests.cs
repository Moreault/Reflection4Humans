namespace Reflection4Humans.Extensions.Tests;

public partial class MemberSearchExtensionsTest : Tester
{
    [TestMethod]
    public void GetAllConstructors_WhenTypeIsNull_Throw()
    {
        //Arrange
        Type type = null!;

        //Act
        var action = () => type.GetAllConstructors();

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(type));
    }

    [TestMethod]
    public void GetAllConstructors_WhenGettingAllConstructors_ReturnAllOfThem()
    {
        //Arrange

        //Act
        var result = typeof(Garbage).GetAllConstructors();

        //Assert
        result.Should().HaveCount(3);
    }

    [TestMethod]
    public void GetAllConstructors_WhenGettingAllPublicConstructors_ReturnOnlyPublicConstructors()
    {
        //Arrange

        //Act
        var result = typeof(Garbage).GetAllConstructors(x => x.IsPublic);

        //Assert
        result.Should().HaveCount(2);
    }

    [TestMethod]
    public void GetSingleConstructor_WhenMultipleConstructorsFitPredicate_Throw()
    {
        //Arrange

        //Act
        var action = () => typeof(Garbage).GetSingleConstructor(x => x.HasNoParameter());

        //Assert
        action.Should().Throw<InvalidOperationException>();
    }

    [TestMethod]
    public void GetSingleConstructor_WhenNoConstructorFitPredicate_Throw()
    {
        //Arrange

        //Act
        var action = () => typeof(Garbage).GetSingleConstructor(x => x.IsInternal());

        //Assert
        action.Should().Throw<InvalidOperationException>();
    }

    [TestMethod]
    public void GetSingleConstructor_WhenExactlyOneConstructorFitsPredicate_ReturnThatConstructor()
    {
        //Arrange

        //Act
        var result = typeof(Garbage).GetSingleConstructor(x => x.IsProtected());

        //Assert
        result.Should().NotBeNull();
    }

    [TestMethod]
    public void GetSingleConstructorOrDefault_WhenMultipleConstructorsFitPredicate_Throw()
    {
        //Arrange

        //Act
        var action = () => typeof(Garbage).GetSingleConstructorOrDefault(x => x.HasNoParameter());

        //Assert
        action.Should().Throw<InvalidOperationException>();
    }

    [TestMethod]
    public void GetSingleConstructorOrDefault_WhenNoConstructorFitPredicate_DoNotThrow()
    {
        //Arrange

        //Act
        var result = typeof(Garbage).GetSingleConstructorOrDefault(x => x.IsInternal());

        //Assert
        result.Should().BeNull();
    }

    [TestMethod]
    public void GetSingleConstructorOrDefault_WhenExactlyOneConstructorFitsPredicate_ReturnThatConstructor()
    {
        //Arrange

        //Act
        var result = typeof(Garbage).GetSingleConstructorOrDefault(x => x.IsProtected());

        //Assert
        result.Should().NotBeNull();
    }
}