namespace Reflection4Humans.Extensions.Tests;

public partial class MemberSearchExtensionsTest
{
    [TestClass]
    public class GetAllMethods : Tester
    {
        [TestMethod]
        public void WhenTypeIsNull_Throw()
        {
            //Arrange
            Type type = null!;

            //Act
            var action = () => type.GetAllMethods();

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(type));
        }

        [TestMethod]
        public void WhenGettingOnlyGenericMethods_ReturnOnlyGenerics()
        {
            //Arrange

            //Act
            var result = typeof(Dummy).GetAllMethods(x => x.IsGeneric);

            //Assert
            result.Select(x => x.Name).Should().BeEquivalentTo(new List<string>
            {
                "SomeoneTouchedMe", "SomeoneTouchedMe", "SomeoneTouchedMe"
            });
        }
    }

    [TestClass]
    public class GetSingleMethod : Tester
    {
        [TestMethod]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow(null)]
        public void WhenNameIsEmpty_Throw(string name)
        {
            //Arrange

            //Act
            var action = () => typeof(Dummy).GetSingleMethod(name);

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void WhenThereAreMethodsWithSameName_Throw()
        {
            //Arrange

            //Act
            var action = () => typeof(Dummy).GetSingleMethod("SomeoneTouchedMe");

            //Assert
            action.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public void WhenThereAreNoMethodsWithName_Throw()
        {
            //Arrange

            //Act
            var action = () => typeof(Dummy).GetSingleMethod("SomethingTouchedMe");

            //Assert
            action.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public void WhenGetFirstTouchWithCorrectParameters_ReturnFirstTouchMethod()
        {
            //Arrange

            //Act
            var result = typeof(Dummy).GetSingleMethod("SomeoneTouchedMe", x => x.HasParameters<int, string>() && !x.IsGeneric);

            //Assert
            result.Should().NotBeNull();
        }

        [TestMethod]
        public void WhenGetSecondTouchWithCorrectParameters_ReturnSecondTouchMethod()
        {
            //Arrange

            //Act
            var result = typeof(Dummy).GetSingleMethod("SomeoneTouchedMe", x => x.HasParameters<int, string>() && x.IsGeneric);

            //Assert
            result.Should().NotBeNull();
        }

        [TestMethod]
        public void WhenGetThirdTouchWithCorrectParameters_ReturnThirdTouchMethod()
        {
            //Arrange

            //Act
            var result = typeof(Dummy).GetSingleMethod("SomeoneTouchedMe", x => x.HasParameters<int, Dummy>() && !x.IsGeneric);

            //Assert
            result.Should().NotBeNull();
        }

        [TestMethod]
        public void WhenGetFourthTouchWithCorrectParameters_ReturnFourthTouchMethod()
        {
            //Arrange

            //Act
            var result = typeof(Dummy).GetSingleMethod("SomeoneTouchedMe", x => x.HasParameters<int, Dummy>() && x.IsGeneric);

            //Assert
            result.Should().NotBeNull();
        }

        [TestMethod]
        public void WhenGettingMethodOverloadWithZeroParameters_ReturnMethod()
        {
            //Arrange

            //Act
            var result = typeof(Dummy).GetSingleMethod(nameof(Dummy.Overload), x => x.HasNoParameter);

            //Assert
            result.Should().NotBeNull();
        }

        [TestMethod]
        public void WhenGettingMethodOverloadWithOneParameterByExactType_ReturnMethod()
        {
            //Arrange

            //Act
            var result = typeof(Dummy).GetSingleMethod(nameof(Dummy.Overload), x => x.HasParameters<string>());

            //Assert
            result.Should().NotBeNull();
        }

        [TestMethod]
        public void WhenGettingMethodOverloadWithTwoParametersByExactType_ReturnMethod()
        {
            //Arrange

            //Act
            var result = typeof(Dummy).GetSingleMethod(nameof(Dummy.Overload), x => x.HasParameters<string, int>());

            //Assert
            result.Should().NotBeNull();
        }

        [TestMethod]
        public void WhenGettingMethodOverloadWithThreeParametersByExactType_ReturnMethod()
        {
            //Arrange

            //Act
            var result = typeof(Dummy).GetSingleMethod(nameof(Dummy.Overload), x => x.HasParameters<string, int, long>());

            //Assert
            result.Should().NotBeNull();
        }

        [TestMethod]
        public void WhenGettingMethodOverloadWithFourParameters_ReturnMethod()
        {
            //Arrange

            //Act
            var result = typeof(Dummy).GetSingleMethod(nameof(Dummy.Overload), x => x.HasParameters<string, int, long, char>());

            //Assert
            result.Should().NotBeNull();
        }

        [TestMethod]
        public void WhenGettingMethodOverloadWithFiveParametersByExactType_ReturnMethod()
        {
            //Arrange

            //Act
            var result = typeof(Dummy).GetSingleMethod(nameof(Dummy.Overload), x => x.HasParameters<string, int, long, char, string>());

            //Assert
            result.Should().NotBeNull();
        }

        [TestMethod]
        public void WhenGettingMethodOverloadWithSixParametersByExactType_ReturnMethod()
        {
            //Arrange

            //Act
            var result = typeof(Dummy).GetSingleMethod(nameof(Dummy.Overload), x => x.HasParameters<string, int, long, char, string, float>());

            //Assert
            result.Should().NotBeNull();
        }

        [TestMethod]
        public void WhenGettingMethodOverloadWithSevenParametersByExactType_ReturnMethod()
        {
            //Arrange

            //Act
            var result = typeof(Dummy).GetSingleMethod(nameof(Dummy.Overload), x => x.HasParameters<string, int, long, char, string, float, double>());

            //Assert
            result.Should().NotBeNull();
        }

        [TestMethod]
        public void WhenGettingMethodOverloadWithEightParametersByExactType_ReturnMethod()
        {
            //Arrange

            //Act
            var result = typeof(Dummy).GetSingleMethod(nameof(Dummy.Overload), x => x.HasParameters<string, int, long, char, string, float, double, string>());

            //Assert
            result.Should().NotBeNull();
        }

        [TestMethod]
        public void WhenGettingMethodOverloadWithNineParametersByExactType_ReturnMethod()
        {
            //Arrange

            //Act
            var result = typeof(Dummy).GetSingleMethod(nameof(Dummy.Overload), x => x.HasParameters<string, int, long, char, string, float, double, string, int>());

            //Assert
            result.Should().NotBeNull();
        }

        [TestMethod]
        public void WhenGettingMethodOverloadWithTenParametersByExactType_ReturnMethod()
        {
            //Arrange

            //Act
            var result = typeof(Dummy).GetSingleMethod(nameof(Dummy.Overload), x => x.HasParameters<string, int, long, char, string, float, double, string, int, char>());

            //Assert
            result.Should().NotBeNull();
        }

        [TestMethod]
        public void WhenGettingMethodOverloadWithOneParameterByCount_ReturnMethod()
        {
            //Arrange

            //Act
            var result = typeof(Dummy).GetSingleMethod(nameof(Dummy.Overload), x => x.HasParameters(1));

            //Assert
            result.Should().NotBeNull();
        }

        [TestMethod]
        public void WhenGettingMethodOverloadWithTwoParametersByCount_ReturnMethod()
        {
            //Arrange

            //Act
            var result = typeof(Dummy).GetSingleMethod(nameof(Dummy.Overload), x => x.HasParameters(2));

            //Assert
            result.Should().NotBeNull();
        }

        [TestMethod]
        public void WhenGettingMethodOverloadWithThreeParametersByCount_ReturnMethod()
        {
            //Arrange

            //Act
            var result = typeof(Dummy).GetSingleMethod(nameof(Dummy.Overload), x => x.HasParameters(3));

            //Assert
            result.Should().NotBeNull();
        }

        [TestMethod]
        public void WhenGettingMethodOverloadWithFourParametersByCount_ReturnMethod()
        {
            //Arrange

            //Act
            var result = typeof(Dummy).GetSingleMethod(nameof(Dummy.Overload), x => x.HasParameters(4));

            //Assert
            result.Should().NotBeNull();
        }

        [TestMethod]
        public void WhenGettingMethodOverloadWithFiveParametersByCount_ReturnMethod()
        {
            //Arrange

            //Act
            var result = typeof(Dummy).GetSingleMethod(nameof(Dummy.Overload), x => x.HasParameters(5));

            //Assert
            result.Should().NotBeNull();
        }

        [TestMethod]
        public void WhenGettingMethodOverloadWithSixParametersByCount_ReturnMethod()
        {
            //Arrange

            //Act
            var result = typeof(Dummy).GetSingleMethod(nameof(Dummy.Overload), x => x.HasParameters(6));

            //Assert
            result.Should().NotBeNull();
        }

        [TestMethod]
        public void WhenGettingMethodOverloadWithSevenParametersByCount_ReturnMethod()
        {
            //Arrange

            //Act
            var result = typeof(Dummy).GetSingleMethod(nameof(Dummy.Overload), x => x.HasParameters(7));

            //Assert
            result.Should().NotBeNull();
        }

        [TestMethod]
        public void WhenGettingMethodOverloadWithEightParametersByCount_ReturnMethod()
        {
            //Arrange

            //Act
            var result = typeof(Dummy).GetSingleMethod(nameof(Dummy.Overload), x => x.HasParameters(8));

            //Assert
            result.Should().NotBeNull();
        }

        [TestMethod]
        public void WhenGettingMethodOverloadWithNineParametersByCount_ReturnMethod()
        {
            //Arrange

            //Act
            var result = typeof(Dummy).GetSingleMethod(nameof(Dummy.Overload), x => x.HasParameters(9));

            //Assert
            result.Should().NotBeNull();
        }

        [TestMethod]
        public void WhenGettingMethodOverloadWithTenParametersByCount_ReturnMethod()
        {
            //Arrange

            //Act
            var result = typeof(Dummy).GetSingleMethod(nameof(Dummy.Overload), x => x.HasParameters(10));

            //Assert
            result.Should().NotBeNull();
        }
    }

    [TestClass]
    public class GetSingleMethodOrDefault : Tester
    {
        [TestMethod]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow(null)]
        public void WhenNameIsEmpty_Throw(string name)
        {
            //Arrange

            //Act
            var action = () => typeof(Dummy).GetSingleMethodOrDefault(name);

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void WhenThereAreMethodsWithSameName_Throw()
        {
            //Arrange

            //Act
            var action = () => typeof(Dummy).GetSingleMethodOrDefault("SomeoneTouchedMe");

            //Assert
            action.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public void WhenThereAreNoMethodsWithName_DoNotThrow()
        {
            //Arrange

            //Act
            var action = () => typeof(Dummy).GetSingleMethodOrDefault("SomethingTouchedMe");

            //Assert
            action.Should().NotThrow();
        }

        [TestMethod]
        public void WhenGetFirstTouchWithCorrectParameters_ReturnFirstTouchMethod()
        {
            //Arrange

            //Act
            var result = typeof(Dummy).GetSingleMethodOrDefault("SomeoneTouchedMe", x => x.HasParameters<int, string>() && !x.IsGeneric);

            //Assert
            result.Should().NotBeNull();
        }

        [TestMethod]
        public void WhenGetSecondTouchWithCorrectParameters_ReturnSecondTouchMethod()
        {
            //Arrange

            //Act
            var result = typeof(Dummy).GetSingleMethodOrDefault("SomeoneTouchedMe", x => x.HasParameters<int, string>() && x.IsGeneric);

            //Assert
            result.Should().NotBeNull();
        }

        [TestMethod]
        public void WhenGetThirdTouchWithCorrectParameters_ReturnThirdTouchMethod()
        {
            //Arrange

            //Act
            var result = typeof(Dummy).GetSingleMethodOrDefault("SomeoneTouchedMe", x => x.HasParameters<int, Dummy>() && !x.IsGeneric);

            //Assert
            result.Should().NotBeNull();
        }

        [TestMethod]
        public void WhenGetFourthTouchWithCorrectParameters_ReturnFourthTouchMethod()
        {
            //Arrange

            //Act
            var result = typeof(Dummy).GetSingleMethodOrDefault("SomeoneTouchedMe", x => x.HasParameters<int, Dummy>() && x.IsGeneric);

            //Assert
            result.Should().NotBeNull();
        }
    }
}