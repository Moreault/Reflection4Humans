namespace Reflection4Humans.TypeFetcher.Tests;

[TestClass]
public class TypeFetcherTester
{
    [TestClass]
    public class Query : Tester
    {
        [TestMethod]
        public void WhenUsingToListWithoutAnyPredicate_ReturnAllTypesInAssembly()
        {
            //Arrange

            //Act
            var result = ToolBX.Reflection4Humans.TypeFetcher.TypeFetcher.Query().ToList();

            //Assert
            result.Should().BeEquivalentTo(AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).DistinctBy(x => x.FullName));
        }

        [TestMethod]
        public void WhenUsingSingleWithoutAnyPredicate_Throw()
        {
            //Arrange

            //Act
            var action = () => ToolBX.Reflection4Humans.TypeFetcher.TypeFetcher.Query().Single();

            //Assert
            action.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public void WhenUsingSingleOrDefaultWithoutAnyPredicate_Throw()
        {
            //Arrange

            //Act
            var action = () => ToolBX.Reflection4Humans.TypeFetcher.TypeFetcher.Query().SingleOrDefault();

            //Assert
            action.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public void WhenIsClass_ReturnOnlyClassesAbstractOrNot()
        {
            //Arrange

            //Act
            var result = ToolBX.Reflection4Humans.TypeFetcher.TypeFetcher.Query().IsClass().ToList();

            //Assert
            result.Should().Contain(new List<Type>
            {
                typeof(AbstractDummy),
                typeof(Dummy),
                typeof(DummyAttribute),
                typeof(DummyAbstractAttribute),
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
            var result = ToolBX.Reflection4Humans.TypeFetcher.TypeFetcher.Query().IsNotClass().ToList();

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
            });
        }

        [TestMethod]
        public void WhenIsClassAndAbstract_ReturnOnlyAbstractClasses()
        {
            //Arrange

            //Act
            var result = ToolBX.Reflection4Humans.TypeFetcher.TypeFetcher.Query().IsClass().IsAbstract().ToList();

            //Assert
            result.Should().Contain(new List<Type>
            {
                typeof(AbstractDummy),
                typeof(DummyAbstractAttribute),
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
            var result = ToolBX.Reflection4Humans.TypeFetcher.TypeFetcher.Query().IsClass().IsNotAbstract().ToList();

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
            });
        }

        [TestMethod]
        public void WhenIsNotClassAndIsAbstract_ReturnOnlyInterfaces()
        {
            //Arrange

            //Act
            var result = ToolBX.Reflection4Humans.TypeFetcher.TypeFetcher.Query().IsNotClass().IsAbstract().ToList();

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
            });
        }

        //TODO Test "Nots" (ex : IsNotClass, IsNotAbstract, IsNotInterface, etc...)

        [TestMethod]
        public void WhenIsAbstract_ReturnOnlyInterfacesAndAbstractClasses()
        {
            //Arrange

            //Act
            var result = ToolBX.Reflection4Humans.TypeFetcher.TypeFetcher.Query().IsAbstract().ToList();

            //Assert
            result.Should().Contain(new List<Type>
            {
                typeof(IDummy),
                typeof(AbstractDummy),
                typeof(DummyAbstractAttribute),
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
            var result = ToolBX.Reflection4Humans.TypeFetcher.TypeFetcher.Query().IsInterface().ToList();

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
            });
        }

