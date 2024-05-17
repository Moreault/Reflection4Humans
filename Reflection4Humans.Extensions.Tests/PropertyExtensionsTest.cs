namespace Reflection4Humans.Extensions.Tests;

[TestClass]
public class PropertyExtensionsTest
{
    public record Garbage
    {
        public int InstanceGetOnly { get; }

        private string InstanceSetOnly
        {
            set => _instanceSetOnly = value;
        }
        private string _instanceSetOnly;

        internal int InstanceGetSet { get; set; }

        public static int StaticGetOnly { get; }

        private static string StaticSetOnly
        {
            set => _staticSetOnly = value;
        }
        private static string _staticSetOnly;

        internal static int StaticGetSet { get; set; }

        public int this[string index]
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }
    }

    [TestClass]
    public class IsStatic : Tester
    {
        [TestMethod]
        public void WhenPropertyInfoIsNull_Throw()
        {
            //Arrange
            PropertyInfo propertyInfo = null!;

            //Act
            var action = () => propertyInfo.IsStatic();

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(propertyInfo));
        }

        [TestMethod]
        public void WhenPropertyIsInstanceWithGetOnly_ReturnFalse()
        {
            //Arrange
            var propertyInfo = typeof(Garbage).GetSingleProperty("InstanceGetOnly");

            //Act
            var result = propertyInfo.IsStatic();

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenPropertyIsInstanceWithSetOnly_ReturnFalse()
        {
            //Arrange
            var propertyInfo = typeof(Garbage).GetSingleProperty("InstanceSetOnly");

            //Act
            var result = propertyInfo.IsStatic();

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenPropertyIsInstanceWithGetSet_ReturnFalse()
        {
            //Arrange
            var propertyInfo = typeof(Garbage).GetSingleProperty("InstanceGetSet");

            //Act
            var result = propertyInfo.IsStatic();

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenPropertyIsStaticWithGetOnly_ReturnTrue()
        {
            //Arrange
            var propertyInfo = typeof(Garbage).GetSingleProperty("StaticGetOnly");

            //Act
            var result = propertyInfo.IsStatic();

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenPropertyIsStaticWithSetOnly_ReturnTrue()
        {
            //Arrange
            var propertyInfo = typeof(Garbage).GetSingleProperty("StaticSetOnly");

            //Act
            var result = propertyInfo.IsStatic();

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenPropertyIsStaticWithGetSet_ReturnTrue()
        {
            //Arrange
            var propertyInfo = typeof(Garbage).GetSingleProperty("StaticGetSet");

            //Act
            var result = propertyInfo.IsStatic();

            //Assert
            result.Should().BeTrue();
        }
    }

    [TestClass]
    public class IsGet : Tester
    {
        [TestMethod]
        public void WhenPropertyInfoIsNull_Throw()
        {
            //Arrange
            PropertyInfo propertyInfo = null!;

            //Act
            var action = () => propertyInfo.IsGet();

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(propertyInfo));
        }

        [TestMethod]
        public void WhenPropertyInfoHasGetOnly_ReturnTrue()
        {
            //Arrange
            var propertyInfo = typeof(Garbage).GetSingleProperty("InstanceGetOnly");

            //Act
            var result = propertyInfo.IsGet();

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenPropertyInfoHasBothGetAndSet_ReturnTrue()
        {
            //Arrange
            var propertyInfo = typeof(Garbage).GetSingleProperty("InstanceGetSet");

            //Act
            var result = propertyInfo.IsGet();

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenPropertyInfoHasNoGet_ReturnFalse()
        {
            //Arrange
            var propertyInfo = typeof(Garbage).GetSingleProperty("InstanceSetOnly");

            //Act
            var result = propertyInfo.IsGet();

            //Assert
            result.Should().BeFalse();
        }
    }

    [TestClass]
    public class IsSet : Tester
    {
        [TestMethod]
        public void WhenPropertyInfoIsNull_Throw()
        {
            //Arrange
            PropertyInfo propertyInfo = null!;

            //Act
            var action = () => propertyInfo.IsSet();

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(propertyInfo));
        }

        [TestMethod]
        public void WhenPropertyInfoHasGetOnly_ReturnFalse()
        {
            //Arrange
            var propertyInfo = typeof(Garbage).GetSingleProperty("InstanceGetOnly");

            //Act
            var result = propertyInfo.IsSet();

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenPropertyInfoHasBothGetAndSet_ReturnTrue()
        {
            //Arrange
            var propertyInfo = typeof(Garbage).GetSingleProperty("InstanceGetSet");

            //Act
            var result = propertyInfo.IsSet();

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenPropertyInfoHasNoGet_ReturnTrue()
        {
            //Arrange
            var propertyInfo = typeof(Garbage).GetSingleProperty("InstanceSetOnly");

            //Act
            var result = propertyInfo.IsSet();

            //Assert
            result.Should().BeTrue();
        }
    }

    [TestClass]
    public class IsIndexer : Tester
    {
        [TestMethod]
        public void WhenPropertInfoIsNull_Throw()
        {
            //Arrange
            PropertyInfo propertyInfo = null!;

            //Act
            var action = () => propertyInfo.IsIndexer();

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(propertyInfo));
        }

        [TestMethod]
        public void WhenIsRegularGetOnly_ReturnFalse()
        {
            //Arrange
            var propertyInfo = typeof(Garbage).GetSingleProperty("InstanceGetOnly");

            //Act
            var result = propertyInfo.IsIndexer();

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenIsIndexer_ReturnFalse()
        {
            //Arrange
            var propertyInfo = typeof(Garbage).GetSingleProperty("Item");

            //Act
            var result = propertyInfo.IsIndexer();

            //Assert
            result.Should().BeTrue();
        }
    }
}