namespace Reflection4Humans.TypeFetcher.Tests;

[TestClass]
public class TypesTest
{
    [TestClass]
    public class Where : Tester
    {
        [TestMethod]
        public void WhenPredicateIsNull_Throw()
        {
            //Arrange
            Func<Type, bool> predicate = null!;

            //Act
            var action = () => Types.Where(predicate);

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(predicate));
        }

        [TestMethod]
        public void WhenIsClass_ReturnOnlyClassesAbstractOrNot()
        {
            //Arrange

            //Act
            var result = Types.Where(x => x.IsClass).ToList();

            //Assert
            result.Should().Contain(new List<Type>
            {
                typeof(AbstractDummy),
                typeof(Dummy),
                typeof(DummyAttribute),
                typeof(DummyAbstractAttribute),
                typeof(StaticDummy),
            });

            result.Should().NotContain(new List<Type>
            {
                typeof(IDummy),
                typeof(DummyStruct),
                typeof(DummyEnum),
            });
        }

        [TestMethod]
        public void WhenIsNotClass_ReturnEverythingThatIsNotAClass()
        {
            //Arrange

            //Act
            var result = Types.Where(x => !x.IsClass).ToList();

            //Assert
            result.Should().Contain(new List<Type>
            {
                typeof(IDummy),
                typeof(DummyStruct),
                typeof(DummyEnum),
            });

            result.Should().NotContain(new List<Type>
            {
                typeof(AbstractDummy),
                typeof(Dummy),
                typeof(DummyAttribute),
                typeof(DummyAbstractAttribute),
                typeof(StaticDummy),
            });
        }

        [TestMethod]
        public void WhenIsClassAndAbstract_ReturnOnlyAbstractClasses()
        {
            //Arrange

            //Act
            var result = Types.Where(x => x.IsClass && x.IsAbstract).ToList();

            //Assert
            result.Should().Contain(new List<Type>
            {
                typeof(AbstractDummy),
                typeof(DummyAbstractAttribute),
                typeof(StaticDummy),
            });

            result.Should().NotContain(new List<Type>
            {
                typeof(IDummy),
                typeof(Dummy),
                typeof(DummyAttribute),
                typeof(DummyStruct),
                typeof(DummyEnum),
            });
        }

        [TestMethod]
        public void WhenIsClassAndNotAbstract_ReturnOnlyNonAbstractClasses()
        {
            //Arrange

            //Act
            var result = Types.Where(x => x.IsClass && !x.IsAbstract).ToList();

            //Assert
            result.Should().Contain(new List<Type>
            {
                typeof(Dummy),
                typeof(DummyAttribute),
            });

            result.Should().NotContain(new List<Type>
            {
                typeof(AbstractDummy),
                typeof(IDummy),
                typeof(DummyStruct),
                typeof(DummyEnum),
                typeof(DummyAbstractAttribute),
                typeof(StaticDummy),
            });
        }

        [TestMethod]
        public void WhenIsNotClassAndIsAbstract_ReturnOnlyInterfaces()
        {
            //Arrange

            //Act
            var result = Types.Where(x => !x.IsClass && x.IsAbstract).ToList();

            //Assert
            result.Should().Contain(new List<Type>
            {
                typeof(IDummy),
            });

            result.Should().NotContain(new List<Type>
            {
                typeof(AbstractDummy),
                typeof(DummyStruct),
                typeof(DummyEnum),
                typeof(Dummy),
                typeof(DummyAttribute),
                typeof(DummyAbstractAttribute),
                typeof(StaticDummy),
            });
        }

        [TestMethod]
        public void WhenIsAbstract_ReturnOnlyInterfacesAndAbstractClasses()
        {
            //Arrange

            //Act
            var result = Types.Where(x => x.IsAbstract).ToList();

            //Assert
            result.Should().Contain(new List<Type>
            {
                typeof(IDummy),
                typeof(AbstractDummy),
                typeof(DummyAbstractAttribute),
                typeof(StaticDummy),
            });

            result.Should().NotContain(new List<Type>
            {
                typeof(DummyStruct),
                typeof(DummyEnum),
                typeof(Dummy),
                typeof(DummyAttribute),
            });
        }

