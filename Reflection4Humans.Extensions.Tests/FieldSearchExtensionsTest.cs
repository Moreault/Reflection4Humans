namespace Reflection4Humans.Extensions.Tests;

public partial class MemberSearchExtensionsTest
{
    [TestClass]
    public class GetAllFields : Tester
    {
        [TestMethod]
        public void WhenTypeIsNull_Throw()
        {
            //Arrange
            Type type = null!;

            //Act
            var action = () => type.GetAllFields();

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(type));
        }

        [TestMethod]
        public void WhenTypeIsNotNull_ReturnAllFields()
        {
            //Arrange

            //Act
            var result = typeof(Dummy).GetAllFields();

            //Assert
            result.Select(x => x.Name).Should().BeEquivalentTo(new List<string>
            {
                "<Id>k__BackingField", "<PrivateGetSetProperty>k__BackingField", "<GetOnlyProperty>k__BackingField", "_wasPoked", "_setOnlyValue", "_nextId"
            });
        }

        [TestMethod]
        public void WhenSearchingForAllStaticFields_ReturnAllStaticFields()
        {
            //Arrange

            //Act
            var result = typeof(Dummy).GetAllFields(x => x.IsStatic);

            //Assert
            result.Select(x => x.Name).Should().BeEquivalentTo(new List<string>
            {
                "_nextId"
            });
        }
    }

    [TestClass]
    public class GetSingleField : Tester
    {
        //TODO Test
    }

    [TestClass]
    public class GetSingleFieldOrDefault : Tester
    {
        //TODO Test
    }
}