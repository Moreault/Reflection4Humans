namespace Reflection4Humans.Extensions.Tests;

public partial class MemberSearchExtensionsTest
{
    [TestMethod]
    public void GetAllMethods_WhenTypeIsNull_Throw()
    {
        //Arrange
        Type type = null!;

        //Act
        var action = () => type.GetAllMethods();

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(type));
    }

    [TestMethod]
    public void GetAllMethods_WhenGettingOnlyGenericMethods_ReturnOnlyGenerics()
    {
        //Arrange

        //Act
        var result = typeof(Garbage).GetAllMethods(x => x.IsGenericMethod);

        //Assert
        result.Select(x => x.Name).Should().BeEquivalentTo(new List<string>
        {
            "SomeoneTouchedMe", "SomeoneTouchedMe", "SomeoneTouchedMe"
        });
    }

    [TestMethod]
    [DataRow("")]
    [DataRow(" ")]
    [DataRow(null)]
    public void GetSingleMethod_WhenNameIsEmpty_Throw(string name)
    {
        //Arrange

        //Act
        var action = () => typeof(Garbage).GetSingleMethod(name);

        //Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void GetSingleMethod_WhenThereAreMethodsWithSameName_Throw()
    {
        //Arrange

        //Act
        var action = () => typeof(Garbage).GetSingleMethod("SomeoneTouchedMe");

        //Assert
        action.Should().Throw<InvalidOperationException>();
    }

    [TestMethod]
    public void GetSingleMethod_WhenThereAreNoMethodsWithName_Throw()
    {
        //Arrange

        //Act
        var action = () => typeof(Garbage).GetSingleMethod("SomethingTouchedMe");

        //Assert
        action.Should().Throw<InvalidOperationException>();
    }

    [TestMethod]
    public void GetSingleMethod_WhenGetFirstTouchWithCorrectParameters_ReturnFirstTouchMethod()
    {
        //Arrange

        //Act
        var result = typeof(Garbage).GetSingleMethod(x => x.Name == "SomeoneTouchedMe" && x.HasParameters<int, string>() && !x.IsGenericMethod);

        //Assert
        result.Should().NotBeNull();
    }

    [TestMethod]
    public void GetSingleMethod_WhenGetSecondTouchWithCorrectParameters_ReturnSecondTouchMethod()
    {
        //Arrange

        //Act
        var result = typeof(Garbage).GetSingleMethod(x => x.Name == "SomeoneTouchedMe" && x.HasParameters<int, string>() && x.IsGenericMethod);

        //Assert
        result.Should().NotBeNull();
    }

    [TestMethod]
    public void GetSingleMethod_WhenGetThirdTouchWithCorrectParameters_ReturnThirdTouchMethod()
    {
        //Arrange

        //Act
        var result = typeof(Garbage).GetSingleMethod(x => x.Name == "SomeoneTouchedMe" && x.HasParameters<int, Garbage>() && !x.IsGenericMethod);

        //Assert
        result.Should().NotBeNull();
    }

    [TestMethod]
    public void GetSingleMethod_WhenGetFourthTouchWithCorrectParameters_ReturnFourthTouchMethod()
    {
        //Arrange

        //Act
        var result = typeof(Garbage).GetSingleMethod(x => x.Name == "SomeoneTouchedMe" && x.HasParameters<int, Garbage>() && x.IsGenericMethod);

        //Assert
        result.Should().NotBeNull();
    }

    [TestMethod]
    public void GetSingleMethod_WhenGettingMethodOverloadWithZeroParameters_ReturnMethod()
    {
        //Arrange

        //Act
        var result = typeof(Garbage).GetSingleMethod(x => x.Name == nameof(Garbage.Overload) && x.HasNoParameter());

        //Assert
        result.Should().NotBeNull();
    }

    [TestMethod]
    public void GetSingleMethod_WhenGettingMethodOverloadWithOneParameterByExactType_ReturnMethod()
    {
        //Arrange

        //Act
        var result = typeof(Garbage).GetSingleMethod(x => x.Name == nameof(Garbage.Overload) && x.HasParameters<string>());

        //Assert
        result.Should().NotBeNull();
    }

    [TestMethod]
    public void GetSingleMethod_WhenGettingMethodOverloadWithTwoParametersByExactType_ReturnMethod()
    {
        //Arrange

        //Act
        var result = typeof(Garbage).GetSingleMethod(x => x.Name == nameof(Garbage.Overload) && x.HasParameters<string, int>());

        //Assert
        result.Should().NotBeNull();
    }

    [TestMethod]
    public void GetSingleMethod_WhenGettingMethodOverloadWithThreeParametersByExactType_ReturnMethod()
    {
        //Arrange

        //Act
        var result = typeof(Garbage).GetSingleMethod(x => x.Name == nameof(Garbage.Overload) && x.HasParameters<string, int, long>());

        //Assert
        result.Should().NotBeNull();
    }

    [TestMethod]
    public void GetSingleMethod_WhenGettingMethodOverloadWithFourParameters_ReturnMethod()
    {
        //Arrange

        //Act
        var result = typeof(Garbage).GetSingleMethod(x => x.Name == nameof(Garbage.Overload) && x.HasParameters<string, int, long, char>());

        //Assert
        result.Should().NotBeNull();
    }

    [TestMethod]
    public void GetSingleMethod_WhenGettingMethodOverloadWithFiveParametersByExactType_ReturnMethod()
    {
        //Arrange

        //Act
        var result = typeof(Garbage).GetSingleMethod(x => x.Name == nameof(Garbage.Overload) && x.HasParameters<string, int, long, char, string>());

        //Assert
        result.Should().NotBeNull();
    }

    [TestMethod]
    public void GetSingleMethod_WhenGettingMethodOverloadWithSixParametersByExactType_ReturnMethod()
    {
        //Arrange

        //Act
        var result = typeof(Garbage).GetSingleMethod(x => x.Name == nameof(Garbage.Overload) && x.HasParameters<string, int, long, char, string, float>());

        //Assert
        result.Should().NotBeNull();
    }

    [TestMethod]
    public void GetSingleMethod_WhenGettingMethodOverloadWithSevenParametersByExactType_ReturnMethod()
    {
        //Arrange

        //Act
        var result = typeof(Garbage).GetSingleMethod(x => x.Name == nameof(Garbage.Overload) && x.HasParameters<string, int, long, char, string, float, double>());

        //Assert
        result.Should().NotBeNull();
    }

    [TestMethod]
    public void GetSingleMethod_WhenGettingMethodOverloadWithEightParametersByExactType_ReturnMethod()
    {
        //Arrange

        //Act
        var result = typeof(Garbage).GetSingleMethod(x => x.Name == nameof(Garbage.Overload) && x.HasParameters<string, int, long, char, string, float, double, string>());

        //Assert
        result.Should().NotBeNull();
    }

    [TestMethod]
    public void GetSingleMethod_WhenGettingMethodOverloadWithNineParametersByExactType_ReturnMethod()
    {
        //Arrange

        //Act
        var result = typeof(Garbage).GetSingleMethod(x => x.Name == nameof(Garbage.Overload) && x.HasParameters<string, int, long, char, string, float, double, string, int>());

        //Assert
        result.Should().NotBeNull();
    }

    [TestMethod]
    public void GetSingleMethod_WhenGettingMethodOverloadWithTenParametersByExactType_ReturnMethod()
    {
        //Arrange

        //Act
        var result = typeof(Garbage).GetSingleMethod(x => x.Name == nameof(Garbage.Overload) && x.HasParameters<string, int, long, char, string, float, double, string, int, char>());

        //Assert
        result.Should().NotBeNull();
    }

    [TestMethod]
    public void GetSingleMethod_WhenGettingMethodOverloadWithOneParameterByCount_ReturnMethod()
    {
        //Arrange

        //Act
        var result = typeof(Garbage).GetSingleMethod(x => x.Name == nameof(Garbage.Overload) && x.HasParameters(1));

        //Assert
        result.Should().NotBeNull();
    }

    [TestMethod]
    public void GetSingleMethod_WhenGettingMethodOverloadWithTwoParametersByCount_ReturnMethod()
    {
        //Arrange

        //Act
        var result = typeof(Garbage).GetSingleMethod(x => x.Name == nameof(Garbage.Overload) && x.HasParameters(2));

        //Assert
        result.Should().NotBeNull();
    }

    [TestMethod]
    public void GetSingleMethod_WhenGettingMethodOverloadWithThreeParametersByCount_ReturnMethod()
    {
        //Arrange

        //Act
        var result = typeof(Garbage).GetSingleMethod(x => x.Name == nameof(Garbage.Overload) && x.HasParameters(3));

        //Assert
        result.Should().NotBeNull();
    }

    [TestMethod]
    public void GetSingleMethod_WhenGettingMethodOverloadWithFourParametersByCount_ReturnMethod()
    {
        //Arrange

        //Act
        var result = typeof(Garbage).GetSingleMethod(x => x.Name == nameof(Garbage.Overload) && x.HasParameters(4));

        //Assert
        result.Should().NotBeNull();
    }

    [TestMethod]
    public void GetSingleMethod_WhenGettingMethodOverloadWithFiveParametersByCount_ReturnMethod()
    {
        //Arrange

        //Act
        var result = typeof(Garbage).GetSingleMethod(x => x.Name == nameof(Garbage.Overload) && x.HasParameters(5));

        //Assert
        result.Should().NotBeNull();
    }

    [TestMethod]
    public void GetSingleMethod_WhenGettingMethodOverloadWithSixParametersByCount_ReturnMethod()
    {
        //Arrange

        //Act
        var result = typeof(Garbage).GetSingleMethod(x => x.Name == nameof(Garbage.Overload) && x.HasParameters(6));

        //Assert
        result.Should().NotBeNull();
    }

    [TestMethod]
    public void GetSingleMethod_WhenGettingMethodOverloadWithSevenParametersByCount_ReturnMethod()
    {
        //Arrange

        //Act
        var result = typeof(Garbage).GetSingleMethod(x => x.Name == nameof(Garbage.Overload) && x.HasParameters(7));

        //Assert
        result.Should().NotBeNull();
    }

    [TestMethod]
    public void GetSingleMethod_WhenGettingMethodOverloadWithEightParametersByCount_ReturnMethod()
    {
        //Arrange

        //Act
        var result = typeof(Garbage).GetSingleMethod(x => x.Name == nameof(Garbage.Overload) && x.HasParameters(8));

        //Assert
        result.Should().NotBeNull();
    }

    [TestMethod]
    public void GetSingleMethod_WhenGettingMethodOverloadWithNineParametersByCount_ReturnMethod()
    {
        //Arrange

        //Act
        var result = typeof(Garbage).GetSingleMethod(x => x.Name == nameof(Garbage.Overload) && x.HasParameters(9));

        //Assert
        result.Should().NotBeNull();
    }

    [TestMethod]
    public void GetSingleMethod_WhenGettingMethodOverloadWithTenParametersByCount_ReturnMethod()
    {
        //Arrange

        //Act
        var result = typeof(Garbage).GetSingleMethod(x => x.Name == nameof(Garbage.Overload) && x.HasParameters(10));

        //Assert
        result.Should().NotBeNull();
    }

    [TestMethod]
    [DataRow("")]
    [DataRow(" ")]
    [DataRow(null)]
    public void GetSingleMethodOrDefault_WhenNameIsEmpty_Throw(string name)
    {
        //Arrange

        //Act
        var action = () => typeof(Garbage).GetSingleMethodOrDefault(name);

        //Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void GetSingleMethodOrDefault_WhenThereAreMethodsWithSameName_Throw()
    {
        //Arrange

        //Act
        var action = () => typeof(Garbage).GetSingleMethodOrDefault("SomeoneTouchedMe");

        //Assert
        action.Should().Throw<InvalidOperationException>();
    }

    [TestMethod]
    public void GetSingleMethodOrDefault_WhenThereAreNoMethodsWithName_DoNotThrow()
    {
        //Arrange

        //Act
        var action = () => typeof(Garbage).GetSingleMethodOrDefault("SomethingTouchedMe");

        //Assert
        action.Should().NotThrow();
    }

    [TestMethod]
    public void GetSingleMethodOrDefault_WhenGetFirstTouchWithCorrectParameters_ReturnFirstTouchMethod()
    {
        //Arrange

        //Act
        var result = typeof(Garbage).GetSingleMethodOrDefault(x => x.Name == "SomeoneTouchedMe" && x.HasParameters<int, string>() && !x.IsGenericMethod);

        //Assert
        result.Should().NotBeNull();
    }

    [TestMethod]
    public void GetSingleMethodOrDefault_WhenGetSecondTouchWithCorrectParameters_ReturnSecondTouchMethod()
    {
        //Arrange

        //Act
        var result = typeof(Garbage).GetSingleMethodOrDefault(x => x.Name == "SomeoneTouchedMe" && x.HasParameters<int, string>() && x.IsGenericMethod);

        //Assert
        result.Should().NotBeNull();
    }

    [TestMethod]
    public void GetSingleMethodOrDefault_WhenGetThirdTouchWithCorrectParameters_ReturnThirdTouchMethod()
    {
        //Arrange

        //Act
        var result = typeof(Garbage).GetSingleMethodOrDefault(x => x.Name == "SomeoneTouchedMe" && x.HasParameters<int, Garbage>() && !x.IsGenericMethod);

        //Assert
        result.Should().NotBeNull();
    }

    [TestMethod]
    public void GetSingleMethodOrDefault_WhenGetFourthTouchWithCorrectParameters_ReturnFourthTouchMethod()
    {
        //Arrange

        //Act
        var result = typeof(Garbage).GetSingleMethodOrDefault(x => x.Name == "SomeoneTouchedMe" && x.HasParameters<int, Garbage>() && x.IsGenericMethod);

        //Assert
        result.Should().NotBeNull();
    }

    [TestMethod]
    [DataRow("")]
    [DataRow(" ")]
    [DataRow(null)]
    public void HasMethod_WhenNameIsEmpty_Throw(string name)
    {
        //Arrange

        //Act
        var action = () => typeof(Garbage).HasMethod(name);

        //Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void HasMethod_WhenThereAreMethodsWithSameName_True()
    {
        //Arrange

        //Act
        var result = typeof(Garbage).HasMethod("SomeoneTouchedMe");

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void HasMethod_WhenThereAreNoMethodsWithName_False()
    {
        //Arrange

        //Act
        var result = typeof(Garbage).HasMethod("SomethingTouchedMe");

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void HasMethod_WhenGetFirstTouchWithCorrectParameters_True()
    {
        //Arrange

        //Act
        var result = typeof(Garbage).HasMethod(x => x.Name == "SomeoneTouchedMe" && x.HasParameters<int, string>() && !x.IsGenericMethod);

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void HasMethod_WhenGetSecondTouchWithCorrectParameters_True()
    {
        //Arrange

        //Act
        var result = typeof(Garbage).HasMethod(x => x.Name == "SomeoneTouchedMe" && x.HasParameters<int, string>() && x.IsGenericMethod);

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void HasMethod_WhenGetThirdTouchWithCorrectParameters_True()
    {
        //Arrange

        //Act
        var result = typeof(Garbage).HasMethod(x => x.Name == "SomeoneTouchedMe" && x.HasParameters<int, Garbage>() && !x.IsGenericMethod);

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void HasMethod_WhenGetFourthTouchWithCorrectParameters_True()
    {
        //Arrange

        //Act
        var result = typeof(Garbage).HasMethod(x => x.Name == "SomeoneTouchedMe" && x.HasParameters<int, Garbage>() && x.IsGenericMethod);

        //Assert
        result.Should().BeTrue();
    }
}