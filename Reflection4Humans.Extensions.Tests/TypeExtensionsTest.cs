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

    [TestClass]
    public class Implements : Tester
    {
        [TestMethod]
        public void WhenTypeIsNull_Throw()
        {
            //Arrange
            Type type = null!;
            var value = Fixture.Create<Type>();

            //Act
            var action = () => type.Implements(value);

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(type));
        }

        [TestMethod]
        public void WhenValueIsNull_Throw()
        {
            //Arrange
            var type = Fixture.Create<Type>();
            Type value = null!;

            //Act
            var action = () => type.Implements(value);

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(value));
        }

        [TestMethod]
        public void WhenTypeImplementsInterface_ReturnTrue()
        {
            //Arrange
            var type = typeof(DummyWithInterfaces);

            //Act
            var result = type.Implements<IDisposable>();

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenTypeDoesNotImplementInterface_ReturnFalse()
        {
            //Arrange
            var type = typeof(Dummy);

            //Act
            var result = type.Implements<IDisposable>();

            //Assert
            result.Should().BeFalse();
        }
    }

    [TestClass]
    public class HasAttribute_Any : Tester
    {
        public class Dummy { }

        [TestMethod]
        public void WhenTypeIsNull_Throw()
        {
            //Arrange
            Type type = null!;

            //Act
            var action = () => type.HasAttribute();

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(type));
        }

        [TestMethod]
        public void WhenTypeDoesNotHaveAnyAttribute_ReturnFalse()
        {
            //Arrange

            //Act
            var result = typeof(Dummy).HasAttribute();

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenTypeHasAttribute_ReturnTrue()
        {
            //Arrange

            //Act
            var result = typeof(DummyWithAttribute).HasAttribute();

            //Assert
            result.Should().BeTrue();
        }
    }

    [TestClass]
    public class HasAttribute_Generic : Tester
    {
        [Dummy(Name = "Roger")]
        public class DummyRoger { }

        [TestMethod]
        public void WhenTypeIsNull_Throw()
        {
            //Arrange
            Type type = null!;

            //Act
            var action = () => type.HasAttribute<DummyAttribute>();

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(type));
        }

        [TestMethod]
        public void WhenTypeDoesNotHaveAttribute_ReturnFalse()
        {
            //Arrange

            //Act
            var result = typeof(Dummy).HasAttribute(Fixture.Create<Func<DummyAttribute, bool>>());

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenTypeHasAttributeAndPredicateIsNull_ReturnTrue()
        {
            //Arrange

            //Act
            var result = typeof(DummyRoger).HasAttribute();

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenTypeHasAttributeButDoesNotMatchPredicate_ReturnFalse()
        {
            //Arrange

            //Act
            var result = typeof(DummyRoger).HasAttribute<DummyAttribute>(x => x.Name == "Seb");

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenTypeHasAttributeAndPredicateMatches_ReturnTrue()
        {
            //Arrange

            //Act
            var result = typeof(DummyRoger).HasAttribute<DummyAttribute>(x => x.Name == "Roger");

            //Assert
            result.Should().BeTrue();
        }
    }

    [TestClass]
    public class HasAttribute_Type : Tester
    {
        [TestMethod]
        public void WhenTypeIsNull_Throw()
        {
            //Arrange
            Type type = null!;
            var attribute = Fixture.Create<Type>();

            //Act
            var action = () => type.HasAttribute(attribute);

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(type));
        }

        [TestMethod]
        public void WhenAttributeIsNull_Throw()
        {
            //Arrange
            var type = Fixture.Create<Type>();
            Type attribute = null!;

            //Act
            var action = () => type.HasAttribute(attribute);

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(attribute));
        }

        [TestMethod]
        public void WhenTypeHasThatAttribute_ReturnTrue()
        {
            //Arrange
            var type = typeof(DummyWithAttribute);
            var attribute = typeof(DummyAttribute);

            //Act
            var result = type.HasAttribute(attribute);

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenTypeDoesNotHaveAttribute_ReturnFalse()
        {
            //Arrange
            var type = typeof(DummyWithAttribute);
            var attribute = typeof(SerializableAttribute);

            //Act
            var result = type.HasAttribute(attribute);

            //Assert
            result.Should().BeFalse();
        }
    }

    [TestClass]
    public class HasInterface_Any : Tester
    {
        public class Dummy { }

        [TestMethod]
        public void WhenTypeIsNull_Throw()
        {
            //Arrange
            Type type = null!;

            //Act
            var action = () => type.HasInterface();

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(type));
        }

        [TestMethod]
        public void WhenTypeHasNoInterface_ReturnFalse()
        {
            //Arrange

            //Act
            var result = typeof(Dummy).HasInterface();

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenTypeHasAnInterface_ReturnTrue()
        {
            //Arrange

            //Act
            var result = typeof(DummyWithInterfaces).HasInterface();

            //Assert
            result.Should().BeTrue();
        }
    }

    [TestClass]
    public class DirectlyImplements_Generic : Tester
    {
        public interface IDummy { }

        public interface IDummyBase { }

        public class Dummy : DummyBase, IDummy { }

        public class DummyBase : IDummyBase { }

        [TestMethod]
        public void WhenTypeIsNull_Throw()
        {
            //Arrange
            Type type = null!;

            //Act
            var action = () => type.DirectlyImplements<IDummy>();

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(type));
        }

        [TestMethod]
        public void WhenIndirectlyImplementsInterface_ReturnTrue()
        {
            //Arrange

            //Act
            var result = typeof(Dummy).DirectlyImplements<IDummyBase>();

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenTypeIsNotInterface_ReturnsFalse()
        {
            //Arrange

            //Act
            var result = typeof(Dummy).DirectlyImplements<DummyBase>();

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenDoesNotImplementDirectlyOrIndirectly_ReturnFalse()
        {
            //Arrange

            //Act
            var result = typeof(Dummy).DirectlyImplements<IDisposable>();

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenDirectlyImplements_ReturnTrue()
        {
            //Arrange

            //Act
            var result = typeof(Dummy).DirectlyImplements<IDummy>();

            //Assert
            result.Should().BeTrue();
        }
    }

    [TestClass]
    public class DirectlyImplements_Type : Tester
    {
        public interface IDummy { }

        public interface IDummyBase { }

        public class Dummy : DummyBase, IDummy { }

        public class DummyBase : IDummyBase { }

        [TestMethod]
        public void WhenTypeIsNull_Throw()
        {
            //Arrange
            Type type = null!;

            //Act
            var action = () => type.DirectlyImplements(typeof(IDummy));

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(type));
        }

        [TestMethod]
        public void WhenInterfaceIsNull_Throw()
        {
            //Arrange
            var type = typeof(Dummy);
            Type @interface = null!;

            //Act
            var action = () => type.DirectlyImplements(@interface);

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(@interface));
        }

        [TestMethod]
        public void WhenIndirectlyImplementsInterface_ReturnTrue()
        {
            //Arrange

            //Act
            var result = typeof(Dummy).DirectlyImplements(typeof(IDummyBase));

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenTypeIsNotInterface_ReturnsFalse()
        {
            //Arrange

            //Act
            var result = typeof(Dummy).DirectlyImplements(typeof(DummyBase));

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenDoesNotImplementDirectlyOrIndirectly_ReturnFalse()
        {
            //Arrange

            //Act
            var result = typeof(Dummy).DirectlyImplements(typeof(IDisposable));

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenDirectlyImplements_ReturnTrue()
        {
            //Arrange

            //Act
            var result = typeof(Dummy).DirectlyImplements(typeof(IDummy));

            //Assert
            result.Should().BeTrue();
        }
    }
}