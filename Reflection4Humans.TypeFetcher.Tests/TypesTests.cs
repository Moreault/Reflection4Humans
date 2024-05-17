namespace Reflection4Humans.TypeFetcher.Tests;

[TestClass]
public class TypesTests : Tester
{
    [TestMethod]
    public void From_WhenAssemblyIsNull_Throw()
    {
        //Arrange
        Assembly assembly = null!;

        //Act
        var action = () => Types.From(assembly);

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(assembly));
    }

    [TestMethod]
    public void From_WhenIsCurrentAssembly_ReturnTypesFromCurrentAssembly()
    {
        //Arrange

        //Act
        var result = Types.From(Assembly.GetExecutingAssembly()).ToList();

        //Assert
        result.Should().Contain(new List<Type>
        {
            typeof(AbstractGarbage),
            typeof(Garbage.Garbage),
            typeof(GarbageAttribute),
            typeof(GarbageAbstractAttribute),
            typeof(StaticGarbage),
        });
    }

    [TestMethod]
    public void Where_WhenPredicateIsNull_Throw()
    {
        //Arrange
        Func<Type, bool> predicate = null!;

        //Act
        var action = () => Types.Where(predicate);

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(predicate));
    }

    [TestMethod]
    public void Where_WhenIsClass_ReturnOnlyClassesAbstractOrNot()
    {
        //Arrange

        //Act
        var result = Types.Where(x => x.IsClass).ToList();

        //Assert
        result.Should().Contain(new List<Type>
            {
                typeof(AbstractGarbage),
                typeof(Garbage.Garbage),
                typeof(GarbageAttribute),
                typeof(GarbageAbstractAttribute),
                typeof(StaticGarbage),
            });

        result.Should().NotContain(new List<Type>
            {
                typeof(IGarbage),
                typeof(GarbageStruct),
                typeof(GarbageEnum),
            });
    }

    [TestMethod]
    public void Where_WhenIsNotClass_ReturnEverythingThatIsNotAClass()
    {
        //Arrange

        //Act
        var result = Types.Where(x => !x.IsClass).ToList();

        //Assert
        result.Should().Contain(new List<Type>
            {
                typeof(IGarbage),
                typeof(GarbageStruct),
                typeof(GarbageEnum),
            });

        result.Should().NotContain(new List<Type>
            {
                typeof(AbstractGarbage),
                typeof(Garbage.Garbage),
                typeof(GarbageAttribute),
                typeof(GarbageAbstractAttribute),
                typeof(StaticGarbage),
            });
    }

    [TestMethod]
    public void Where_WhenIsClassAndAbstract_ReturnOnlyAbstractClasses()
    {
        //Arrange

        //Act
        var result = Types.Where(x => x.IsClass && x.IsAbstract).ToList();

        //Assert
        result.Should().Contain(new List<Type>
            {
                typeof(AbstractGarbage),
                typeof(GarbageAbstractAttribute),
                typeof(StaticGarbage),
            });

        result.Should().NotContain(new List<Type>
            {
                typeof(IGarbage),
                typeof(Garbage.Garbage),
                typeof(GarbageAttribute),
                typeof(GarbageStruct),
                typeof(GarbageEnum),
            });
    }

    [TestMethod]
    public void Where_WhenIsClassAndNotAbstract_ReturnOnlyNonAbstractClasses()
    {
        //Arrange

        //Act
        var result = Types.Where(x => x.IsClass && !x.IsAbstract).ToList();

        //Assert
        result.Should().Contain(new List<Type>
            {
                typeof(Garbage.Garbage),
                typeof(GarbageAttribute),
            });

        result.Should().NotContain(new List<Type>
            {
                typeof(AbstractGarbage),
                typeof(IGarbage),
                typeof(GarbageStruct),
                typeof(GarbageEnum),
                typeof(GarbageAbstractAttribute),
                typeof(StaticGarbage),
            });
    }

    [TestMethod]
    public void Where_WhenIsNotClassAndIsAbstract_ReturnOnlyInterfaces()
    {
        //Arrange

        //Act
        var result = Types.Where(x => !x.IsClass && x.IsAbstract).ToList();

        //Assert
        result.Should().Contain(new List<Type>
            {
                typeof(IGarbage),
            });

        result.Should().NotContain(new List<Type>
            {
                typeof(AbstractGarbage),
                typeof(GarbageStruct),
                typeof(GarbageEnum),
                typeof(Garbage.Garbage),
                typeof(GarbageAttribute),
                typeof(GarbageAbstractAttribute),
                typeof(StaticGarbage),
            });
    }

    [TestMethod]
    public void Where_WhenIsAbstract_ReturnOnlyInterfacesAndAbstractClasses()
    {
        //Arrange

        //Act
        var result = Types.Where(x => x.IsAbstract).ToList();

        //Assert
        result.Should().Contain(new List<Type>
            {
                typeof(IGarbage),
                typeof(AbstractGarbage),
                typeof(GarbageAbstractAttribute),
                typeof(StaticGarbage),
            });

        result.Should().NotContain(new List<Type>
            {
                typeof(GarbageStruct),
                typeof(GarbageEnum),
                typeof(Garbage.Garbage),
                typeof(GarbageAttribute),
            });
    }

    [TestMethod]
    public void Where_WhenIsInterface_ReturnOnlyInterfaces()
    {
        //Arrange

        //Act
        var result = Types.Where(x => x.IsInterface).ToList();

        //Assert
        result.Should().Contain(new List<Type>
            {
                typeof(IGarbage),
            });

        result.Should().NotContain(new List<Type>
            {
                typeof(GarbageStruct),
                typeof(GarbageEnum),
                typeof(Garbage.Garbage),
                typeof(GarbageAttribute),
                typeof(AbstractGarbage),
                typeof(GarbageAbstractAttribute),
                typeof(StaticGarbage),
            });
    }

    [TestMethod]
    public void Where_WhenIsClassAndInterfaces_ReturnEmpty()
    {
        //Arrange

        //Act
        var result = Types.Where(x => x.IsClass && x.IsInterface).ToList();

        //Assert
        //It is impossible to both class AND interface
        result.Should().BeEmpty();
    }

    [TestMethod]
    public void Where_WhenIsAttribute_ReturnOnlyAttributes()
    {
        //Arrange

        //Act
        var result = Types.Where(x => x.IsAttribute()).ToList();

        //Assert
        result.Should().Contain(new List<Type>
            {
                typeof(GarbageAttribute),
                typeof(GarbageAbstractAttribute),
            });

        result.Should().NotContain(new List<Type>
            {
                typeof(GarbageStruct),
                typeof(GarbageEnum),
                typeof(Garbage.Garbage),
                typeof(AbstractGarbage),
                typeof(IGarbage),
                typeof(StaticGarbage),
            });
    }

    [TestMethod]
    public void Where_WhenIsAttributeAndInterface_ReturnEmpty()
    {
        //Arrange

        //Act
        var result = Types.Where(x => x.IsInterface && x.IsAttribute()).ToList();

        //Assert
        //It is impossible to both class AND interface
        result.Should().BeEmpty();
    }

    [TestMethod]
    public void Where_WhenIsAttributeAndAbstract_ReturnOnlyAbstractAttributes()
    {
        //Arrange

        //Act
        var result = Types.Where(x => x.IsAttribute() && x.IsAbstract).ToList();

        //Assert
        result.Should().Contain(new List<Type>
            {
                typeof(GarbageAbstractAttribute),
            });

        result.Should().NotContain(new List<Type>
            {
                typeof(GarbageStruct),
                typeof(GarbageEnum),
                typeof(Garbage.Garbage),
                typeof(AbstractGarbage),
                typeof(IGarbage),
                typeof(GarbageAttribute),
                typeof(StaticGarbage),
            });
    }

    [TestMethod]
    public void Where_WhenIsAttributeAndClassAndAbstract_ReturnOnlyAbstractAttributes()
    {
        //Arrange

        //Act
        var result = Types.Where(x => x.IsAttribute() && x.IsAbstract && x.IsClass).ToList();

        //Assert
        result.Should().Contain(new List<Type>
            {
                typeof(GarbageAbstractAttribute),
            });

        result.Should().NotContain(new List<Type>
            {
                typeof(GarbageStruct),
                typeof(GarbageEnum),
                typeof(Garbage.Garbage),
                typeof(AbstractGarbage),
                typeof(IGarbage),
                typeof(GarbageAttribute),
                typeof(StaticGarbage),
            });
    }

    [TestMethod]
    public void Where_WhenIsAttributeAndNotAbstract_ReturnOnlyNonAbstractAttributes()
    {
        //Arrange

        //Act
        var result = Types.Where(x => x.IsAttribute() && !x.IsAbstract).ToList();

        //Assert
        result.Should().Contain(new List<Type>
            {
                typeof(GarbageAttribute),
            });

        result.Should().NotContain(new List<Type>
            {
                typeof(GarbageStruct),
                typeof(GarbageEnum),
                typeof(Garbage.Garbage),
                typeof(AbstractGarbage),
                typeof(IGarbage),
                typeof(GarbageAbstractAttribute),
                typeof(StaticGarbage),
            });
    }

    [TestMethod]
    public void Where_WhenIsNotAttribute_ReturnEverythingThatIsNotAnAttribute()
    {
        //Arrange

        //Act
        var result = Types.Where(x => !x.IsAttribute()).ToList();

        //Assert
        result.Should().Contain(new List<Type>
            {
                typeof(GarbageStruct),
                typeof(GarbageEnum),
                typeof(Garbage.Garbage),
                typeof(AbstractGarbage),
                typeof(IGarbage),
                typeof(StaticGarbage),
            });

        result.Should().NotContain(new List<Type>
            {
                typeof(GarbageAbstractAttribute),
                typeof(GarbageAttribute),
            });
    }

    [TestMethod]
    public void Where_WhenIsNotInterface_ReturnEverythingThatIsNotAnInterface()
    {
        //Arrange

        //Act
        var result = Types.Where(x => !x.IsInterface).ToList();

        //Assert
        result.Should().Contain(new List<Type>
            {
                typeof(GarbageStruct),
                typeof(GarbageEnum),
                typeof(Garbage.Garbage),
                typeof(AbstractGarbage),
                typeof(GarbageAbstractAttribute),
                typeof(GarbageAttribute),
                typeof(StaticGarbage),
            });

        result.Should().NotContain(new List<Type>
            {
                typeof(IGarbage),
            });
    }

    [TestMethod]
    public void Where_WhenIsNotStruct_ReturnEverythingThatIsNotAStruct()
    {
        //Arrange

        //Act
        var result = Types.Where(x => !x.IsValueType).ToList();

        //Assert
        result.Should().Contain(new List<Type>
            {
                typeof(IGarbage),
                typeof(Garbage.Garbage),
                typeof(AbstractGarbage),
                typeof(GarbageAbstractAttribute),
                typeof(GarbageAttribute),
                typeof(StaticGarbage),
            });

        result.Should().NotContain(new List<Type>
            {
                typeof(GarbageStruct),
                typeof(GarbageEnum),
            });
    }

    [TestMethod]
    public void Where_WhenIsStruct_ReturnStructsAndEnums()
    {
        //Arrange

        //Act
        var result = Types.Where(x => x.IsValueType).ToList();

        //Assert
        result.Should().Contain(new List<Type>
            {
                typeof(GarbageStruct),
                typeof(GarbageEnum),

            });

        result.Should().NotContain(new List<Type>
            {
                typeof(IGarbage),
                typeof(Garbage.Garbage),
                typeof(AbstractGarbage),
                typeof(GarbageAbstractAttribute),
                typeof(GarbageAttribute),
                typeof(StaticGarbage),
            });
    }

    [TestMethod]
    public void Where_WhenIsEnum_ReturnEnums()
    {
        //Arrange

        //Act
        var result = Types.Where(x => x.IsEnum).ToList();

        //Assert
        result.Should().Contain(new List<Type>
            {
                typeof(GarbageEnum),

            });

        result.Should().NotContain(new List<Type>
            {
                typeof(GarbageStruct),
                typeof(IGarbage),
                typeof(Garbage.Garbage),
                typeof(AbstractGarbage),
                typeof(GarbageAbstractAttribute),
                typeof(GarbageAttribute),
                typeof(StaticGarbage),
            });
    }

    [TestMethod]
    public void Where_WhenIsNotEnum_ReturnEverythingButEnums()
    {
        //Arrange

        //Act
        var result = Types.Where(x => !x.IsEnum).ToList();

        //Assert
        result.Should().Contain(new List<Type>
            {
                typeof(GarbageStruct),
                typeof(IGarbage),
                typeof(Garbage.Garbage),
                typeof(AbstractGarbage),
                typeof(GarbageAbstractAttribute),
                typeof(GarbageAttribute),
                typeof(StaticGarbage),
            });

        result.Should().NotContain(new List<Type>
            {
                typeof(GarbageEnum),
            });
    }

    [TestMethod]
    public void Where_WhenIsGeneric_ReturnAllGenerics()
    {
        //Arrange

        //Act
        var result = Types.Where(x => x.IsGenericType).ToList();

        //Assert
        result.Should().Contain(new List<Type>
            {
                typeof(GenericGarbage<>),
                typeof(IGenericGarbage<>),
                typeof(GenericGarbageStruct<>),

            });

        result.Should().NotContain(new List<Type>
            {
                typeof(GarbageStruct),
                typeof(IGarbage),
                typeof(Garbage.Garbage),
                typeof(AbstractGarbage),
                typeof(GarbageAbstractAttribute),
                typeof(GarbageAttribute),
                typeof(GarbageEnum),
                typeof(StaticGarbage),
            });
    }

    [TestMethod]
    public void Where_WhenIsNotGeneric_ReturnAllExceptGenerics()
    {
        //Arrange

        //Act
        var result = Types.Where(x => !x.IsGenericType).ToList();

        //Assert
        result.Should().Contain(new List<Type>
            {
                typeof(GarbageStruct),
                typeof(IGarbage),
                typeof(Garbage.Garbage),
                typeof(AbstractGarbage),
                typeof(GarbageAbstractAttribute),
                typeof(GarbageAttribute),
                typeof(GarbageEnum),
                typeof(StaticGarbage),
            });

        result.Should().NotContain(new List<Type>
            {
                typeof(GenericGarbage<>),
                typeof(IGenericGarbage<>),
                typeof(GenericGarbageStruct<>),
            });
    }

    [TestMethod]
    public void Where_WhenIsGenericTypeDefinition_ReturnAllGenericTypeDefinitions()
    {
        //Arrange

        //Act
        var result = Types.Where(x => x.IsGenericTypeDefinition).ToList();

        //Assert
        result.Should().Contain(new List<Type>
            {
                typeof(GenericGarbage<>),
                typeof(IGenericGarbage<>),
                typeof(GenericGarbageStruct<>),
            });

        result.Should().NotContain(new List<Type>
            {
                typeof(GarbageStruct),
                typeof(IGarbage),
                typeof(Garbage.Garbage),
                typeof(AbstractGarbage),
                typeof(GarbageAbstractAttribute),
                typeof(GarbageAttribute),
                typeof(GarbageEnum),
                typeof(StaticGarbage),
            });
    }

    [TestMethod]
    public void Where_WhenIsNotGenericTypeDefinition_ReturnAllExceptGenericTypeDefinitions()
    {
        //Arrange

        //Act
        var result = Types.Where(x => !x.IsGenericTypeDefinition).ToList();

        //Assert
        result.Should().Contain(new List<Type>
            {
                typeof(GarbageStruct),
                typeof(IGarbage),
                typeof(Garbage.Garbage),
                typeof(AbstractGarbage),
                typeof(GarbageAbstractAttribute),
                typeof(GarbageAttribute),
                typeof(GarbageEnum),
                typeof(StaticGarbage),
            });

        result.Should().NotContain(new List<Type>
            {
                typeof(GenericGarbage<>),
                typeof(IGenericGarbage<>),
                typeof(GenericGarbageStruct<>),
            });
    }

    [TestMethod]
    public void Where_WhenImplementsInterface_ReturnEverythingThatImplementsInterface()
    {
        //Arrange

        //Act
        var result = Types.Where(x => x.Implements<IGarbage>()).ToList();

        //Assert
        result.Should().Contain(new List<Type>
            {
                typeof(Garbage.Garbage),
            });

        result.Should().NotContain(new List<Type>
            {
                typeof(GarbageEnum),
                typeof(GarbageStruct),
                typeof(IGarbage),
                typeof(AbstractGarbage),
                typeof(GarbageAbstractAttribute),
                typeof(GarbageAttribute),
                typeof(StaticGarbage),
            });
    }

    [TestMethod]
    public void Where_WhenHasAttribute_ReturnEverthingThatHasAttribute()
    {
        //Arrange

        //Act
        var result = Types.Where(x => x.HasAttribute<GarbageAttribute>()).ToList();

        //Assert
        result.Should().Contain(new List<Type>
            {
                typeof(AttributedGarbage),
            });

        result.Should().NotContain(new List<Type>
            {
                typeof(Garbage.Garbage),
                typeof(GarbageEnum),
                typeof(GarbageStruct),
                typeof(IGarbage),
                typeof(AbstractGarbage),
                typeof(GarbageAbstractAttribute),
                typeof(GarbageAttribute),
                typeof(StaticGarbage),
            });
    }

    [TestMethod]
    public void Where_WhenIsStatic_ReturnAllStaticClasses()
    {
        //Arrange

        //Act
        var result = Types.Where(x => x.IsStatic()).ToList();

        //Assert
        result.Should().Contain(new List<Type>
            {
                typeof(StaticGarbage),
            });

        result.Should().NotContain(new List<Type>
            {
                typeof(Garbage.Garbage),
                typeof(GarbageEnum),
                typeof(GarbageStruct),
                typeof(IGarbage),
                typeof(AbstractGarbage),
                typeof(GarbageAbstractAttribute),
                typeof(GarbageAttribute),
                typeof(AttributedGarbage),
            });
    }

    [TestMethod]
    public void Where_WhenIsNotStatic_ReturnAllNonStaticClasses()
    {
        //Arrange

        //Act
        var result = Types.Where(x => !x.IsStatic()).ToList();

        //Assert
        result.Should().Contain(new List<Type>
            {
                typeof(Garbage.Garbage),
                typeof(GarbageEnum),
                typeof(GarbageStruct),
                typeof(IGarbage),
                typeof(AbstractGarbage),
                typeof(GarbageAbstractAttribute),
                typeof(GarbageAttribute),
                typeof(AttributedGarbage),
            });

        result.Should().NotContain(new List<Type>
            {
                typeof(StaticGarbage),
            });
    }

    [TestMethod]
    public void Where_WhenIsPublic_ReturnPublicTypes()
    {
        //Arrange

        //Act
        var result = Types.Where(x => x.IsPublic).ToList();

        //Assert
        result.Should().Contain(new List<Type>
            {
                typeof(Garbage.Garbage),
                typeof(GarbageEnum),
                typeof(GarbageStruct),
                typeof(IGarbage),
                typeof(AbstractGarbage),
                typeof(GarbageAbstractAttribute),
                typeof(GarbageAttribute),
                typeof(AttributedGarbage),
                typeof(StaticGarbage),
                typeof(PublicGarbage)
            });

        result.Should().NotContain(new List<Type>
            {
                typeof(PublicGarbage.IInternalGarbage),
            });
    }

    [TestMethod]
    public void Where_WhenIsPrivate_ReturnPrivateTypes()
    {
        //Arrange

        //Act
        var result = Types.Where(x => x.IsPrivate()).ToList();

        //Assert
        result.Select(x => x.Name).Should().Contain(new List<string>
            {
                "PrivateGarbage"
            });

        result.Should().NotContain(new List<Type>
            {
                typeof(Garbage.Garbage),
                typeof(GarbageEnum),
                typeof(GarbageStruct),
                typeof(IGarbage),
                typeof(AbstractGarbage),
                typeof(GarbageAbstractAttribute),
                typeof(GarbageAttribute),
                typeof(AttributedGarbage),
                typeof(StaticGarbage),
                typeof(PublicGarbage),
                typeof(PublicGarbage.IInternalGarbage),
            });
    }

    [TestMethod]
    public void Where_WhenIsProtected_ReturnProtectedTypes()
    {
        //Arrange

        //Act
        var result = Types.Where(x => x.IsProtected()).ToList();

        //Assert
        result.Select(x => x.Name).Should().Contain(new List<string>
            {
                "ProtectedGarbage"
            });

        result.Should().NotContain(new List<Type>
            {
                typeof(Garbage.Garbage),
                typeof(GarbageEnum),
                typeof(GarbageStruct),
                typeof(IGarbage),
                typeof(AbstractGarbage),
                typeof(GarbageAbstractAttribute),
                typeof(GarbageAttribute),
                typeof(AttributedGarbage),
                typeof(StaticGarbage),
                typeof(PublicGarbage),
                typeof(PublicGarbage.IInternalGarbage),
            });
    }

    [TestMethod]
    public void Where_WhenIsInternal_ReturnInternalTypes()
    {
        //Arrange

        //Act
        var result = Types.Where(x => x.IsInternal()).ToList();

        //Assert
        result.Should().Contain(new List<Type>
            {
                typeof(PublicGarbage.IInternalGarbage)
            });

        result.Should().NotContain(new List<Type>
            {
                typeof(Garbage.Garbage),
                typeof(GarbageEnum),
                typeof(GarbageStruct),
                typeof(IGarbage),
                typeof(AbstractGarbage),
                typeof(GarbageAbstractAttribute),
                typeof(GarbageAttribute),
                typeof(AttributedGarbage),
                typeof(StaticGarbage),
                typeof(PublicGarbage),
            });
    }

    [TestMethod]
    public void Where_WhenDirectlyImplementsIGarbage_ReturnTypesThatDirectlyImplementGarbage()
    {
        //Arrange

        //Act
        var result = Types.Where(x => x.DirectlyImplements<IGarbage>()).ToList();

        //Assert
        result.Should().Contain(new List<Type>
            {
                typeof(Garbage.Garbage),
                typeof(DirectlyImplementingGarbage),
            });

        result.Should().NotContain(new List<Type>
            {
                typeof(GarbageEnum),
                typeof(GarbageStruct),
                typeof(IGarbage),
                typeof(AbstractGarbage),
                typeof(GarbageAbstractAttribute),
                typeof(GarbageAttribute),
                typeof(AttributedGarbage),
                typeof(StaticGarbage),
                typeof(PublicGarbage),
                typeof(PublicGarbage.IInternalGarbage)
            });
    }

    [TestMethod]
    public void Where_WhenNotDirectlyImplementsIGarbage_ReturnTypesThatDirectlyImplementGarbage()
    {
        //Arrange

        //Act
        var result = Types.Where(x => !x.DirectlyImplements<IGarbage>()).ToList();

        //Assert
        result.Should().Contain(new List<Type>
            {
                typeof(GarbageEnum),
                typeof(GarbageStruct),
                typeof(IGarbage),
                typeof(AbstractGarbage),
                typeof(GarbageAbstractAttribute),
                typeof(GarbageAttribute),
                typeof(AttributedGarbage),
                typeof(StaticGarbage),
                typeof(PublicGarbage),
                typeof(PublicGarbage.IInternalGarbage)
            });

        result.Should().NotContain(new List<Type>
            {
                typeof(Garbage.Garbage),
                typeof(DirectlyImplementingGarbage),
            });
    }

    [TestMethod]
    public void Where_WhenDirectlyImplementsIDisposable_ReturnTypesThatDirectlyImplementIDisposable()
    {
        //Arrange

        //Act
        var result = Types.Where(x => x.DirectlyImplements<IDisposable>()).ToList();

        //Assert
        result.Should().Contain(new List<Type>
            {
                typeof(IndirectlyImplementingGarbage),
            });

        result.Should().NotContain(new List<Type>
            {
                typeof(GarbageEnum),
                typeof(GarbageStruct),
                typeof(IGarbage),
                typeof(AbstractGarbage),
                typeof(GarbageAbstractAttribute),
                typeof(GarbageAttribute),
                typeof(AttributedGarbage),
                typeof(StaticGarbage),
                typeof(PublicGarbage),
                typeof(PublicGarbage.IInternalGarbage),
                typeof(Garbage.Garbage),
                typeof(DirectlyImplementingGarbage),
            });
    }

    [TestMethod]
    public void Where_WhenSearchingForTypesThatDoNotImplementAnything_ReturnTypesThatDoNotImplement()
    {
        //Arrange

        //Act
        var result = Types.Where(x => !x.HasInterface()).ToList();

        //Assert
        result.Should().Contain(new List<Type>
            {
                typeof(AbstractGarbage),
                typeof(GarbageAbstractAttribute),
                typeof(GarbageAttribute),
                typeof(AttributedGarbage),
                typeof(PublicGarbage.IInternalGarbage),
                typeof(StaticGarbage),
                typeof(IGarbage),
                typeof(PublicGarbage),
            });

        result.Should().NotContain(new List<Type>
            {
                typeof(Garbage.Garbage),
                typeof(DirectlyImplementingGarbage),
                typeof(IndirectlyImplementingGarbage),
                typeof(GarbageEnum),
                typeof(GarbageStruct),
            });
    }

    [TestMethod]
    public void Where_WhenSearchingForTypesThatHaveAttributeWithExactValue_Return()
    {
        //Arrange

        //Act
        var result = Types.Where(x => x.HasAttribute<GarbageAttribute>(y => y.Description == "I am a dummy")).ToList();

        //Assert
        result.Should().Contain(new List<Type>
            {
                typeof(AttributedGarbage),
            });

        result.Should().NotContain(new List<Type>
            {
                typeof(Garbage.Garbage),
                typeof(DirectlyImplementingGarbage),
                typeof(IndirectlyImplementingGarbage),
                typeof(GarbageEnum),
                typeof(GarbageStruct),
                typeof(AbstractGarbage),
                typeof(GarbageAbstractAttribute),
                typeof(GarbageAttribute),
                typeof(PublicGarbage.IInternalGarbage),
                typeof(StaticGarbage),
                typeof(IGarbage),
                typeof(PublicGarbage),
            });
    }

    [TestMethod]
    public void Where_WhenSearchingForTypesThatHaveAttributeWithExactValueButNotTheCorrectOne_ReturnEmpty()
    {
        //Arrange

        //Act
        var result = Types.Where(x => x.HasAttribute<GarbageAttribute>(y => y.Description == "I am a big dummy")).ToList();

        //Assert
        result.Should().BeEmpty();
    }

    [TestMethod]
    public void Where_WhenSearchingForSealedTypes_Return()
    {
        //Arrange

        //Act
        var result = Types.Where(x => x.IsSealed).ToList();

        //Assert
        result.Should().Contain(new List<Type>
            {
                typeof(SealedGarbage),
                typeof(StaticGarbage),
                typeof(GarbageEnum),
                typeof(GarbageStruct),
            });

        result.Should().NotContain(new List<Type>
            {
                typeof(Garbage.Garbage),
                typeof(DirectlyImplementingGarbage),
                typeof(IndirectlyImplementingGarbage),
                typeof(AbstractGarbage),
                typeof(GarbageAbstractAttribute),
                typeof(GarbageAttribute),
                typeof(PublicGarbage.IInternalGarbage),
                typeof(IGarbage),
                typeof(PublicGarbage),
                typeof(AttributedGarbage),
            });
    }

    [TestMethod]
    public void Where_WhenSearchingForNullAttributeInHasAttribute_Throw()
    {
        //Arrange

        //Act
        var action = () => Types.Where(x => x.HasAttribute(null!)).ToList();

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName("attribute");
    }

    [TestMethod]
    public void Where_WhenSearchingForNullInterfaceInImplements_Throw()
    {
        //Arrange

        //Act
        var action = () => Types.Where(x => x.Implements(null!)).ToList();

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName("value");
    }

    [TestMethod]
    public void Where_WhenSearchingForNullInterfaceDirectlyInImplements_Throw()
    {
        //Arrange

        //Act
        var action = () => Types.Where(x => x.DirectlyImplements(null!)).ToList();

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName("interface");
    }

    [TestMethod]
    public void Where_WhenSearchingForTypesWithNoAttribute_Return()
    {
        //Arrange

        //Act
        var result = Types.Where(x => !x.HasAttribute()).ToList();

        //Assert
        result.Should().Contain(new List<Type>
            {
                typeof(IndirectlyImplementingGarbage),
                typeof(GarbageEnum),
                typeof(PublicGarbage.IInternalGarbage),
                typeof(StaticGarbage),
                typeof(PublicGarbage),
            });

        result.Should().NotContain(new List<Type>
            {
                typeof(Garbage.Garbage),
                typeof(AttributedGarbage),
                typeof(DirectlyImplementingGarbage),
                typeof(GarbageStruct),
                typeof(AbstractGarbage),
                typeof(GarbageAbstractAttribute),
                typeof(GarbageAttribute),
                typeof(IGarbage),
            });
    }

    [TestMethod]
    public void Where_WhenSearchingForTypesWithNamesThatStartWithGarbage_Return()
    {
        //Arrange

        //Act
        var result = Types.Where(x => x.Name.StartsWith("Garbage")).ToList();

        //Assert
        result.Should().Contain(new List<Type>
            {
                typeof(Garbage.Garbage),
                typeof(GarbageStruct),
                typeof(GarbageEnum),
                typeof(GarbageAttribute),
                typeof(GarbageAbstractAttribute),
            });

        result.Should().NotContain(new List<Type>
            {
                typeof(AttributedGarbage),
                typeof(DirectlyImplementingGarbage),
                typeof(AbstractGarbage),
                typeof(IGarbage),
                typeof(IndirectlyImplementingGarbage),
                typeof(PublicGarbage.IInternalGarbage),
                typeof(PublicGarbage),
                typeof(StaticGarbage),
            });
    }

    [TestMethod]
    public void Where_WhenSearchingForTypesWithNamesThatEndWithGarbage_Return()
    {
        //Arrange

        //Act
        var result = Types.Where(x => x.Name.EndsWith("Garbage")).ToList();

        //Assert
        result.Should().Contain(new List<Type>
            {
                typeof(Garbage.Garbage),
                typeof(AttributedGarbage),
                typeof(DirectlyImplementingGarbage),
                typeof(AbstractGarbage),
                typeof(IGarbage),
                typeof(IndirectlyImplementingGarbage),
                typeof(PublicGarbage.IInternalGarbage),
                typeof(PublicGarbage),
                typeof(StaticGarbage),
            });

        result.Should().NotContain(new List<Type>
            {
                typeof(GarbageStruct),
                typeof(GarbageEnum),
                typeof(GarbageAttribute),
                typeof(GarbageAbstractAttribute),
            });
    }

    [TestClass]
    public class First : Tester
    {
        [TestMethod]
        public void WhenThereShouldBeOnlyOneResultAndSingleIsUsed_ReturnSingleResult()
        {
            //Arrange

            //Act
            var result = Types.First(x => x.HasAttribute<GarbageAttribute>());

            //Assert
            result.Should().Be(typeof(AttributedGarbage));
        }

        [TestMethod]
        public void WhenThereShouldBeMultipleResultsAndSingleIsUsed_Throw()
        {
            //Arrange

            //Act
            var result = Types.First(x => x.IsClass);

            //Assert
            result.IsClass.Should().BeTrue();
        }
    }

    [TestMethod]
    public void FirstOrDefault_WhenThereShouldBeOnlyOneResultAndSingleOrDefaultIsUsed_ReturnSingleResult()
    {
        //Arrange

        //Act
        var result = Types.FirstOrDefault(x => x.HasAttribute(typeof(GarbageAttribute)));

        //Assert
        result.Should().Be(typeof(AttributedGarbage));
    }

    [TestMethod]
    public void FirstOrDefault_WhenThereShouldBeNoResultAndSingleOrDefaultIsUsed_ReturnDefault()
    {
        //Arrange

        //Act
        var result = Types.FirstOrDefault(x => x.HasAttribute<GarbageAttribute>() && x.IsInterface);

        //Assert
        result.Should().BeNull();
    }

    [TestMethod]
    public void FirstOrDefault_WhenThereShouldBeMultipleResultsAndSingleOrDefaultIsUsed_Throw()
    {
        //Arrange

        //Act
        var result = Types.FirstOrDefault(x => x.IsClass)!;

        //Assert
        result.IsClass.Should().BeTrue();
    }

    [TestMethod]
    public void Single_WhenThereShouldBeOnlyOneResultAndSingleIsUsed_ReturnSingleResult()
    {
        //Arrange

        //Act
        var result = Types.Single(x => x.HasAttribute<GarbageAttribute>());

        //Assert
        result.Should().Be(typeof(AttributedGarbage));
    }

    [TestMethod]
    public void Single_WhenThereShouldBeMultipleResultsAndSingleIsUsed_Throw()
    {
        //Arrange

        //Act
        var action = () => Types.Single(x => x.IsClass);

        //Assert
        action.Should().Throw<InvalidOperationException>();
    }

    [TestMethod]
    public void SingleOrDefault_WhenThereShouldBeOnlyOneResultAndSingleOrDefaultIsUsed_ReturnSingleResult()
    {
        //Arrange

        //Act
        var result = Types.SingleOrDefault(x => x.HasAttribute(typeof(GarbageAttribute)));

        //Assert
        result.Should().Be(typeof(AttributedGarbage));
    }

    [TestMethod]
    public void SingleOrDefault_WhenThereShouldBeNoResultAndSingleOrDefaultIsUsed_ReturnDefault()
    {
        //Arrange

        //Act
        var result = Types.SingleOrDefault(x => x.HasAttribute<GarbageAttribute>() && x.IsInterface);

        //Assert
        result.Should().BeNull();
    }

    [TestMethod]
    public void SingleOrDefault_WhenThereShouldBeMultipleResultsAndSingleOrDefaultIsUsed_Throw()
    {
        //Arrange

        //Act
        var action = () => Types.SingleOrDefault(x => x.IsClass);

        //Assert
        action.Should().Throw<InvalidOperationException>();
    }

    [TestMethod]
    public void ToList_WhenUsingToListWithoutAnyPredicate_ReturnAllTypesInAssembly()
    {
        //Arrange

        //Act
        var result = Types.ToList();

        //Assert
        result.Should().BeEquivalentTo(AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).DistinctBy(x => x.FullName));
    }

    [TestMethod]
    public void ToArray_WhenUsingToArrayWithoutAnyPredicate_ReturnAllTypesInAssembly()
    {
        //Arrange

        //Act
        var result = Types.ToArray();

        //Assert
        result.Should().BeEquivalentTo(AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).DistinctBy(x => x.FullName));
    }
}