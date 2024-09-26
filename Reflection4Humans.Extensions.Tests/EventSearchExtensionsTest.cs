namespace Reflection4Humans.Extensions.Tests;

public partial class MemberSearchExtensionsTest
{
    [TestMethod]
    public void GetAllEvents_WhenTypeIsNull_Throw()
    {
        //Arrange
        Type type = null!;

        //Act
        var action = () => type.GetAllEvents();

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(type));
    }

    [TestMethod]
    public void GetAllEvents_WhenGettingAllEvents_ReturnAllOfThem()
    {
        //Arrange

        //Act
        var result = typeof(Garbage).GetAllEvents();

        //Assert
        result.Select(x => x.Name).Should().BeEquivalentTo(new List<string>
        {
            "OnPublic", "OnProtected", "OnInternal", "OnPrivate", "OnStatic"
        });
    }

    [TestMethod]
    public void GetAllEvents_WhenGettingAllInstanceEvents_ReturnOnlyInstanceEvents()
    {
        //Arrange

        //Act
        var result = typeof(Garbage).GetAllEvents(x => x.IsInstance());

        //Assert
        result.Select(x => x.Name).Should().BeEquivalentTo(new List<string>
        {
            "OnPublic", "OnProtected", "OnInternal", "OnPrivate"
        });
    }

    [TestMethod]
    public void GetSingleEvent_WhenTypeIsNull_Throw()
    {
        //Arrange
        Type type = null!;
        var name = Dummy.Create<string>();

        //Act
        var action = () => type.GetSingleEvent(name);

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(type));
    }

    [TestMethod]
    [DataRow("")]
    [DataRow(" ")]
    [DataRow(null)]
    public void GetSingleEvent_WhenNameIsNullOrEmpty_Throw(string name)
    {
        //Arrange

        //Act
        var action = () => typeof(Garbage).GetSingleEvent(name);

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(name));
    }

    [TestMethod]
    public void GetSingleEvent_WhenThereIsNoEventWithName_Throw()
    {
        //Arrange

        //Act
        var action = () => typeof(Garbage).GetSingleEvent(Dummy.Create<string>());

        //Assert
        action.Should().Throw<InvalidOperationException>();
    }

    [TestMethod]
    public void GetSingleEvent_WhenThereIsEventWithName_ReturnEvent()
    {
        //Arrange

        //Act
        var result = typeof(Garbage).GetSingleEvent("OnInternal");

        //Assert
        result.Should().NotBeNull();
    }

    [TestMethod]
    public void GetSingleEvent_WhenGettingSoleProtectedEvent_ReturnIt()
    {
        //Arrange

        //Act
        var result = typeof(Garbage).GetSingleEvent(x => x.IsProtected());

        //Assert
        result.Should().NotBeNull();
    }

    [TestMethod]
    public void GetSingleEventOrDefault_WhenTypeIsNull_Throw()
    {
        //Arrange
        Type type = null!;
        var name = Dummy.Create<string>();

        //Act
        var action = () => type.GetSingleEventOrDefault(name);

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(type));
    }

    [TestMethod]
    [DataRow("")]
    [DataRow(" ")]
    [DataRow(null)]
    public void GetSingleEventOrDefault_WhenNameIsNullOrEmpty_Throw(string name)
    {
        //Arrange

        //Act
        var action = () => typeof(Garbage).GetSingleEventOrDefault(name);

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(name));
    }

    [TestMethod]
    public void GetSingleEventOrDefault_WhenLookingForPropertyByNameButItDoesntExist_ReturnNull()
    {
        //Arrange

        //Act
        var result = typeof(Garbage).GetSingleEventOrDefault("Les Meubles Alexandra");

        //Assert
        result.Should().BeNull();
    }

    [TestMethod]
    public void GetSingleEventOrDefault_WhenThereIsNoEventWithName_ReturnNull()
    {
        //Arrange

        //Act
        var result = typeof(Garbage).GetSingleEventOrDefault(Dummy.Create<string>());

        //Assert
        result.Should().BeNull();
    }

    [TestMethod]
    public void GetSingleEventOrDefault_WhenThereIsEventWithName_ReturnEvent()
    {
        //Arrange

        //Act
        var result = typeof(Garbage).GetSingleEventOrDefault("OnPrivate");

        //Assert
        result.Should().NotBeNull();
    }

    [TestMethod]
    public void GetSingleEventOrDefault_WhenGettingSoleProtectedEvent_ReturnIt()
    {
        //Arrange

        //Act
        var result = typeof(Garbage).GetSingleEventOrDefault(x => x.IsProtected());

        //Assert
        result.Should().NotBeNull();
    }



    [TestMethod]
    [DataRow("")]
    [DataRow(" ")]
    [DataRow(null)]
    public void HasEvent_WhenNameIsNullOrEmpty_Throw(string name)
    {
        //Arrange

        //Act
        var action = () => typeof(Garbage).HasEvent(name);

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(name));
    }

    [TestMethod]
    public void HasEvent_WhenLookingForPropertyByNameButItDoesntExist_False()
    {
        //Arrange

        //Act
        var result = typeof(Garbage).HasEvent("Les Meubles Alexandra");

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void HasEvent_WhenThereIsNoEventWithName_False()
    {
        //Arrange

        //Act
        var result = typeof(Garbage).HasEvent(Dummy.Create<string>());

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void HasEvent_WhenThereIsEventWithName_True()
    {
        //Arrange

        //Act
        var result = typeof(Garbage).HasEvent("OnPrivate");

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void HasEvent_WhenGettingSoleProtectedEvent_True()
    {
        //Arrange

        //Act
        var result = typeof(Garbage).HasEvent(x => x.IsProtected());

        //Assert
        result.Should().BeTrue();

    }
}