        [TestMethod]
        public void WhenIsClassAndInterfaces_ReturnEmpty()
        {
            //Arrange

            //Act
            //TODO Should this be inferred to be an "or" ?
            var result = ToolBX.Reflection4Humans.TypeFetcher.TypeFetcher.Query().IsClass().IsInterface().ToList();

            //Assert
            //It is impossible to both class AND interface
            result.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenIsAttribute_ReturnOnlyAttributes()
        {
            //Arrange

            //Act
            var result = ToolBX.Reflection4Humans.TypeFetcher.TypeFetcher.Query().IsAttribute().ToList();

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
            });
        }

        [TestMethod]
        public void WhenIsAttributeAndInterface_ReturnEmpty()
        {
            //Arrange

            //Act
            //TODO Should this be inferred to be an "or" ?
            var result = ToolBX.Reflection4Humans.TypeFetcher.TypeFetcher.Query().IsAttribute().IsInterface().ToList();

            //Assert
            //It is impossible to both class AND interface
            result.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenIsAttributeAndAbstract_ReturnOnlyAbstractAttributes()
        {
            //Arrange

            //Act
            var result = ToolBX.Reflection4Humans.TypeFetcher.TypeFetcher.Query().IsAttribute().IsAbstract().ToList();

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
            });
        }

        [TestMethod]
        public void WhenIsAttributeAndClassAndAbstract_ReturnOnlyAbstractAttributes()
        {
            //Arrange

            //Act
            var result = ToolBX.Reflection4Humans.TypeFetcher.TypeFetcher.Query().IsAttribute().IsAbstract().IsClass().ToList();

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
            });
        }

        [TestMethod]
        public void WhenIsAttributeAndNotAbstract_ReturnOnlyNonAbstractAttributes()
        {
            //Arrange

            //Act
            var result = ToolBX.Reflection4Humans.TypeFetcher.TypeFetcher.Query().IsAttribute().IsNotAbstract().ToList();

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
            });
        }

        [TestMethod]
        public void WhenIsNotAttribute_ReturnEverythingThatIsNotAnAttribute()
        {
            //Arrange

            //Act
            var result = ToolBX.Reflection4Humans.TypeFetcher.TypeFetcher.Query().IsNotAttribute().ToList();

            //Assert
            result.Should().Contain(new List<Type>
            {
                typeof(DummyStruct),
                typeof(DummyEnum),
                typeof(Dummy),
                typeof(AbstractDummy),
                typeof(IDummy),
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
            var result = ToolBX.Reflection4Humans.TypeFetcher.TypeFetcher.Query().IsNotInterface().ToList();

            //Assert
            result.Should().Contain(new List<Type>
            {
                typeof(DummyStruct),
                typeof(DummyEnum),
                typeof(Dummy),
                typeof(AbstractDummy),
                typeof(DummyAbstractAttribute),
                typeof(DummyAttribute),
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
            var result = ToolBX.Reflection4Humans.TypeFetcher.TypeFetcher.Query().IsNotStruct().ToList();

            //Assert
            result.Should().Contain(new List<Type>
            {
                typeof(IDummy),
                typeof(Dummy),
                typeof(AbstractDummy),
                typeof(DummyAbstractAttribute),
                typeof(DummyAttribute),
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
            var result = ToolBX.Reflection4Humans.TypeFetcher.TypeFetcher.Query().IsStruct().ToList();

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
            });
        }

        [TestMethod]
        public void WhenIsEnum_ReturnEnums()
        {
            //Arrange

            //Act
            var result = ToolBX.Reflection4Humans.TypeFetcher.TypeFetcher.Query().IsEnum().ToList();

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
            });
        }

        [TestMethod]
        public void WhenIsNotEnum_ReturnEverythingButEnums()
        {
            //Arrange

            //Act
            var result = ToolBX.Reflection4Humans.TypeFetcher.TypeFetcher.Query().IsNotEnum().ToList();

            //Assert
            result.Should().Contain(new List<Type>
            {
                typeof(DummyStruct),
                typeof(IDummy),
                typeof(Dummy),
                typeof(AbstractDummy),
                typeof(DummyAbstractAttribute),
                typeof(DummyAttribute),
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
            var result = ToolBX.Reflection4Humans.TypeFetcher.TypeFetcher.Query().IsGeneric().ToList();

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
            });
        }

        [TestMethod]
        public void WhenIsNotGeneric_ReturnAllExceptGenerics()
        {
            //Arrange

            //Act
            var result = ToolBX.Reflection4Humans.TypeFetcher.TypeFetcher.Query().IsNotGeneric().ToList();

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
            var result = ToolBX.Reflection4Humans.TypeFetcher.TypeFetcher.Query().IsGenericTypeDefinition().ToList();

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
            });
        }

        [TestMethod]
        public void WhenIsNotGenericTypeDefinition_ReturnAllExceptGenericTypeDefinitions()
        {
            //Arrange

            //Act
            var result = ToolBX.Reflection4Humans.TypeFetcher.TypeFetcher.Query().IsNotGenericTypeDefinition().ToList();

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
            var result = ToolBX.Reflection4Humans.TypeFetcher.TypeFetcher.Query().Implements(typeof(IDummy)).ToList();

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
            });
        }

        [TestMethod]
        public void WhenHasAttribute_ReturnEverthingThatHasAttribute()
        {
            //Arrange

            //Act
            var result = ToolBX.Reflection4Humans.TypeFetcher.TypeFetcher.Query().HasAttributes(typeof(DummyAttribute)).ToList();

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
            });
        }

        [TestMethod]
        public void WhenThereShouldBeOnlyOneResultAndSingleIsUsed_ReturnSingleResult()
        {
            //Arrange

            //Act
            var result = ToolBX.Reflection4Humans.TypeFetcher.TypeFetcher.Query().HasAttribute(typeof(DummyAttribute)).Single();

            //Assert
            result.Should().Be(typeof(AttributedDummy));
        }

        [TestMethod]
        public void WhenThereShouldBeMultipleResultsAndSingleIsUsed_Throw()
        {
            //Arrange

            //Act
            var action = () => ToolBX.Reflection4Humans.TypeFetcher.TypeFetcher.Query().IsClass().Single();

            //Assert
            action.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public void WhenThereShouldBeOnlyOneResultAndSingleOrDefaultIsUsed_ReturnSingleResult()
        {
            //Arrange

            //Act
            var result = ToolBX.Reflection4Humans.TypeFetcher.TypeFetcher.Query().HasAttribute(typeof(DummyAttribute)).SingleOrDefault();

            //Assert
            result.Should().Be(typeof(AttributedDummy));
        }

        [TestMethod]
        public void WhenThereShouldBeNoResultAndSingleOrDefaultIsUsed_ReturnDefault()
        {
            //Arrange

            //Act
            var result = ToolBX.Reflection4Humans.TypeFetcher.TypeFetcher.Query().HasAttribute(typeof(DummyAttribute)).IsInterface().SingleOrDefault();

            //Assert
            result.Should().BeNull();
        }

        [TestMethod]
        public void WhenThereShouldBeMultipleResultsAndSingleOrDefaultIsUsed_Throw()
        {
            //Arrange

            //Act
            var action = () => ToolBX.Reflection4Humans.TypeFetcher.TypeFetcher.Query().IsClass().SingleOrDefault();

            //Assert
            action.Should().Throw<InvalidOperationException>();
        }
    }
}