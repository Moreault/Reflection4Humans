using Reflection4Humans.Extensions.Tests.Dummies;
using ToolBX.Reflection4Humans.Extensions;
using ToolBX.Reflection4Humans.Extensions.Resources;
using TypeExtensions = ToolBX.Reflection4Humans.Extensions.TypeExtensions;

namespace Reflection4Humans.Extensions.Tests;

[TestClass]
public class TypeExtensionsTester
{
    [TestClass]
    public class GetReadableName : Tester
    {
        [TestMethod]
        public void WhenTypeIsNull_Throw()
        {
            //Arrange
            Type type = null!;

            //Act
            var action = () => type.GetHumanReadableName();

            //Assert
            action.Should().Throw<ArgumentNullException>().WithMessage(string.Format($"{Resource.CannotUseMethodBecauseParamaterIsMandatory}*", nameof(TypeExtensions.GetHumanReadableName), "type"));
        }

        [TestMethod]
        public void WhenIsNotGeneric_ReturnName()
        {
            //Arrange
            var type = typeof(Dummy);

            //Act
            var result = type.GetHumanReadableName();

            //Assert
            result.Should().Be("Dummy");
        }

        [TestMethod]
        public void WhenIsGeneric_ReturnNameWithGenericParameter()
        {
            //Arrange
            var type = typeof(List<Dummy>);

            //Act
            var result = type.GetHumanReadableName();

            //Assert
            result.Should().Be("List<Dummy>");
        }

        [TestMethod]
        public void WhenHasMultipleGenericParameters_ReturnNameWithGenericParameters()
        {
            //Arrange
            var type = typeof(Dictionary<string, Dummy>);

            //Act
            var result = type.GetHumanReadableName();

            //Assert
            result.Should().Be("Dictionary<String, Dummy>");
        }

        [TestMethod]
        public void WhenIsNestedGeneric_ReturnNameWithGenericParameters()
        {
            //Arrange
            var type = typeof(List<Dictionary<int, Dummy>>);

            //Act
            var result = type.GetHumanReadableName();

            //Assert
            result.Should().Be("List<Dictionary<Int32, Dummy>>");
        }
    }

    [TestClass]
    public class IsAttribute : Tester
    {
        [TestMethod]
        public void WhenIsAttribute_ReturnTrue()
        {
            //Arrange

            //Act
            var result = typeof(DummyAttribute).IsAttribute();

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenIsNotAttribute_ReturnFalse()
        {
            //Arrange

            //Act
            var result = typeof(Dummy).IsAttribute();

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenInheritsFromAttribute_ReturnTrue()
        {
            //Arrange

            //Act
            var result = typeof(DummyInheritedAttribute).IsAttribute();

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenInheritsFromClassThatInheritsFromAttribute_ReturnTrue()
        {
            //Arrange

            //Act
            var result = typeof(DummyThirdChildOfAttribute).IsAttribute();

            //Assert
            result.Should().BeTrue();
        }
    }
}