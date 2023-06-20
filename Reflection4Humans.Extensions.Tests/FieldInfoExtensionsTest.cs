namespace Reflection4Humans.Extensions.Tests;

[TestClass]
public class FieldInfoExtensionsTest
{
    public record Dummy
    {
        public int PublicField;
        internal uint InternalField;
        protected long ProtectedField;
        protected internal char ProtectedInternalField;
        private string _privateField;
        private protected string PrivateProtectedField;
    }

    [TestClass]
    public class GetAccessModifier : Tester
    {
        [TestMethod]
        public void WhenFieldInfoIsNull_Throw()
        {
            //Arrange
            FieldInfo fieldInfo = null!;

            //Act
            var action = () => fieldInfo.GetAccessModifier();

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(fieldInfo));
        }

        [TestMethod]
        public void WhenIsPublic_ReturnPublic()
        {
            //Arrange
            var fieldInfo = typeof(Dummy).GetSingleField(nameof(Dummy.PublicField));

            //Act
            var result = fieldInfo.GetAccessModifier();

            //Assert
            result.Should().Be(AccessModifier.Public);
        }

        [TestMethod]
        public void WhenIsInternal_ReturnInternal()
        {
            //Arrange
            var fieldInfo = typeof(Dummy).GetSingleField(nameof(Dummy.InternalField));

            //Act
            var result = fieldInfo.GetAccessModifier();

            //Assert
            result.Should().Be(AccessModifier.Internal);
        }

        [TestMethod]
        public void WhenIsProtectedInternal_ReturnProtectedInternal()
        {
            //Arrange
            var fieldInfo = typeof(Dummy).GetSingleField("ProtectedInternalField");

            //Act
            var result = fieldInfo.GetAccessModifier();

            //Assert
            result.Should().Be(AccessModifier.ProtectedInternal);
        }

        [TestMethod]
        public void WhenIsProtected_ReturnProtected()
        {
            //Arrange
            var fieldInfo = typeof(Dummy).GetSingleField("ProtectedField");

            //Act
            var result = fieldInfo.GetAccessModifier();

            //Assert
            result.Should().Be(AccessModifier.Protected);
        }

        [TestMethod]
        public void WhenIsPrivate_ReturnPrivate()
        {
            //Arrange
            var fieldInfo = typeof(Dummy).GetSingleField("_privateField");

            //Act
            var result = fieldInfo.GetAccessModifier();

            //Assert
            result.Should().Be(AccessModifier.Private);
        }

        [TestMethod]
        public void WhenIsPrivateProtected_ReturnPrivateProtected()
        {
            //Arrange
            var fieldInfo = typeof(Dummy).GetSingleField("PrivateProtectedField");

            //Act
            var result = fieldInfo.GetAccessModifier();

            //Assert
            result.Should().Be(AccessModifier.PrivateProtected);
        }
    }
}