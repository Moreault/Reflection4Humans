namespace Reflection4Humans.Extensions.Tests;

[TestClass]
public sealed class PropertyOrFieldExtensionsTests : Tester
{
    public class Garbage
    {
        public int ValueField;

        public string ValueProperty { get; set; }

        public float ReadOnly { get; }

        public char WriteOnly
        {
            set => _writeOnly = value;
        }
        private char _writeOnly;
    }

    //TODO Test
    [TestMethod]
    public void GetAllPropertiesOrFields_WhenTypeIsNull_Throw()
    {
        //Arrange
        Type type = null!;

        //Act
        var action = () => type.GetAllPropertiesOrFields();

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(type));
    }

    [TestMethod]
    public void GetAllPropertiesOrFields_WhenPredicateIsNull_ReturnAllPropertiesAndFields()
    {
        //Arrange

        //Act
        var result = typeof(Garbage).GetAllPropertiesOrFields();

        //Assert
        var fields = typeof(Garbage).GetAllFields().OfType<MemberInfo>();
        var properties = typeof(Garbage).GetAllProperties().OfType<MemberInfo>();
        result.Should().BeEquivalentTo(fields.Concat(properties));
    }

    [TestMethod]
    public void GetAllPropertiesOrFields_WhenSeekingAllExcludingBackingFields_DoNotReturnAutomaticAndCustomBackingFields()
    {
        //Arrange

        //Act
        var result = typeof(Garbage).GetAllPropertiesOrFields(x => !x.IsBackingField());

        //Assert
        result.Should().BeEquivalentTo(new List<MemberInfo>
        {
            typeof(Garbage).GetSingleField(x => x.Name == "ValueField"),
            typeof(Garbage).GetSingleProperty(x => x.Name == "ValueProperty"),
            typeof(Garbage).GetSingleProperty(x => x.Name == "ReadOnly"),
            typeof(Garbage).GetSingleProperty(x => x.Name == "WriteOnly")
        });
    }


}