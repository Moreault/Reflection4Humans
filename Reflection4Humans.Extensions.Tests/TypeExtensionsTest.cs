using TypeExtensions = ToolBX.Reflection4Humans.Extensions.TypeExtensions;

namespace Reflection4Humans.Extensions.Tests;

[TestClass]
public class TypeExtensionsTest
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
            action.Should().Throw<ArgumentNullException>().WithMessage(string.Format($"{Exceptions.CannotUseMethodBecauseParamaterIsMandatory}*", nameof(TypeExtensions.GetHumanReadableName), "type"));
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

    [TestClass]
    public class GetPropertyPath : Tester
    {
        [TestMethod]
        public void WhenTypeIsNull_Throw()
        {
            //Arrange
            Type type = null!;
            var propertyName = Fixture.Create<string>();
            var comparison = Fixture.Create<StringComparison>();

            //Act
            var action = () => type.GetPropertyPath(propertyName, comparison);

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(type));
        }

        [TestMethod]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow(null)]
        public void WhenPropertyNameIsEmpty_Throw(string propertyName)
        {
            //Arrange
            var type = Fixture.Create<Type>();
            var comparison = Fixture.Create<StringComparison>();

            //Act
            var action = () => type.GetPropertyPath(propertyName, comparison);

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(propertyName));
        }

        [TestMethod]
        public void WhenPropertyIsNotOnObject_Throw()
        {
            //Arrange
            var type = Fixture.Create<Type>();
            var propertyName = Fixture.Create<string>();
            var comparison = Fixture.Create<StringComparison>();

            //Act
            var action = () => type.GetPropertyPath(propertyName, comparison);

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage($"{string.Format(Exceptions.PropertyNotFoundOnType, propertyName, type.Name)}*").WithParameterName(nameof(propertyName));
        }

        [TestMethod]
        public void WhenChildPropertyIsNotOnChild_Throw()
        {
            //Arrange
            var type = typeof(DummyChild);
            var missingProperty = Fixture.Create<string>();
            var propertyName = $"{nameof(DummyChild.GrandChild)}.{missingProperty}";
            var comparison = Fixture.Create<StringComparison>();

            //Act
            var action = () => type.GetPropertyPath(propertyName, comparison);

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage($"{string.Format(Exceptions.PropertyNotFoundOnType, missingProperty, nameof(DummyGrandChild))}*").WithParameterName(nameof(propertyName));
        }

        [TestMethod]
        public void WhenPropertyIsOnObjectButWithDifferentCasingWithoutIgnoreCase_Throw()
        {
            //Arrange
            var type = typeof(DummyChild);
            var propertyName = $"{nameof(DummyChild.GrandChild)}.{nameof(DummyGrandChild.Age).ToUpper()}";
            var comparison = StringComparison.CurrentCulture;

            //Act
            var action = () => type.GetPropertyPath(propertyName, comparison);

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage($"{string.Format(Exceptions.PropertyNotFoundOnType, nameof(DummyGrandChild.Age).ToUpper(), nameof(DummyGrandChild))}*").WithParameterName(nameof(propertyName));
        }

        [TestMethod]
        public void WhenPropertyIsOnObjectWithSameCasingWithoutIgnoreCase_Return()
        {
            //Arrange
            var type = typeof(DummyChild);
            var propertyName = $"{nameof(DummyChild.GrandChild)}.{nameof(DummyGrandChild.Age)}";
            var comparison = StringComparison.CurrentCulture;

            //Act
            var result = type.GetPropertyPath(propertyName, comparison);

            //Assert
            result.Should().BeEquivalentTo(new List<PropertyPath>
            {
                new() { Property = typeof(DummyChild).GetProperty(nameof(DummyChild.GrandChild))!, Owner = typeof(DummyChild) },
                new() { Property = typeof(DummyGrandChild).GetProperty(nameof(DummyGrandChild.Age))!, Owner = typeof(DummyGrandChild) },
            });
        }

        [TestMethod]
        public void WhenPropertyIsOnObjectWithDifferentCasingWithIgnoreCase_Return()
        {
            //Arrange
            var type = typeof(DummyChild);
            var propertyName = $"{nameof(DummyChild.GrandChild)}.{nameof(DummyGrandChild.Age).ToUpper()}";
            var comparison = StringComparison.InvariantCultureIgnoreCase;

            //Act
            var result = type.GetPropertyPath(propertyName, comparison);

            //Assert
            result.Should().BeEquivalentTo(new List<PropertyPath>
            {
                new() { Property = typeof(DummyChild).GetProperty(nameof(DummyChild.GrandChild))!, Owner = typeof(DummyChild) },
                new() { Property = typeof(DummyGrandChild).GetProperty(nameof(DummyGrandChild.Age))!, Owner = typeof(DummyGrandChild) },
            });
        }

        [TestMethod]
        public void WhenPropertyIsOnGrandChild_ReturnFullPath()
        {
            //Arrange
            var type = typeof(MultiLevelDummy);

            //Act
            var result = type.GetPropertyPath($"{nameof(MultiLevelDummy.Child)}.{nameof(DummyChild.GrandChild)}.{nameof(DummyGrandChild.Age)}");

            //Assert
            result.Should().BeEquivalentTo(new List<PropertyPath>
            {
                new() { Property = typeof(MultiLevelDummy).GetProperty(nameof(MultiLevelDummy.Child))!, Owner = typeof(MultiLevelDummy) },
                new() { Property = typeof(DummyChild).GetProperty(nameof(DummyChild.GrandChild))!, Owner = typeof(DummyChild) },
                new() { Property = typeof(DummyGrandChild).GetProperty(nameof(DummyGrandChild.Age))!, Owner = typeof(DummyGrandChild) },
            });
        }
    }

    [TestClass]
    public class GetDirectInterfaces : Tester
    {
        [TestMethod]
        public void WhenTypeIsNull_Throw()
        {
            //Arrange
            Type type = null!;

            //Act
            var action = () => type.GetDirectInterfaces();

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(type));
        }

        [TestMethod]
        public void Always_OnlyReturnInterfacesOnCurrentType()
        {
            //Arrange

            //Act
            var result = typeof(DummyWithInterfaces).GetDirectInterfaces();

            //Assert
            result.Should().BeEquivalentTo(new List<Type> { typeof(ITopDummy1), typeof(ITopDummy2) });
        }
    }
}