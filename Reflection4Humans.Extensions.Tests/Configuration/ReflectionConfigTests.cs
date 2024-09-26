namespace Reflection4Humans.Extensions.Tests.Configuration;

[TestClass]
public sealed class ReflectionConfigTests : Tester
{
    [TestMethod]
    public void Add_WhenIsNotEmpty_AddToConventions()
    {
        //Arrange
        var conventions = Dummy.CreateMany<BackingFieldConvention>().ToArray();

        //Act
        ReflectionConfig.Add(conventions);

        //Assert
        ReflectionConfig.BackingFieldConventions.Should().BeEquivalentTo(new List<BackingFieldConvention>
        {
            BackingFieldConvention.Csharp,
        }.Concat(conventions));
    }

    [TestMethod]
    public void Set_WhenIsNotEmpty_ClearConventionsAndUseOnlyThoseProvided()
    {
        //Arrange
        var conventions = Dummy.CreateMany<BackingFieldConvention>().ToArray();

        //Act
        ReflectionConfig.Set(conventions);

        //Assert
        ReflectionConfig.BackingFieldConventions.Should().BeEquivalentTo(conventions);
    }
}