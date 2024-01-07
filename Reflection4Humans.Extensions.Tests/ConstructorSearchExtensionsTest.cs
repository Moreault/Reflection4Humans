namespace Reflection4Humans.Extensions.Tests;

public partial class MemberSearchExtensionsTest
{
    [TestClass]
    public class GetAllConstructors : Tester
    {
        [TestMethod]
        public void WhenTypeIsNull_Throw()
        {
            //Arrange
            Type type = null!;

            //Act
            var action = () => type.GetAllConstructors();

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(type));
        }

        [TestMethod]
        public void WhenGettingAllConstructors_ReturnAllOfThem()
        {
            //Arrange

            //Act
            var result = typeof(Dummy).GetAllConstructors();

            //Assert
            result.Should().HaveCount(3);
        }

        [TestMethod]
        public void WhenGettingAllPublicConstructors_ReturnOnlyPublicConstructors()
        {
            //Arrange

            //Act
            var result = typeof(Dummy).GetAllConstructors(x => x.IsPublic);

            //Assert
            result.Should().HaveCount(2);
        }
    }

    [TestClass]
    public class GetSingleConstructor : Tester
    {
        [TestMethod]
        public void WhenMultipleConstructorsFitPredicate_Throw()
        {
            //Arrange

            //Act
            var action = () => typeof(Dummy).GetSingleConstructor(x => x.HasNoParameter());

            //Assert
            action.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public void WhenNoConstructorFitPredicate_Throw()
        {
            //Arrange

            //Act
            var action = () => typeof(Dummy).GetSingleConstructor(x => x.IsInternal());

            //Assert
            action.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public void WhenExactlyOneConstructorFitsPredicate_ReturnThatConstructor()
        {
            //Arrange

            //Act
            var result= typeof(Dummy).GetSingleConstructor(x => x.IsProtected());

            //Assert
            result.Should().NotBeNull();
        }
    }

    [TestClass]
    public class GetSingleConstructorOrDefault : Tester
    {
        [TestMethod]
        public void WhenMultipleConstructorsFitPredicate_Throw()
        {
            //Arrange

            //Act
            var action = () => typeof(Dummy).GetSingleConstructorOrDefault(x => x.HasNoParameter());

            //Assert
            action.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public void WhenNoConstructorFitPredicate_DoNotThrow()
        {
            //Arrange

            //Act
            var result = typeof(Dummy).GetSingleConstructorOrDefault(x => x.IsInternal());

            //Assert
            result.Should().BeNull();
        }

        [TestMethod]
        public void WhenExactlyOneConstructorFitsPredicate_ReturnThatConstructor()
        {
            //Arrange

            //Act
            var result = typeof(Dummy).GetSingleConstructorOrDefault(x => x.IsProtected());

            //Assert
            result.Should().NotBeNull();
        }
    }
}