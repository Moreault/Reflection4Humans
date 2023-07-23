namespace Reflection4Humans.Extensions.Tests;

[TestClass]
public class MethodBaseExtensionsTest
{
    private class Dummy
    {
        private void SomeMethod()
        {

        }

        private void SomeMethod(int a)
        {

        }

        private void SomeMethod(int a, char b)
        {

        }

        private void SomeMethod(int a, char b, long c)
        {

        }

        private void SomeMethod(int a, char b, long c, string d)
        {

        }

        private void SomeMethod(string a)
        {

        }

        private void SomeMethod(float a, char b)
        {

        }

        private void SomeMethod(int a, string b, double c)
        {

        }

        private void SomeMethod(string a, char b, decimal c, string d)
        {

        }

    }

    [TestClass]
    public class HasParameters : Tester
    {
        [TestMethod]
        public void WhenMethodInfoIsNull_Throw()
        {
            //Arrange
            MethodBase methodInfo = null!;

            //Act
            var action = () => methodInfo.HasParameters(Fixture.CreateMany<Type>().ToArray());

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(methodInfo));
        }

        [TestMethod]
        public void WhenParametersIsNull_Throw()
        {
            //Arrange
            var methodInfo = typeof(Dummy).GetSingleMethod(x => x.Name == "SomeMethod" && x.HasNoParameter());
            Type[] parameters = null!;

            //Act
            var action = () => methodInfo.HasParameters(parameters);

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(parameters));
        }

        [TestMethod]
        public void WhenParametersIsEmpty_ReturnTrueIfMethodHasNoParameters()
        {
            //Arrange
            var methodInfo = typeof(Dummy).GetSingleMethod(x => x.Name == "SomeMethod" && x.HasNoParameter());
            var parameters = Array.Empty<Type>();

            //Act
            var result = methodInfo.HasParameters(parameters);

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenParametersIsEmpty_ReturnFalseIfMethodHasParameters()
        {
            //Arrange
            var methodInfo = typeof(Dummy).GetSingleMethod(x => x.Name == "SomeMethod" && x.HasParameters<string>());
            var parameters = Array.Empty<Type>();

            //Act
            var result = methodInfo.HasParameters(parameters);

            //Assert
            result.Should().BeFalse();
        }
    }

    [TestClass]
    public class HasParameterCount : Tester
    {
        [TestMethod]
        public void WhenMethodInfoIsNull_Throw()
        {
            //Arrange
            MethodBase methodInfo = null!;
            var count = Fixture.Create<int>();

            //Act
            var action = () => methodInfo.HasParameters(count);

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(methodInfo));
        }

        [TestMethod]
        public void WhenParameterCountIsTheSame_ReturnTrue()
        {
            //Arrange
            var methodInfo = typeof(Dummy).GetSingleMethod(x => x.Name == "SomeMethod" && x.HasParameters<int, char>());

            //Act
            var result = methodInfo.HasParameters(2);

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenParameterCountIsDifferent_ReturnFalse()
        {
            //Arrange
            var methodInfo = typeof(Dummy).GetSingleMethod(x => x.Name == "SomeMethod" && x.HasParameters<int, char>());

            //Act
            var result = methodInfo.HasParameters(3);

            //Assert
            result.Should().BeFalse();
        }
    }

    [TestClass]
    public class HasNoParameter : Tester
    {
        [TestMethod]
        public void WhenMethodInfoIsNull_Throw()
        {
            //Arrange
            MethodBase methodInfo = null!;

            //Act
            var action = () => methodInfo.HasNoParameter();

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(methodInfo));
        }

        [TestMethod]
        public void WhenMethodHasNoParameter_ReturnTrue()
        {
            //Arrange
            var methodInfo = typeof(Dummy).GetSingleMethod(x => x.Name == "SomeMethod" && x.HasNoParameter());

            //Act
            var result = methodInfo.HasNoParameter();

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenMethodHasParameters_ReturnFalse()
        {
            //Arrange
            var methodInfo = typeof(Dummy).GetSingleMethod(x => x.Name == "SomeMethod" && x.HasParameters<int, char>());

            //Act
            var result = methodInfo.HasNoParameter();

            //Assert
            result.Should().BeFalse();
        }
    }
}