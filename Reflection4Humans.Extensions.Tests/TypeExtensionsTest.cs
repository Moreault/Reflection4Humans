using System.Data;
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
            var type = typeof(Garbage.Garbage);

            //Act
            var result = type.GetHumanReadableName();

            //Assert
            result.Should().Be("Garbage");
        }

        [TestMethod]
        public void WhenIsGeneric_ReturnNameWithGenericParameter()
        {
            //Arrange
            var type = typeof(List<Garbage.Garbage>);

            //Act
            var result = type.GetHumanReadableName();

            //Assert
            result.Should().Be("List<Garbage>");
        }

        [TestMethod]
        public void WhenHasMultipleGenericParameters_ReturnNameWithGenericParameters()
        {
            //Arrange
            var type = typeof(Dictionary<string, Garbage.Garbage>);

            //Act
            var result = type.GetHumanReadableName();

            //Assert
            result.Should().Be("Dictionary<String, Garbage>");
        }

        [TestMethod]
        public void WhenIsNestedGeneric_ReturnNameWithGenericParameters()
        {
            //Arrange
            var type = typeof(List<Dictionary<int, Garbage.Garbage>>);

            //Act
            var result = type.GetHumanReadableName();

            //Assert
            result.Should().Be("List<Dictionary<Int32, Garbage>>");
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
            var result = typeof(GarbageAttribute).IsAttribute();

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenIsNotAttribute_ReturnFalse()
        {
            //Arrange

            //Act
            var result = typeof(Garbage.Garbage).IsAttribute();

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenInheritsFromAttribute_ReturnTrue()
        {
            //Arrange

            //Act
            var result = typeof(GarbageInheritedAttribute).IsAttribute();

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenInheritsFromClassThatInheritsFromAttribute_ReturnTrue()
        {
            //Arrange

            //Act
            var result = typeof(GarbageThirdChildOfAttribute).IsAttribute();

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
            var propertyName = Dummy.Create<string>();
            var comparison = Dummy.Create<StringComparison>();

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
            var type = Dummy.Create<Type>();
            var comparison = Dummy.Create<StringComparison>();

            //Act
            var action = () => type.GetPropertyPath(propertyName, comparison);

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(propertyName));
        }

        [TestMethod]
        public void WhenPropertyIsNotOnObject_Throw()
        {
            //Arrange
            var type = typeof(object);
            var propertyName = Dummy.Create<string>();
            var comparison = Dummy.Create<StringComparison>();

            //Act
            var action = () => type.GetPropertyPath(propertyName, comparison);

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage($"{string.Format(Exceptions.PropertyNotFoundOnType, propertyName, type.Name)}*").WithParameterName(nameof(propertyName));
        }

        [TestMethod]
        public void WhenChildPropertyIsNotOnChild_Throw()
        {
            //Arrange
            var type = typeof(GarbageChild);
            var missingProperty = Dummy.Create<string>();
            var propertyName = $"{nameof(GarbageChild.GrandChild)}.{missingProperty}";
            var comparison = Dummy.Create<StringComparison>();

            //Act
            var action = () => type.GetPropertyPath(propertyName, comparison);

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage($"{string.Format(Exceptions.PropertyNotFoundOnType, missingProperty, nameof(GarbageGrandChild))}*").WithParameterName(nameof(propertyName));
        }

        [TestMethod]
        public void WhenPropertyIsOnObjectButWithDifferentCasingWithoutIgnoreCase_Throw()
        {
            //Arrange
            var type = typeof(GarbageChild);
            var propertyName = $"{nameof(GarbageChild.GrandChild)}.{nameof(GarbageGrandChild.Age).ToUpper()}";
            var comparison = StringComparison.CurrentCulture;

            //Act
            var action = () => type.GetPropertyPath(propertyName, comparison);

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage($"{string.Format(Exceptions.PropertyNotFoundOnType, nameof(GarbageGrandChild.Age).ToUpper(), nameof(GarbageGrandChild))}*").WithParameterName(nameof(propertyName));
        }

        [TestMethod]
        public void WhenPropertyIsOnObjectWithSameCasingWithoutIgnoreCase_Return()
        {
            //Arrange
            var type = typeof(GarbageChild);
            var propertyName = $"{nameof(GarbageChild.GrandChild)}.{nameof(GarbageGrandChild.Age)}";
            var comparison = StringComparison.CurrentCulture;

            //Act
            var result = type.GetPropertyPath(propertyName, comparison);

            //Assert
            result.Should().BeEquivalentTo(new List<PropertyPath>
            {
                new() { Property = typeof(GarbageChild).GetProperty(nameof(GarbageChild.GrandChild))!, Owner = typeof(GarbageChild) },
                new() { Property = typeof(GarbageGrandChild).GetProperty(nameof(GarbageGrandChild.Age))!, Owner = typeof(GarbageGrandChild) },
            });
        }

        [TestMethod]
        public void WhenPropertyIsOnObjectWithDifferentCasingWithIgnoreCase_Return()
        {
            //Arrange
            var type = typeof(GarbageChild);
            var propertyName = $"{nameof(GarbageChild.GrandChild)}.{nameof(GarbageGrandChild.Age).ToUpper()}";
            var comparison = StringComparison.InvariantCultureIgnoreCase;

            //Act
            var result = type.GetPropertyPath(propertyName, comparison);

            //Assert
            result.Should().BeEquivalentTo(new List<PropertyPath>
            {
                new() { Property = typeof(GarbageChild).GetProperty(nameof(GarbageChild.GrandChild))!, Owner = typeof(GarbageChild) },
                new() { Property = typeof(GarbageGrandChild).GetProperty(nameof(GarbageGrandChild.Age))!, Owner = typeof(GarbageGrandChild) },
            });
        }

        [TestMethod]
        public void WhenPropertyIsOnGrandChild_ReturnFullPath()
        {
            //Arrange
            var type = typeof(MultiLevelGarbage);

            //Act
            var result = type.GetPropertyPath($"{nameof(MultiLevelGarbage.Child)}.{nameof(GarbageChild.GrandChild)}.{nameof(GarbageGrandChild.Age)}");

            //Assert
            result.Should().BeEquivalentTo(new List<PropertyPath>
            {
                new() { Property = typeof(MultiLevelGarbage).GetProperty(nameof(MultiLevelGarbage.Child))!, Owner = typeof(MultiLevelGarbage) },
                new() { Property = typeof(GarbageChild).GetProperty(nameof(GarbageChild.GrandChild))!, Owner = typeof(GarbageChild) },
                new() { Property = typeof(GarbageGrandChild).GetProperty(nameof(GarbageGrandChild.Age))!, Owner = typeof(GarbageGrandChild) },
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
            var result = typeof(GarbageWithInterfaces).GetDirectInterfaces();

            //Assert
            result.Should().BeEquivalentTo(new List<Type> { typeof(ITopGarbage1), typeof(ITopGarbage2) });
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
            var value = Dummy.Create<Type>();

            //Act
            var action = () => type.Implements(value);

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(type));
        }

        [TestMethod]
        public void WhenValueIsNull_Throw()
        {
            //Arrange
            var type = Dummy.Create<Type>();
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
            var type = typeof(GarbageWithInterfaces);

            //Act
            var result = type.Implements<IDisposable>();

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenTypeDoesNotImplementInterface_ReturnFalse()
        {
            //Arrange
            var type = typeof(Garbage.Garbage);

            //Act
            var result = type.Implements<IDisposable>();

            //Assert
            result.Should().BeFalse();
        }
    }

    [TestClass]
    public class HasAttribute_Any : Tester
    {
        public class Garbage { }

        [TestMethod]
        public void WhenTypeIsNull_Throw()
        {
            //Arrange
            Type member = null!;

            //Act
            var action = () => member.HasAttribute();

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(member));
        }

        [TestMethod]
        public void WhenTypeDoesNotHaveAnyAttribute_ReturnFalse()
        {
            //Arrange

            //Act
            var result = typeof(Garbage).HasAttribute();

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenTypeHasAttribute_ReturnTrue()
        {
            //Arrange

            //Act
            var result = typeof(GarbageWithAttribute).HasAttribute();

            //Assert
            result.Should().BeTrue();
        }
    }

    [TestClass]
    public class HasAttribute_Generic : Tester
    {
        [Garbage(Name = "Roger")]
        public class GarbageRoger { }

        [TestMethod]
        public void WhenTypeIsNull_Throw()
        {
            //Arrange
            Type member = null!;

            //Act
            var action = () => member.HasAttribute<GarbageAttribute>();

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(member));
        }

        [TestMethod]
        public void WhenTypeDoesNotHaveAttribute_ReturnFalse()
        {
            //Arrange

            //Act
            var result = typeof(Garbage.Garbage).HasAttribute(Dummy.Create<Func<GarbageAttribute, bool>>());

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenTypeHasAttributeAndPredicateIsNull_ReturnTrue()
        {
            //Arrange

            //Act
            var result = typeof(GarbageRoger).HasAttribute();

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenTypeHasAttributeButDoesNotMatchPredicate_ReturnFalse()
        {
            //Arrange

            //Act
            var result = typeof(GarbageRoger).HasAttribute<GarbageAttribute>(x => x.Name == "Seb");

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenTypeHasAttributeAndPredicateMatches_ReturnTrue()
        {
            //Arrange

            //Act
            var result = typeof(GarbageRoger).HasAttribute<GarbageAttribute>(x => x.Name == "Roger");

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
            Type member = null!;
            var attribute = Dummy.Create<Type>();

            //Act
            var action = () => member.HasAttribute(attribute);

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(member));
        }

        [TestMethod]
        public void WhenAttributeIsNull_Throw()
        {
            //Arrange
            var type = Dummy.Create<Type>();
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
            var type = typeof(GarbageWithAttribute);
            var attribute = typeof(GarbageAttribute);

            //Act
            var result = type.HasAttribute(attribute);

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenTypeDoesNotHaveAttribute_ReturnFalse()
        {
            //Arrange
            var type = typeof(GarbageWithAttribute);
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
        public class Garbage { }

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
            var result = typeof(Garbage).HasInterface();

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenTypeHasAnInterface_ReturnTrue()
        {
            //Arrange

            //Act
            var result = typeof(GarbageWithInterfaces).HasInterface();

            //Assert
            result.Should().BeTrue();
        }
    }

    [TestClass]
    public class DirectlyImplements_Generic : Tester
    {
        public interface IGarbage { }

        public interface IGarbageBase { }

        public class Garbage : GarbageBase, IGarbage { }

        public class GarbageBase : IGarbageBase { }

        [TestMethod]
        public void WhenTypeIsNull_Throw()
        {
            //Arrange
            Type type = null!;

            //Act
            var action = () => type.DirectlyImplements<IGarbage>();

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(type));
        }

        [TestMethod]
        public void WhenIndirectlyImplementsInterface_ReturnTrue()
        {
            //Arrange

            //Act
            var result = typeof(Garbage).DirectlyImplements<IGarbageBase>();

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenTypeIsNotInterface_ReturnsFalse()
        {
            //Arrange

            //Act
            var result = typeof(Garbage).DirectlyImplements<GarbageBase>();

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenDoesNotImplementDirectlyOrIndirectly_ReturnFalse()
        {
            //Arrange

            //Act
            var result = typeof(Garbage).DirectlyImplements<IDisposable>();

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenDirectlyImplements_ReturnTrue()
        {
            //Arrange

            //Act
            var result = typeof(Garbage).DirectlyImplements<IGarbage>();

            //Assert
            result.Should().BeTrue();
        }
    }

    [TestClass]
    public class DirectlyImplements_Type : Tester
    {
        public interface IGarbage { }

        public interface IGarbageBase { }

        public class Garbage : GarbageBase, IGarbage { }

        public class GarbageBase : IGarbageBase { }

        [TestMethod]
        public void WhenTypeIsNull_Throw()
        {
            //Arrange
            Type type = null!;

            //Act
            var action = () => type.DirectlyImplements(typeof(IGarbage));

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(type));
        }

        [TestMethod]
        public void WhenInterfaceIsNull_Throw()
        {
            //Arrange
            var type = typeof(Garbage);
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
            var result = typeof(Garbage).DirectlyImplements(typeof(IGarbageBase));

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenTypeIsNotInterface_ReturnsFalse()
        {
            //Arrange

            //Act
            var result = typeof(Garbage).DirectlyImplements(typeof(GarbageBase));

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenDoesNotImplementDirectlyOrIndirectly_ReturnFalse()
        {
            //Arrange

            //Act
            var result = typeof(Garbage).DirectlyImplements(typeof(IDisposable));

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenDirectlyImplements_ReturnTrue()
        {
            //Arrange

            //Act
            var result = typeof(Garbage).DirectlyImplements(typeof(IGarbage));

            //Assert
            result.Should().BeTrue();
        }
    }

    [TestClass]
    public class GetDefaultValue : Tester
    {
        [TestMethod]
        public void WhenTypeIsNull_Throw()
        {
            //Arrange
            Type type = null!;

            //Act
            var action = () => type.GetDefaultValue();

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(type));
        }

        [TestMethod]
        [DataRow(typeof(int))]
        [DataRow(typeof(uint))]
        [DataRow(typeof(byte))]
        [DataRow(typeof(sbyte))]
        [DataRow(typeof(short))]
        [DataRow(typeof(ushort))]
        [DataRow(typeof(long))]
        [DataRow(typeof(ulong))]
        [DataRow(typeof(float))]
        [DataRow(typeof(decimal))]
        [DataRow(typeof(double))]
        public void WhenIsValueType_ReturnDefault(Type type)
        {
            //Arrange

            //Act
            var result = type.GetDefaultValue();

            //Assert
            result.Should().Be(Convert.ChangeType(0, type));
        }

        [TestMethod]
        public void WhenIsReferenceType_ReturnNull()
        {
            //Arrange
            var type = typeof(string);

            //Act
            var result = type.GetDefaultValue();

            //Assert
            result.Should().BeNull();
        }
    }
}