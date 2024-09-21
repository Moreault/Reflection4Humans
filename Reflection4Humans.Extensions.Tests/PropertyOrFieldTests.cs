namespace Reflection4Humans.Extensions.Tests;

[TestClass]
public sealed class PropertyOrFieldTests : Tester
{
    private class Garbage
    {
        public static int StaticProperty { get; private set; }
        public static int StaticField;

        private string PrivateProperty { get; set; }
        private string PrivateField;

        protected string ProtectedProperty { get; set; }
        protected string ProtectedField;

        internal string InternalProperty { get; set; }
        internal string InternalField;

        public string Property { get; set; }
        public string ReadOnlyProperty => "Yo";
        public string WriteOnly { set => _writeOnly = value; }
        private string _writeOnly;
        public string Field;
    }

    [TestMethod]
    [DataRow(nameof(Garbage.Property))]
    [DataRow(nameof(Garbage.Field))]
    public void DeclaringType_Always_ReturnDeclaringType(string memberName)
    {
        //Arrange
        var propertyOrField = typeof(Garbage).GetSinglePropertyOrField(x => x.Name == memberName);

        //Act
        var result = propertyOrField.DeclaringType;

        //Assert
        result.Should().Be(typeof(Garbage).GetSingleMember(x => x.Name == memberName).DeclaringType);
    }

