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
    }

    [TestClass]
    public class GetSingleMethodOrDefault : Tester
    {
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