        [TestMethod]
        public void WhenIsInterface_ReturnOnlyInterfaces()
        {
            //Arrange

            //Act
            var result = Types.Where(x => x.IsInterface).ToList();

            //Assert
            result.Should().Contain(new List<Type>
            {
                typeof(IDummy),
            });

            result.Should().NotContain(new List<Type>
            {
                typeof(DummyStruct),
                typeof(DummyEnum),
                typeof(Dummy),
                typeof(DummyAttribute),
                typeof(AbstractDummy),
                typeof(DummyAbstractAttribute),
                typeof(StaticDummy),
            });
        }

        [TestMethod]
        public void WhenIsClassAndInterfaces_ReturnEmpty()
        {
            //Arrange

            //Act
            var result = Types.Where(x => x.IsClass && x.IsInterface).ToList();

            //Assert
            //It is impossible to both class AND interface
            result.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenIsAttribute_ReturnOnlyAttributes()
        {
            //Arrange

            //Act
            var result = Types.Where(x => x.IsAttribute()).ToList();

            //Assert
            result.Should().Contain(new List<Type>
            {
                typeof(DummyAttribute),
                typeof(DummyAbstractAttribute),
            });

            result.Should().NotContain(new List<Type>
            {
                typeof(DummyStruct),
                typeof(DummyEnum),
                typeof(Dummy),
                typeof(AbstractDummy),
                typeof(IDummy),
                typeof(StaticDummy),
            });
        }

        [TestMethod]
        public void WhenIsAttributeAndInterface_ReturnEmpty()
        {
            //Arrange

            //Act
            var result = Types.Where(x => x.IsInterface && x.IsAttribute()).ToList();

            //Assert
            //It is impossible to both class AND interface
            result.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenIsAttributeAndAbstract_ReturnOnlyAbstractAttributes()
        {
            //Arrange

            //Act
            var result = Types.Where(x => x.IsAttribute() && x.IsAbstract).ToList();

            //Assert
            result.Should().Contain(new List<Type>
            {
                typeof(DummyAbstractAttribute),
            });

            result.Should().NotContain(new List<Type>
            {
                typeof(DummyStruct),
                typeof(DummyEnum),
                typeof(Dummy),
                typeof(AbstractDummy),
                typeof(IDummy),
                typeof(DummyAttribute),
                typeof(StaticDummy),
            });
        }

        [TestMethod]
        public void WhenIsAttributeAndClassAndAbstract_ReturnOnlyAbstractAttributes()
        {
            //Arrange

            //Act
            var result = Types.Where(x => x.IsAttribute() && x.IsAbstract && x.IsClass).ToList();

            //Assert
            result.Should().Contain(new List<Type>
            {
                typeof(DummyAbstractAttribute),
            });

            result.Should().NotContain(new List<Type>
            {
                typeof(DummyStruct),
                typeof(DummyEnum),
                typeof(Dummy),
                typeof(AbstractDummy),
                typeof(IDummy),
                typeof(DummyAttribute),
                typeof(StaticDummy),
            });
        }

        [TestMethod]
        public void WhenIsAttributeAndNotAbstract_ReturnOnlyNonAbstractAttributes()
        {
            //Arrange

            //Act
            var result = Types.Where(x => x.IsAttribute() && !x.IsAbstract).ToList();

            //Assert
            result.Should().Contain(new List<Type>
            {
                typeof(DummyAttribute),
            });

            result.Should().NotContain(new List<Type>
            {
                typeof(DummyStruct),
                typeof(DummyEnum),
                typeof(Dummy),
                typeof(AbstractDummy),
                typeof(IDummy),
                typeof(DummyAbstractAttribute),
                typeof(StaticDummy),
            });
        }

        [TestMethod]
        public void WhenIsNotAttribute_ReturnEverythingThatIsNotAnAttribute()
        {
            //Arrange

            //Act
            var result = Types.Where(x => !x.IsAttribute()).ToList();

            //Assert
            result.Should().Contain(new List<Type>
            {
                typeof(DummyStruct),
                typeof(DummyEnum),
                typeof(Dummy),
                typeof(AbstractDummy),
                typeof(IDummy),
                typeof(StaticDummy),
            });

            result.Should().NotContain(new List<Type>
            {
                typeof(DummyAbstractAttribute),
                typeof(DummyAttribute),
            });
        }

        [TestMethod]
        public void WhenIsNotInterface_ReturnEverythingThatIsNotAnInterface()
        {
            //Arrange

            //Act
            var result = Types.Where(x => !x.IsInterface).ToList();

            //Assert
            result.Should().Contain(new List<Type>
            {
                typeof(DummyStruct),
                typeof(DummyEnum),
                typeof(Dummy),
                typeof(AbstractDummy),
                typeof(DummyAbstractAttribute),
                typeof(DummyAttribute),
                typeof(StaticDummy),
            });

            result.Should().NotContain(new List<Type>
            {
                typeof(IDummy),
            });
        }

        [TestMethod]
        public void WhenIsNotStruct_ReturnEverythingThatIsNotAStruct()
        {
            //Arrange

            //Act
            var result = Types.Where(x => !x.IsValueType).ToList();

            //Assert
            result.Should().Contain(new List<Type>
            {
                typeof(IDummy),
                typeof(Dummy),
                typeof(AbstractDummy),
                typeof(DummyAbstractAttribute),
                typeof(DummyAttribute),
                typeof(StaticDummy),
            });

            result.Should().NotContain(new List<Type>
            {
                typeof(DummyStruct),
                typeof(DummyEnum),
            });
        }

        [TestMethod]
        public void WhenIsStruct_ReturnStructsAndEnums()
        {
            //Arrange

            //Act
            var result = Types.Where(x => x.IsValueType).ToList();

            //Assert
            result.Should().Contain(new List<Type>
            {
                typeof(DummyStruct),
                typeof(DummyEnum),

            });

            result.Should().NotContain(new List<Type>
            {
                typeof(IDummy),
                typeof(Dummy),
                typeof(AbstractDummy),
                typeof(DummyAbstractAttribute),
                typeof(DummyAttribute),
                typeof(StaticDummy),
            });
        }

        [TestMethod]
        public void WhenIsEnum_ReturnEnums()
        {
            //Arrange

            //Act
            var result = Types.Where(x => x.IsEnum).ToList();

            //Assert
            result.Should().Contain(new List<Type>
            {
                typeof(DummyEnum),

            });

            result.Should().NotContain(new List<Type>
            {
                typeof(DummyStruct),
                typeof(IDummy),
                typeof(Dummy),
                typeof(AbstractDummy),
                typeof(DummyAbstractAttribute),
                typeof(DummyAttribute),
                typeof(StaticDummy),
            });
        }

        [TestMethod]
        public void WhenIsNotEnum_ReturnEverythingButEnums()
        {
            //Arrange

            //Act
            var result = Types.Where(x => !x.IsEnum).ToList();

            //Assert
            result.Should().Contain(new List<Type>
            {
                typeof(DummyStruct),
                typeof(IDummy),
                typeof(Dummy),
                typeof(AbstractDummy),
                typeof(DummyAbstractAttribute),
                typeof(DummyAttribute),
                typeof(StaticDummy),
            });

            result.Should().NotContain(new List<Type>
            {
                typeof(DummyEnum),
            });
        }

        [TestMethod]
        public void WhenIsGeneric_ReturnAllGenerics()
        {
            //Arrange

            //Act
            var result = Types.Where(x => x.IsGenericType).ToList();

            //Assert
            result.Should().Contain(new List<Type>
            {
                typeof(GenericDummy<>),
                typeof(IGenericDummy<>),
                typeof(GenericDummyStruct<>),

            });

            result.Should().NotContain(new List<Type>
            {
                typeof(DummyStruct),
                typeof(IDummy),
                typeof(Dummy),
                typeof(AbstractDummy),
                typeof(DummyAbstractAttribute),
                typeof(DummyAttribute),
                typeof(DummyEnum),
                typeof(StaticDummy),
            });
        }

        [TestMethod]
        public void WhenIsNotGeneric_ReturnAllExceptGenerics()
        {
            //Arrange

            //Act
            var result = Types.Where(x => !x.IsGenericType).ToList();

            //Assert
            result.Should().Contain(new List<Type>
            {
                typeof(DummyStruct),
                typeof(IDummy),
                typeof(Dummy),
                typeof(AbstractDummy),
                typeof(DummyAbstractAttribute),
                typeof(DummyAttribute),
                typeof(DummyEnum),
                typeof(StaticDummy),
            });

            result.Should().NotContain(new List<Type>
            {
                typeof(GenericDummy<>),
                typeof(IGenericDummy<>),
                typeof(GenericDummyStruct<>),
            });
        }

        [TestMethod]
        public void WhenIsGenericTypeDefinition_ReturnAllGenericTypeDefinitions()
        {
            //Arrange

            //Act
            var result = Types.Where(x => x.IsGenericTypeDefinition).ToList();

            //Assert
            result.Should().Contain(new List<Type>
            {
                typeof(GenericDummy<>),
                typeof(IGenericDummy<>),
                typeof(GenericDummyStruct<>),
            });

            result.Should().NotContain(new List<Type>
            {
                typeof(DummyStruct),
                typeof(IDummy),
                typeof(Dummy),
                typeof(AbstractDummy),
                typeof(DummyAbstractAttribute),
                typeof(DummyAttribute),
                typeof(DummyEnum),
                typeof(StaticDummy),
            });
        }

        [TestMethod]
        public void WhenIsNotGenericTypeDefinition_ReturnAllExceptGenericTypeDefinitions()
        {
            //Arrange

            //Act
            var result = Types.Where(x => !x.IsGenericTypeDefinition).ToList();

            //Assert
            result.Should().Contain(new List<Type>
            {
                typeof(DummyStruct),
                typeof(IDummy),
                typeof(Dummy),
                typeof(AbstractDummy),
                typeof(DummyAbstractAttribute),
                typeof(DummyAttribute),
                typeof(DummyEnum),
                typeof(StaticDummy),
            });

            result.Should().NotContain(new List<Type>
            {
                typeof(GenericDummy<>),
                typeof(IGenericDummy<>),
                typeof(GenericDummyStruct<>),
            });
        }

        [TestMethod]
        public void WhenImplementsInterface_ReturnEverythingThatImplementsInterface()
        {
            //Arrange

            //Act
            var result = Types.Where(x => x.Implements<IDummy>()).ToList();

            //Assert
            result.Should().Contain(new List<Type>
            {
                typeof(Dummy),
            });

            result.Should().NotContain(new List<Type>
            {
                typeof(DummyEnum),
                typeof(DummyStruct),
                typeof(IDummy),
                typeof(AbstractDummy),
                typeof(DummyAbstractAttribute),
                typeof(DummyAttribute),
                typeof(StaticDummy),
            });
        }

        [TestMethod]
        public void WhenHasAttribute_ReturnEverthingThatHasAttribute()
        {
            //Arrange

            //Act
            var result = Types.Where(x => x.HasAttribute<DummyAttribute>()).ToList();

            //Assert
            result.Should().Contain(new List<Type>
            {
                typeof(AttributedDummy),
            });

            result.Should().NotContain(new List<Type>
            {
                typeof(Dummy),
                typeof(DummyEnum),
                typeof(DummyStruct),
                typeof(IDummy),
                typeof(AbstractDummy),
                typeof(DummyAbstractAttribute),
                typeof(DummyAttribute),
                typeof(StaticDummy),
            });
        }

        [TestMethod]
        public void WhenIsStatic_ReturnAllStaticClasses()
        {
            //Arrange

            //Act
            var result = Types.Where(x => x.IsStatic()).ToList();

            //Assert
            result.Should().Contain(new List<Type>
            {
                typeof(StaticDummy),
            });

            result.Should().NotContain(new List<Type>
            {
                typeof(Dummy),
                typeof(DummyEnum),
                typeof(DummyStruct),
                typeof(IDummy),
                typeof(AbstractDummy),
                typeof(DummyAbstractAttribute),
                typeof(DummyAttribute),
                typeof(AttributedDummy),
            });
        }

        [TestMethod]
        public void WhenIsNotStatic_ReturnAllNonStaticClasses()
        {
            //Arrange

            //Act
            var result = Types.Where(x => !x.IsStatic()).ToList();

            //Assert
            result.Should().Contain(new List<Type>
            {
                typeof(Dummy),
                typeof(DummyEnum),
                typeof(DummyStruct),
                typeof(IDummy),
                typeof(AbstractDummy),
                typeof(DummyAbstractAttribute),
                typeof(DummyAttribute),
                typeof(AttributedDummy),
            });

            result.Should().NotContain(new List<Type>
            {
                typeof(StaticDummy),
            });
        }

        [TestMethod]
        public void WhenIsPublic_ReturnPublicTypes()
        {
            //Arrange

            //Act
            var result = Types.Where(x => x.IsPublic).ToList();

            //Assert
            result.Should().Contain(new List<Type>
            {
                typeof(Dummy),
                typeof(DummyEnum),
                typeof(DummyStruct),
                typeof(IDummy),
                typeof(AbstractDummy),
                typeof(DummyAbstractAttribute),
                typeof(DummyAttribute),
                typeof(AttributedDummy),
                typeof(StaticDummy),
                typeof(PublicDummy)
            });

            result.Should().NotContain(new List<Type>
            {
                typeof(PublicDummy.IInternalDummy),
            });
        }

        [TestMethod]
        public void WhenIsPrivate_ReturnPrivateTypes()
        {
            //Arrange

            //Act
            var result = Types.Where(x => x.IsPrivate()).ToList();

            //Assert
            result.Select(x => x.Name).Should().Contain(new List<string>
            {
                "PrivateDummy"
            });

            result.Should().NotContain(new List<Type>
            {
                typeof(Dummy),
                typeof(DummyEnum),
                typeof(DummyStruct),
                typeof(IDummy),
                typeof(AbstractDummy),
                typeof(DummyAbstractAttribute),
                typeof(DummyAttribute),
                typeof(AttributedDummy),
                typeof(StaticDummy),
                typeof(PublicDummy),
                typeof(PublicDummy.IInternalDummy),
            });
        }

        [TestMethod]
        public void WhenIsProtected_ReturnProtectedTypes()
        {
            //Arrange

            //Act
            var result = Types.Where(x => x.IsProtected()).ToList();

            //Assert
            result.Select(x => x.Name).Should().Contain(new List<string>
            {
                "ProtectedDummy"
            });

            result.Should().NotContain(new List<Type>
            {
                typeof(Dummy),
                typeof(DummyEnum),
                typeof(DummyStruct),
                typeof(IDummy),
                typeof(AbstractDummy),
                typeof(DummyAbstractAttribute),
                typeof(DummyAttribute),
                typeof(AttributedDummy),
                typeof(StaticDummy),
                typeof(PublicDummy),
                typeof(PublicDummy.IInternalDummy),
            });
        }

        [TestMethod]
        public void WhenIsInternal_ReturnInternalTypes()
        {
            //Arrange

            //Act
            var result = Types.Where(x => x.IsInternal()).ToList();

            //Assert
            result.Should().Contain(new List<Type>
            {
                typeof(PublicDummy.IInternalDummy)
            });

            result.Should().NotContain(new List<Type>
            {
                typeof(Dummy),
                typeof(DummyEnum),
                typeof(DummyStruct),
                typeof(IDummy),
                typeof(AbstractDummy),
                typeof(DummyAbstractAttribute),
                typeof(DummyAttribute),
                typeof(AttributedDummy),
                typeof(StaticDummy),
                typeof(PublicDummy),
            });
        }

        [TestMethod]
        public void WhenDirectlyImplementsIDummy_ReturnTypesThatDirectlyImplementDummy()
        {
            //Arrange

            //Act
            var result = Types.Where(x => x.DirectlyImplements<IDummy>()).ToList();

            //Assert
            result.Should().Contain(new List<Type>
            {
                typeof(Dummy),
                typeof(DirectlyImplementingDummy),
            });

            result.Should().NotContain(new List<Type>
            {
                typeof(DummyEnum),
                typeof(DummyStruct),
                typeof(IDummy),
                typeof(AbstractDummy),
                typeof(DummyAbstractAttribute),
                typeof(DummyAttribute),
                typeof(AttributedDummy),
                typeof(StaticDummy),
                typeof(PublicDummy),
                typeof(PublicDummy.IInternalDummy)
            });
        }

        [TestMethod]
        public void WhenNotDirectlyImplementsIDummy_ReturnTypesThatDirectlyImplementDummy()
        {
            //Arrange

            //Act
            var result = Types.Where(x => !x.DirectlyImplements<IDummy>()).ToList();

            //Assert
            result.Should().Contain(new List<Type>
            {
                typeof(DummyEnum),
                typeof(DummyStruct),
                typeof(IDummy),
                typeof(AbstractDummy),
                typeof(DummyAbstractAttribute),
                typeof(DummyAttribute),
                typeof(AttributedDummy),
                typeof(StaticDummy),
                typeof(PublicDummy),
                typeof(PublicDummy.IInternalDummy)
            });

            result.Should().NotContain(new List<Type>
            {
                typeof(Dummy),
                typeof(DirectlyImplementingDummy),
            });
        }

        [TestMethod]
        public void WhenDirectlyImplementsIDisposable_ReturnTypesThatDirectlyImplementIDisposable()
        {
            //Arrange

            //Act
            var result = Types.Where(x => x.DirectlyImplements<IDisposable>()).ToList();

            //Assert
            result.Should().Contain(new List<Type>
            {
                typeof(IndirectlyImplementingDummy),
            });

            result.Should().NotContain(new List<Type>
            {
                typeof(DummyEnum),
                typeof(DummyStruct),
                typeof(IDummy),
                typeof(AbstractDummy),
                typeof(DummyAbstractAttribute),
                typeof(DummyAttribute),
                typeof(AttributedDummy),
                typeof(StaticDummy),
                typeof(PublicDummy),
                typeof(PublicDummy.IInternalDummy),
                typeof(Dummy),
                typeof(DirectlyImplementingDummy),
            });
        }

        [TestMethod]
        public void WhenSearchingForTypesThatDoNotImplementAnything_ReturnTypesThatDoNotImplement()
        {
            //Arrange

            //Act
            var result = Types.Where(x => !x.HasInterface()).ToList();

            //Assert
            result.Should().Contain(new List<Type>
            {
                typeof(AbstractDummy),
                typeof(DummyAbstractAttribute),
                typeof(DummyAttribute),
                typeof(AttributedDummy),
                typeof(PublicDummy.IInternalDummy),
                typeof(StaticDummy),
                typeof(IDummy),
                typeof(PublicDummy),
            });

            result.Should().NotContain(new List<Type>
            {
                typeof(Dummy),
                typeof(DirectlyImplementingDummy),
                typeof(IndirectlyImplementingDummy),
                typeof(DummyEnum),
                typeof(DummyStruct),
            });
        }

        [TestMethod]
        public void WhenSearchingForTypesThatHaveAttributeWithExactValue_Return()
        {
            //Arrange

            //Act
            var result = Types.Where(x => x.HasAttribute<DummyAttribute>(y => y.Description == "I am a dummy")).ToList();

            //Assert
            result.Should().Contain(new List<Type>
            {
                typeof(AttributedDummy),
            });

            result.Should().NotContain(new List<Type>
            {
                typeof(Dummy),
                typeof(DirectlyImplementingDummy),
                typeof(IndirectlyImplementingDummy),
                typeof(DummyEnum),
                typeof(DummyStruct),
                typeof(AbstractDummy),
                typeof(DummyAbstractAttribute),
                typeof(DummyAttribute),
                typeof(PublicDummy.IInternalDummy),
                typeof(StaticDummy),
                typeof(IDummy),
                typeof(PublicDummy),
            });
        }

        [TestMethod]
        public void WhenSearchingForTypesThatHaveAttributeWithExactValueButNotTheCorrectOne_ReturnEmpty()
        {
            //Arrange

            //Act
            var result = Types.Where(x => x.HasAttribute<DummyAttribute>(y => y.Description == "I am a big dummy")).ToList();

            //Assert
            result.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenSearchingForSealedTypes_Return()
        {
            //Arrange

            //Act
            var result = Types.Where(x => x.IsSealed).ToList();

            //Assert
            result.Should().Contain(new List<Type>
            {
                typeof(SealedDummy),
                typeof(StaticDummy),
                typeof(DummyEnum),
                typeof(DummyStruct),
            });

            result.Should().NotContain(new List<Type>
            {
                typeof(Dummy),
                typeof(DirectlyImplementingDummy),
                typeof(IndirectlyImplementingDummy),
                typeof(AbstractDummy),
                typeof(DummyAbstractAttribute),
                typeof(DummyAttribute),
                typeof(PublicDummy.IInternalDummy),
                typeof(IDummy),
                typeof(PublicDummy),
                typeof(AttributedDummy),
            });
        }

        [TestMethod]
        public void WhenSearchingForNullAttributeInHasAttribute_Throw()
        {
            //Arrange

            //Act
            var action = () => Types.Where(x => x.HasAttribute(null!)).ToList();

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName("attribute");
        }

        [TestMethod]
        public void WhenSearchingForNullInterfaceInImplements_Throw()
        {
            //Arrange

            //Act
            var action = () => Types.Where(x => x.Implements(null!)).ToList();

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName("value");
        }

        [TestMethod]
        public void WhenSearchingForNullInterfaceDirectlyInImplements_Throw()
        {
            //Arrange

            //Act
            var action = () => Types.Where(x => x.DirectlyImplements(null!)).ToList();

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName("interface");
        }

        [TestMethod]
        public void WhenSearchingForTypesWithNoAttribute_Return()
        {
            //Arrange

            //Act
            var result = Types.Where(x => !x.HasAttribute()).ToList();

            //Assert
            result.Should().Contain(new List<Type>
            {
                typeof(IndirectlyImplementingDummy),
                typeof(DummyEnum),
                typeof(PublicDummy.IInternalDummy),
                typeof(StaticDummy),
                typeof(PublicDummy),
            });

            result.Should().NotContain(new List<Type>
            {
                typeof(Dummy),
                typeof(AttributedDummy),
                typeof(DirectlyImplementingDummy),
                typeof(DummyStruct),
                typeof(AbstractDummy),
                typeof(DummyAbstractAttribute),
                typeof(DummyAttribute),
                typeof(IDummy),
            });
        }

        [TestMethod]
        public void WhenSearchingForTypesWithNamesThatStartWithDummy_Return()
        {
            //Arrange

            //Act
            var result = Types.Where(x => x.Name.StartsWith("Dummy")).ToList();

            //Assert
            result.Should().Contain(new List<Type>
            {
                typeof(Dummy),
                typeof(DummyStruct),
                typeof(DummyEnum),
                typeof(DummyAttribute),
                typeof(DummyAbstractAttribute),
            });

            result.Should().NotContain(new List<Type>
            {
                typeof(AttributedDummy),
                typeof(DirectlyImplementingDummy),
                typeof(AbstractDummy),
                typeof(IDummy),
                typeof(IndirectlyImplementingDummy),
                typeof(PublicDummy.IInternalDummy),
                typeof(PublicDummy),
                typeof(StaticDummy),
            });
        }

        [TestMethod]
        public void WhenSearchingForTypesWithNamesThatEndWithDummy_Return()
        {
            //Arrange

            //Act
            var result = Types.Where(x => x.Name.EndsWith("Dummy")).ToList();

            //Assert
            result.Should().Contain(new List<Type>
            {
                typeof(Dummy),
                typeof(AttributedDummy),
                typeof(DirectlyImplementingDummy),
                typeof(AbstractDummy),
                typeof(IDummy),
                typeof(IndirectlyImplementingDummy),
                typeof(PublicDummy.IInternalDummy),
                typeof(PublicDummy),
                typeof(StaticDummy),
            });

            result.Should().NotContain(new List<Type>
            {
                typeof(DummyStruct),
                typeof(DummyEnum),
                typeof(DummyAttribute),
                typeof(DummyAbstractAttribute),
            });
        }
    }

    [TestClass]
    public class First : Tester
    {
        [TestMethod]
        public void WhenThereShouldBeOnlyOneResultAndSingleIsUsed_ReturnSingleResult()
        {
            //Arrange

            //Act
            var result = Types.First(x => x.HasAttribute<DummyAttribute>());

            //Assert
            result.Should().Be(typeof(AttributedDummy));
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

    [TestClass]
    public class FirstOrDefault : Tester
    {
        [TestMethod]
        public void WhenThereShouldBeOnlyOneResultAndSingleOrDefaultIsUsed_ReturnSingleResult()
        {
            //Arrange

            //Act
            var result = Types.FirstOrDefault(x => x.HasAttribute(typeof(DummyAttribute)));

            //Assert
            result.Should().Be(typeof(AttributedDummy));
        }

        [TestMethod]
        public void WhenThereShouldBeNoResultAndSingleOrDefaultIsUsed_ReturnDefault()
        {
            //Arrange

            //Act
            var result = Types.FirstOrDefault(x => x.HasAttribute<DummyAttribute>() && x.IsInterface);

            //Assert
            result.Should().BeNull();
        }

        [TestMethod]
        public void WhenThereShouldBeMultipleResultsAndSingleOrDefaultIsUsed_Throw()
        {
            //Arrange

            //Act
            var result = Types.FirstOrDefault(x => x.IsClass)!;

            //Assert
            result.IsClass.Should().BeTrue();
        }
    }

    [TestClass]
    public class Single : Tester
    {
        [TestMethod]
        public void WhenThereShouldBeOnlyOneResultAndSingleIsUsed_ReturnSingleResult()
        {
            //Arrange

            //Act
            var result = Types.Single(x => x.HasAttribute<DummyAttribute>());

            //Assert
            result.Should().Be(typeof(AttributedDummy));
        }

        [TestMethod]
        public void WhenThereShouldBeMultipleResultsAndSingleIsUsed_Throw()
        {
            //Arrange

            //Act
            var action = () => Types.Single(x => x.IsClass);

            //Assert
            action.Should().Throw<InvalidOperationException>();
        }
    }

    [TestClass]
    public class SingleOrDefault : Tester
    {
        [TestMethod]
        public void WhenThereShouldBeOnlyOneResultAndSingleOrDefaultIsUsed_ReturnSingleResult()
        {
            //Arrange

            //Act
            var result = Types.SingleOrDefault(x => x.HasAttribute(typeof(DummyAttribute)));

            //Assert
            result.Should().Be(typeof(AttributedDummy));
        }

        [TestMethod]
        public void WhenThereShouldBeNoResultAndSingleOrDefaultIsUsed_ReturnDefault()
        {
            //Arrange

            //Act
            var result = Types.SingleOrDefault(x => x.HasAttribute<DummyAttribute>() && x.IsInterface);

            //Assert
            result.Should().BeNull();
        }

        [TestMethod]
        public void WhenThereShouldBeMultipleResultsAndSingleOrDefaultIsUsed_Throw()
        {
            //Arrange

            //Act
            var action = () => Types.SingleOrDefault(x => x.IsClass);

            //Assert
            action.Should().Throw<InvalidOperationException>();
        }
    }

    [TestClass]
    public class ToList : Tester
    {
        [TestMethod]
        public void WhenUsingToListWithoutAnyPredicate_ReturnAllTypesInAssembly()
        {
            //Arrange

            //Act
            var result = Types.ToList();

            //Assert
            result.Should().BeEquivalentTo(AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).DistinctBy(x => x.FullName));
        }
    }

    [TestClass]
    public class ToArray : Tester
    {
        [TestMethod]
        public void WhenUsingToArrayWithoutAnyPredicate_ReturnAllTypesInAssembly()
        {
            //Arrange

            //Act
            var result = Types.ToArray();

            //Assert
            result.Should().BeEquivalentTo(AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).DistinctBy(x => x.FullName));
        }
    }
}