    [TestMethod]
    public void CanRead_WhenIsField_ReturnTrue()
    {
        //Arrange
        var propertyOrField = typeof(Garbage).GetSinglePropertyOrField(x => x.Name == nameof(Garbage.Field));

        //Act
        var result = propertyOrField.CanRead;

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void CanRead_WhenIsGetSetProperty_ReturnTrue()
    {
        //Arrange
        var propertyOrField = typeof(Garbage).GetSinglePropertyOrField(x => x.Name == nameof(Garbage.Property));

        //Act
        var result = propertyOrField.CanRead;

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void CanRead_WhenIsGetProperty_ReturnTrue()
    {
        //Arrange
        var propertyOrField = typeof(Garbage).GetSinglePropertyOrField(x => x.Name == nameof(Garbage.ReadOnlyProperty));

        //Act
        var result = propertyOrField.CanRead;

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void CanRead_WhenIsSetProperty_ReturnFalse()
    {
        //Arrange
        var propertyOrField = typeof(Garbage).GetSinglePropertyOrField(x => x.Name == nameof(Garbage.WriteOnly));

        //Act
        var result = propertyOrField.CanRead;

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void CanWrite_WhenIsField_ReturnTrue()
    {
        //Arrange
        var propertyOrField = typeof(Garbage).GetSinglePropertyOrField(x => x.Name == nameof(Garbage.Field));

        //Act
        var result = propertyOrField.CanWrite;

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void CanWrite_WhenIsGetSetProperty_ReturnTrue()
    {
        //Arrange
        var propertyOrField = typeof(Garbage).GetSinglePropertyOrField(x => x.Name == nameof(Garbage.Property));

        //Act
        var result = propertyOrField.CanWrite;

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void CanWrite_WhenIsGetProperty_ReturnTrue()
    {
        //Arrange
        var propertyOrField = typeof(Garbage).GetSinglePropertyOrField(x => x.Name == nameof(Garbage.ReadOnlyProperty));

        //Act
        var result = propertyOrField.CanWrite;

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void CanWrite_WhenIsSetProperty_ReturnFalse()
    {
        //Arrange
        var propertyOrField = typeof(Garbage).GetSinglePropertyOrField(x => x.Name == nameof(Garbage.WriteOnly));

        //Act
        var result = propertyOrField.CanWrite;

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void CanReadAndWrite_WhenIsField_ReturnTrue()
    {
        //Arrange
        var propertyOrField = typeof(Garbage).GetSinglePropertyOrField(x => x.Name == nameof(Garbage.Field));

        //Act
        var result = propertyOrField.CanReadAndWrite;

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void CanReadAndWrite_WhenIsGetSetProperty_ReturnTrue()
    {
        //Arrange
        var propertyOrField = typeof(Garbage).GetSinglePropertyOrField(x => x.Name == nameof(Garbage.Property));

        //Act
        var result = propertyOrField.CanReadAndWrite;

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void CanReadAndWrite_WhenIsGetProperty_ReturnTrue()
    {
        //Arrange
        var propertyOrField = typeof(Garbage).GetSinglePropertyOrField(x => x.Name == nameof(Garbage.ReadOnlyProperty));

        //Act
        var result = propertyOrField.CanReadAndWrite;

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void CanReadAndWrite_WhenIsSetProperty_ReturnFalse()
    {
        //Arrange
        var propertyOrField = typeof(Garbage).GetSinglePropertyOrField(x => x.Name == nameof(Garbage.WriteOnly));

        //Act
        var result = propertyOrField.CanReadAndWrite;

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void MemberType_Always_ReturnMemberInnerType()
    {
        //Arrange


        //Act

        //Assert
        var propertiesOrFields = typeof(Garbage).GetAllPropertiesOrFields().ToList();

        foreach (var propertyOrField in propertiesOrFields)
        {
            var member = typeof(Garbage).GetSingleMember(x => x.Name == propertyOrField.Name);
            propertyOrField.MemberType.Should().Be(member.MemberType);
        }
    }

    [TestMethod]
    public void Name_Always_ReturnInnerMemberName()
    {
        //Arrange

        //Act

        //Assert
        var propertiesOrFields = typeof(Garbage).GetAllPropertiesOrFields().ToList();

        foreach (var propertyOrField in propertiesOrFields)
        {
            var member = typeof(Garbage).GetSingleMember(x => x.Name == propertyOrField.Name);
            propertyOrField.Name.Should().Be(member.Name);
        }
    }

    [TestMethod]
    public void ReflectedType_Always_ReturnInnerMemberReflectedType()
    {
        //Arrange

        //Act

        //Assert
        var propertiesOrFields = typeof(Garbage).GetAllPropertiesOrFields().ToList();

        foreach (var propertyOrField in propertiesOrFields)
        {
            var member = typeof(Garbage).GetSingleMember(x => x.Name == propertyOrField.Name);
            propertyOrField.ReflectedType.Should().Be(member.ReflectedType);
        }
    }

    [TestMethod]
    public void Module_Always_ReturnInnerMemberModule()
    {
        //Arrange

        //Act

        //Assert
        var propertiesOrFields = typeof(Garbage).GetAllPropertiesOrFields().ToList();

        foreach (var propertyOrField in propertiesOrFields)
        {
            var member = typeof(Garbage).GetSingleMember(x => x.Name == propertyOrField.Name);
            propertyOrField.Module.Should().Be(member.Module);
        }
    }

    [TestMethod]
    public void HasSameMetadataDefinitionAs_Always_ThrowNotImplemented()
    {
        //Arrange

        //Act

        //Assert
        var propertiesOrFields = typeof(Garbage).GetAllPropertiesOrFields().ToList();

        foreach (var propertyOrField in propertiesOrFields)
        {
            var action = () => propertyOrField.HasSameMetadataDefinitionAs(Dummy.Create<MemberInfo>());
            action.Should().Throw<NotImplementedException>();
        }
    }

    [TestMethod]
    public void CustomAttributes_Always_ReturnInnerMemberAttributes()
    {
        //Arrange

        //Act

        //Assert
        var propertiesOrFields = typeof(Garbage).GetAllPropertiesOrFields().ToList();

        foreach (var propertyOrField in propertiesOrFields)
        {
            var member = typeof(Garbage).GetSingleMember(x => x.Name == propertyOrField.Name);
            propertyOrField.CustomAttributes.Select(x => x.AttributeType).Should().BeEquivalentTo(member.CustomAttributes.Select(x => x.AttributeType));
        }
    }

    [TestMethod]
    public void IsCollectible_Always_ReturnInnerMemberIsCollectible()
    {
        //Arrange

        //Act

        //Assert
        var propertiesOrFields = typeof(Garbage).GetAllPropertiesOrFields().ToList();

        foreach (var propertyOrField in propertiesOrFields)
        {
            var member = typeof(Garbage).GetSingleMember(x => x.Name == propertyOrField.Name);
            propertyOrField.IsCollectible.Should().Be(member.IsCollectible);
        }
    }

    [TestMethod]
    public void MetadataToken_Always_ReturnInnerMemberMetadataToken()
    {
        //Arrange

        //Act

        //Assert
        var propertiesOrFields = typeof(Garbage).GetAllPropertiesOrFields().ToList();

        foreach (var propertyOrField in propertiesOrFields)
        {
            var member = typeof(Garbage).GetSingleMember(x => x.Name == propertyOrField.Name);
            propertyOrField.MetadataToken.Should().Be(member.MetadataToken);
        }
    }

    [TestMethod]
    public void GetCustomAttributesData_Always_ReturnGetCustomAttributesData()
    {
        //Arrange

        //Act

        //Assert
        var propertiesOrFields = typeof(Garbage).GetAllPropertiesOrFields().ToList();

        foreach (var propertyOrField in propertiesOrFields)
        {
            var member = typeof(Garbage).GetSingleMember(x => x.Name == propertyOrField.Name);
            propertyOrField.GetCustomAttributesData().Select(x => x.AttributeType).Should().BeEquivalentTo(member.GetCustomAttributesData().Select(x => x.AttributeType));
        }
    }

    [TestMethod]
    public void IsStatic_WhenIsStaticProperty_ReturnTrue()
    {
        //Arrange
        var propertyOrField = typeof(Garbage).GetSinglePropertyOrField(x => x.Name == nameof(Garbage.StaticProperty));

        //Act
        var result = propertyOrField.IsStatic;

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void IsStatic_WhenIsStaticField_ReturnTrue()
    {
        //Arrange
        var propertyOrField = typeof(Garbage).GetSinglePropertyOrField(x => x.Name == nameof(Garbage.StaticField));

        //Act
        var result = propertyOrField.IsStatic;

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void IsStatic_WhenIsNonStaticProperty_ReturnFalse()
    {
        //Arrange
        var propertyOrField = typeof(Garbage).GetSinglePropertyOrField(x => x.Name == nameof(Garbage.Property));

        //Act
        var result = propertyOrField.IsStatic;

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void IsStatic_WhenIsNonStaticField_ReturnFalse()
    {
        //Arrange
        var propertyOrField = typeof(Garbage).GetSinglePropertyOrField(x => x.Name == nameof(Garbage.Field));

        //Act
        var result = propertyOrField.IsStatic;

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void IsInstance_WhenIsStaticProperty_ReturnFalse()
    {
        //Arrange
        var propertyOrField = typeof(Garbage).GetSinglePropertyOrField(x => x.Name == nameof(Garbage.StaticProperty));

        //Act
        var result = propertyOrField.IsInstance;

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void IsInstance_WhenIsStaticField_ReturnFalse()
    {
        //Arrange
        var propertyOrField = typeof(Garbage).GetSinglePropertyOrField(x => x.Name == nameof(Garbage.StaticField));

        //Act
        var result = propertyOrField.IsInstance;

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void IsInstance_WhenIsNonStaticProperty_ReturnTrue()
    {
        //Arrange
        var propertyOrField = typeof(Garbage).GetSinglePropertyOrField(x => x.Name == nameof(Garbage.Property));

        //Act
        var result = propertyOrField.IsInstance;

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void IsInstance_WhenIsNonStaticField_ReturnTrue()
    {
        //Arrange
        var propertyOrField = typeof(Garbage).GetSinglePropertyOrField(x => x.Name == nameof(Garbage.Field));

        //Act
        var result = propertyOrField.IsInstance;

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void IsPrivate_WhenIsPrivateProperty_ReturnTrue()
    {
        //Arrange
        var propertyOrField = typeof(Garbage).GetSinglePropertyOrField(x => x.Name == "PrivateProperty");

        //Act
        var result = propertyOrField.IsPrivate;

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void IsPrivate_WhenIsPrivateField_ReturnTrue()
    {
        //Arrange
        var propertyOrField = typeof(Garbage).GetSinglePropertyOrField(x => x.Name == "PrivateField");

        //Act
        var result = propertyOrField.IsPrivate;

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void IsPrivate_WhenIsNonPrivateProperty_ReturnFalse()
    {
        //Arrange
        var propertyOrField = typeof(Garbage).GetSinglePropertyOrField(x => x.Name == nameof(Garbage.Property));

        //Act
        var result = propertyOrField.IsPrivate;

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void IsPrivate_WhenIsNonPrivateField_ReturnFalse()
    {
        //Arrange
        var propertyOrField = typeof(Garbage).GetSinglePropertyOrField(x => x.Name == nameof(Garbage.Field));

        //Act
        var result = propertyOrField.IsPrivate;

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void IsPublic_WhenIsPublicProperty_ReturnTrue()
    {
        //Arrange
        var propertyOrField = typeof(Garbage).GetSinglePropertyOrField(x => x.Name == nameof(Garbage.Property));

        //Act
        var result = propertyOrField.IsPublic;

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void IsPublic_WhenIsPublicField_ReturnTrue()
    {
        //Arrange
        var propertyOrField = typeof(Garbage).GetSinglePropertyOrField(x => x.Name == nameof(Garbage.Field));

        //Act
        var result = propertyOrField.IsPublic;

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void IsPublic_WhenIsNonPublicProperty_ReturnFalse()
    {
        //Arrange
        var propertyOrField = typeof(Garbage).GetSinglePropertyOrField(x => x.Name == "PrivateProperty");

        //Act
        var result = propertyOrField.IsPublic;

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void IsPublic_WhenIsNonPublicField_ReturnFalse()
    {
        //Arrange
        var propertyOrField = typeof(Garbage).GetSinglePropertyOrField(x => x.Name == "PrivateField");

        //Act
        var result = propertyOrField.IsPublic;

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void IsProtected_WhenIsProtectedProperty_ReturnTrue()
    {
        //Arrange
        var propertyOrField = typeof(Garbage).GetSinglePropertyOrField(x => x.Name == "ProtectedProperty");

        //Act
        var result = propertyOrField.IsProtected;

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void IsProtected_WhenIsProtectedField_ReturnTrue()
    {
        //Arrange
        var propertyOrField = typeof(Garbage).GetSinglePropertyOrField(x => x.Name == "ProtectedField");

        //Act
        var result = propertyOrField.IsProtected;

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void IsProtected_WhenIsNonProtectedProperty_ReturnFalse()
    {
        //Arrange
        var propertyOrField = typeof(Garbage).GetSinglePropertyOrField(x => x.Name == nameof(Garbage.Property));

        //Act
        var result = propertyOrField.IsProtected;

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void IsProtected_WhenIsNonProtectedField_ReturnFalse()
    {
        //Arrange
        var propertyOrField = typeof(Garbage).GetSinglePropertyOrField(x => x.Name == nameof(Garbage.Field));

        //Act
        var result = propertyOrField.IsProtected;

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void IsInternal_WhenIsInternalProperty_ReturnTrue()
    {
        //Arrange
        var propertyOrField = typeof(Garbage).GetSinglePropertyOrField(x => x.Name == "InternalProperty");

        //Act
        var result = propertyOrField.IsInternal;

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void IsInternal_WhenIsInternalField_ReturnTrue()
    {
        //Arrange
        var propertyOrField = typeof(Garbage).GetSinglePropertyOrField(x => x.Name == "InternalField");

        //Act
        var result = propertyOrField.IsInternal;

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void IsInternal_WhenIsNonInternalProperty_ReturnFalse()
    {
        //Arrange
        var propertyOrField = typeof(Garbage).GetSinglePropertyOrField(x => x.Name == nameof(Garbage.Property));

        //Act
        var result = propertyOrField.IsInternal;

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void IsInternal_WhenIsNonInternalField_ReturnFalse()
    {
        //Arrange
        var propertyOrField = typeof(Garbage).GetSinglePropertyOrField(x => x.Name == nameof(Garbage.Field));

        //Act
        var result = propertyOrField.IsPrivate;

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void IsProperty_Always_ReturnTrueForPropertiesAndFalseForFields()
    {
        //Arrange

        //Act

        //Assert
        var propertiesOrFields = typeof(Garbage).GetAllPropertiesOrFields().ToList();

        foreach (var propertyOrField in propertiesOrFields)
        {
            var member = typeof(Garbage).GetSingleMember(x => x.Name == propertyOrField.Name);
            propertyOrField.IsProperty.Should().Be(member is PropertyInfo);
        }
    }

    [TestMethod]
    public void IsField_Always_ReturnFalseForPropertiesAndTrueForFields()
    {
        //Arrange

        //Act

        //Assert
        var propertiesOrFields = typeof(Garbage).GetAllPropertiesOrFields().ToList();

        foreach (var propertyOrField in propertiesOrFields)
        {
            var member = typeof(Garbage).GetSingleMember(x => x.Name == propertyOrField.Name);
            propertyOrField.IsField.Should().Be(member is FieldInfo);
        }
    }

    [TestMethod]
    public void IsAutomaticBackingField_WhenIsAutomaticBackingField_ReturnTrue()
    {
        //Arrange
        var instance = typeof(Garbage).GetSinglePropertyOrField(x => x.Name.StartsWith("<Property"));

        //Act
        var result = instance.IsAutomaticBackingField;

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void IsAutomaticBackingField_WhenIsProperty_ReturnFalse()
    {
        //Arrange
        var instance = typeof(Garbage).GetSinglePropertyOrField(x => x.Name == "Property");

        //Act
        var result = instance.IsAutomaticBackingField;

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void IsAutomaticBackingField_WhenIsBackingFieldButNotAutomatic_ReturnFalse()
    {
        //Arrange
        var instance = typeof(Garbage).GetSinglePropertyOrField(x => x.Name == "_writeOnly");

        //Act
        var result = instance.IsAutomaticBackingField;

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void IsBackingField_WhenIsAutomaticBackingField_ReturnTrue()
    {
        //Arrange
        var instance = typeof(Garbage).GetSinglePropertyOrField(x => x.Name.StartsWith("<Property"));

        //Act
        var result = instance.IsBackingField();

        //Assert
        result.Should().BeTrue();

    }

    [TestMethod]
    public void IsBackingField_WhenIsNonAutomaticBackingField_ReturnTrue()
    {
        //Arrange
        var instance = typeof(Garbage).GetSinglePropertyOrField(x => x.Name == "_writeOnly");

        //Act
        var result = instance.IsBackingField();

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void IsBackingField_WhenIsProperty_ReturnFalse()
    {
        //Arrange
        var instance = typeof(Garbage).GetSinglePropertyOrField(x => x.Name == "Property");

        //Act
        var result = instance.IsBackingField();

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void IsBackingField_WhenIsRegularField_ReturnFalse()
    {
        //Arrange
        var instance = typeof(Garbage).GetSinglePropertyOrField(x => x.Name == "Field");

        //Act
        var result = instance.IsBackingField();

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void TryGetValue_WhenIsField_ReturnFieldValue()
    {
        //Arrange
        var member = typeof(Garbage).GetSinglePropertyOrField(x => x.Name == "Field");

        var instance = Dummy.Create<Garbage>();

        //Act
        var result = member.TryGetValue(instance);

        //Assert
        result.Should().Be(Result<object>.Success(instance.Field));
    }

    [TestMethod]
    public void TryGetValue_WhenIsReadOnlyProperty_ReturnValue()
    {
        //Arrange
        var member = typeof(Garbage).GetSinglePropertyOrField(x => x.Name == nameof(Garbage.ReadOnlyProperty));

        var instance = Dummy.Create<Garbage>();

        //Act
        var result = member.TryGetValue(instance);

        //Assert
        result.Should().Be(Result<object>.Success(instance.ReadOnlyProperty));
    }


    [TestMethod]
    public void TryGetValue_WhenIsGetSetProperty_ReturnValue()
    {
        //Arrange
        var member = typeof(Garbage).GetSinglePropertyOrField(x => x.Name == nameof(Garbage.Property));

        var instance = Dummy.Create<Garbage>();

        //Act
        var result = member.TryGetValue(instance);

        //Assert
        result.Should().Be(Result<object>.Success(instance.Property));
    }

    [TestMethod]
    public void TryGetValue_WhenIsWriteOnlyProperty_ReturnFailure()
    {
        //Arrange
        var member = typeof(Garbage).GetSinglePropertyOrField(x => x.Name == nameof(Garbage.WriteOnly));

        var instance = Dummy.Create<Garbage>();

        //Act
        var result = member.TryGetValue(instance);

        //Assert
        result.Should().Be(Result<object>.Failure());
    }

    [TestMethod]
    public void GetValue_WhenIsField_ReturnFieldValue()
    {
        //Arrange
        var member = typeof(Garbage).GetSinglePropertyOrField(x => x.Name == "Field");

        var instance = Dummy.Create<Garbage>();

        //Act
        var result = member.GetValue(instance);

        //Assert
        result.Should().Be(instance.Field);
    }

    [TestMethod]
    public void GetValue_WhenIsReadOnlyProperty_ReturnValue()
    {
        //Arrange
        var member = typeof(Garbage).GetSinglePropertyOrField(x => x.Name == nameof(Garbage.ReadOnlyProperty));

        var instance = Dummy.Create<Garbage>();

        //Act
        var result = member.GetValue(instance);

        //Assert
        result.Should().Be(instance.ReadOnlyProperty);
    }


    [TestMethod]
    public void GetValue_WhenIsGetSetProperty_ReturnValue()
    {
        //Arrange
        var member = typeof(Garbage).GetSinglePropertyOrField(x => x.Name == nameof(Garbage.Property));

        var instance = Dummy.Create<Garbage>();

        //Act
        var result = member.GetValue(instance);

        //Assert
        result.Should().Be(instance.Property);
    }

    [TestMethod]
    public void GetValue_WhenIsWriteOnlyProperty_Throws()
    {
        //Arrange
        var member = typeof(Garbage).GetSinglePropertyOrField(x => x.Name == nameof(Garbage.WriteOnly));

        var instance = Dummy.Create<Garbage>();

        //Act
        var action = () => member.GetValue(instance);

        //Assert
        action.Should().Throw<InvalidOperationException>(string.Format(Exceptions.UsingGetOnWriteOnlyProperty, member.Name));
    }

    [TestMethod]
    public void TrySetValue_WhenField_SetValue()
    {
        //Arrange
        var member = typeof(Garbage).GetSinglePropertyOrField(x => x.Name == "Field");

        var instance = Dummy.Create<Garbage>();
        var value = Dummy.Create<string>();

        //Act
        member.TrySetValue(instance, value);

        //Assert
        member.GetValue(instance).Should().Be(value);
    }

    [TestMethod]
    public void TrySetValue_WhenGetSetProperty_SetValue()
    {
        //Arrange
        var member = typeof(Garbage).GetSinglePropertyOrField(x => x.Name == "Property");

        var instance = Dummy.Create<Garbage>();
        var value = Dummy.Create<string>();

        //Act
        member.TrySetValue(instance, value);

        //Assert
        member.GetValue(instance).Should().Be(value);
    }

    [TestMethod]
    public void TrySetValue_WhenReadOnlyProperty_DoNotThrow()
    {
        //Arrange
        var member = typeof(Garbage).GetSinglePropertyOrField(x => x.Name == "ReadOnlyProperty");

        var instance = Dummy.Create<Garbage>();
        var value = Dummy.Create<string>();

        //Act
        var action = () => member.TrySetValue(instance, value);

        //Assert
        action.Should().NotThrow();
    }

    [TestMethod]
    public void TrySetValue_WhenWriteOnlyProperty_SetValue()
    {
        //Arrange
        var member = typeof(Garbage).GetSinglePropertyOrField(x => x.Name == "WriteOnly");

        var instance = Dummy.Create<Garbage>();
        var value = Dummy.Create<string>();

        //Act
        member.TrySetValue(instance, value);

        //Assert
        typeof(Garbage).GetSingleField(x => x.Name == "_writeOnly").GetValue(instance).Should().Be(value);
    }

    [TestMethod]
    public void SetValue_WhenField_SetValue()
    {
        //Arrange
        var member = typeof(Garbage).GetSinglePropertyOrField(x => x.Name == "Field");

        var instance = Dummy.Create<Garbage>();
        var value = Dummy.Create<string>();

        //Act
        member.SetValue(instance, value);

        //Assert
        member.GetValue(instance).Should().Be(value);
    }

    [TestMethod]
    public void SetValue_WhenGetSetProperty_SetValue()
    {
        //Arrange
        var member = typeof(Garbage).GetSinglePropertyOrField(x => x.Name == "Property");

        var instance = Dummy.Create<Garbage>();
        var value = Dummy.Create<string>();

        //Act
        member.SetValue(instance, value);

        //Assert
        member.GetValue(instance).Should().Be(value);
    }

    [TestMethod]
    public void SetValue_WhenReadOnlyProperty_Throw()
    {
        //Arrange
        var member = typeof(Garbage).GetSinglePropertyOrField(x => x.Name == "ReadOnlyProperty");

        var instance = Dummy.Create<Garbage>();
        var value = Dummy.Create<string>();

        //Act
        var action = () => member.SetValue(instance, value);

        //Assert
        action.Should().Throw<InvalidOperationException>().WithMessage(string.Format(Exceptions.UsingSetOnReadOnlyProperty, "ReadOnlyProperty"));
    }

    [TestMethod]
    public void SetValue_WhenWriteOnlyProperty_SetValue()
    {
        //Arrange
        var member = typeof(Garbage).GetSinglePropertyOrField(x => x.Name == "WriteOnly");

        var instance = Dummy.Create<Garbage>();
        var value = Dummy.Create<string>();

        //Act
        member.SetValue(instance, value);

        //Assert
        typeof(Garbage).GetSingleField(x => x.Name == "_writeOnly").GetValue(instance).Should().Be(value);
    }

    [TestMethod]
    public void TryAsProperty_WhenField_ReturnFailure()
    {
        //Arrange
        var member = typeof(Garbage).GetSinglePropertyOrField(x => x.Name == "Field");

        //Act
        var result = member.TryAsProperty();

        //Assert
        result.Should().Be(Result<PropertyInfo>.Failure());
    }

    [TestMethod]
    public void TryAsProperty_WhenGetSetProperty_ReturnProperty()
    {
        //Arrange
        var member = typeof(Garbage).GetSinglePropertyOrField(x => x.Name == "Property");

        //Act
        var result = member.TryAsProperty();

        //Assert
        result.Should().Be(Result<PropertyInfo>.Success(typeof(Garbage).GetSingleProperty("Property")));
    }

    [TestMethod]
    public void TryAsProperty_WhenReadOnlyProperty_ReturnProperty()
    {
        //Arrange
        var member = typeof(Garbage).GetSinglePropertyOrField(x => x.Name == "ReadOnlyProperty");

        //Act
        var result = member.TryAsProperty();

        //Assert
        result.Should().Be(Result<PropertyInfo>.Success(typeof(Garbage).GetSingleProperty("ReadOnlyProperty")));
    }

    [TestMethod]
    public void TryAsProperty_WhenWriteOnlyProperty_ReturnProperty()
    {
        //Arrange
        var member = typeof(Garbage).GetSinglePropertyOrField(x => x.Name == "WriteOnly");

        //Act
        var result = member.TryAsProperty();

        //Assert
        result.Should().Be(Result<PropertyInfo>.Success(typeof(Garbage).GetSingleProperty("WriteOnly")));
    }

    [TestMethod]
    public void AsProperty_WhenField_Throw()
    {
        //Arrange
        var member = typeof(Garbage).GetSinglePropertyOrField(x => x.Name == "Field");

        //Act
        var action = () => member.AsProperty();

        //Assert
        action.Should().Throw<InvalidCastException>().WithMessage(string.Format(Exceptions.MemberCannotBeCastToProperty, "Field"));
    }

    [TestMethod]
    public void AsProperty_WhenGetSetProperty_ReturnProperty()
    {
        //Arrange
        var member = typeof(Garbage).GetSinglePropertyOrField(x => x.Name == "Property");

        //Act
        var result = member.AsProperty();

        //Assert
        result.Should().BeSameAs(typeof(Garbage).GetSingleProperty("Property"));
    }

    [TestMethod]
    public void AsProperty_WhenReadOnlyProperty_ReturnProperty()
    {
        //Arrange
        var member = typeof(Garbage).GetSinglePropertyOrField(x => x.Name == "ReadOnlyProperty");

        //Act
        var result = member.AsProperty();

        //Assert
        result.Should().BeSameAs(typeof(Garbage).GetSingleProperty("ReadOnlyProperty"));
    }

    [TestMethod]
    public void AsProperty_WhenWriteOnlyProperty_ReturnProperty()
    {
        //Arrange
        var member = typeof(Garbage).GetSinglePropertyOrField(x => x.Name == "WriteOnly");

        //Act
        var result = member.AsProperty();

        //Assert
        result.Should().BeSameAs(typeof(Garbage).GetSingleProperty("WriteOnly"));
    }




    [TestMethod]
    public void TryAsField_WhenField_ReturnFailure()
    {
        //Arrange
        var member = typeof(Garbage).GetSinglePropertyOrField(x => x.Name == "Field");

        //Act
        var result = member.TryAsField();

        //Assert
        result.Should().Be(Result<FieldInfo>.Success(typeof(Garbage).GetSingleField("Field")));
    }

    [TestMethod]
    public void TryAsField_WhenGetSetProperty_ReturnProperty()
    {
        //Arrange
        var member = typeof(Garbage).GetSinglePropertyOrField(x => x.Name == "Property");

        //Act
        var result = member.TryAsField();

        //Assert
        result.Should().Be(Result<FieldInfo>.Failure());
    }

    [TestMethod]
    public void TryAsField_WhenReadOnlyProperty_ReturnProperty()
    {
        //Arrange
        var member = typeof(Garbage).GetSinglePropertyOrField(x => x.Name == "ReadOnlyProperty");

        //Act
        var result = member.TryAsField();

        //Assert
        result.Should().Be(Result<FieldInfo>.Failure());
    }

    [TestMethod]
    public void TryAsField_WhenWriteOnlyProperty_ReturnProperty()
    {
        //Arrange
        var member = typeof(Garbage).GetSinglePropertyOrField(x => x.Name == "WriteOnly");

        //Act
        var result = member.TryAsField();

        //Assert
        result.Should().Be(Result<FieldInfo>.Failure());
    }

    [TestMethod]
    public void AsField_WhenField_Throw()
    {
        //Arrange
        var member = typeof(Garbage).GetSinglePropertyOrField(x => x.Name == "Field");

        //Act
        var result = member.AsField();

        //Assert
        result.Should().BeSameAs(typeof(Garbage).GetSingleField("Field"));
    }

    [TestMethod]
    public void AsField_WhenGetSetProperty_ReturnProperty()
    {
        //Arrange
        var member = typeof(Garbage).GetSinglePropertyOrField(x => x.Name == "Property");

        //Act
        var action = () => member.AsField();

        //Assert
        action.Should().Throw<InvalidCastException>().WithMessage(string.Format(Exceptions.MemberCannotBeCastToField, "Property"));
    }

    [TestMethod]
    public void AsField_WhenReadOnlyProperty_ReturnProperty()
    {
        //Arrange
        var member = typeof(Garbage).GetSinglePropertyOrField(x => x.Name == "ReadOnlyProperty");

        //Act
        var action = () => member.AsField();

        //Assert
        action.Should().Throw<InvalidCastException>().WithMessage(string.Format(Exceptions.MemberCannotBeCastToField, "ReadOnlyProperty"));
    }

    [TestMethod]
    public void AsField_WhenWriteOnlyProperty_ReturnProperty()
    {
        //Arrange
        var member = typeof(Garbage).GetSinglePropertyOrField(x => x.Name == "WriteOnly");

        //Act
        var action = () => member.AsField();

        //Assert
        action.Should().Throw<InvalidCastException>().WithMessage(string.Format(Exceptions.MemberCannotBeCastToField, "WriteOnly"));
    }

    //TODO Test equality
    [TestMethod]
    public void Equality_WhenAreSameReference_DoNotThrow()
    {
        //Arrange
        var member = Dummy.Create<PropertyOrField>();
        IPropertyOrField other = member;

        //Act
        //Assert
        Ensure.Equality(member, other);
    }
}