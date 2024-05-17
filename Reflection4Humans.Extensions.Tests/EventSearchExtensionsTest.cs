﻿namespace Reflection4Humans.Extensions.Tests;

public partial class MemberSearchExtensionsTest
{
    [TestClass]
    public class GetAllEvents : Tester
    {
        [TestMethod]
        public void WhenTypeIsNull_Throw()
        {
            //Arrange
            Type type = null!;

            //Act
            var action = () => type.GetAllEvents();

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(type));
        }

        [TestMethod]
        public void WhenGettingAllEvents_ReturnAllOfThem()
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
        public void WhenGettingAllInstanceEvents_ReturnOnlyInstanceEvents()
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
    }

    [TestClass]
    public class GetSingleEvent : Tester
    {
        [TestMethod]
        public void WhenTypeIsNull_Throw()
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
        public void WhenNameIsNullOrEmpty_Throw(string name)
        {
            //Arrange

            //Act
            var action = () => typeof(Garbage).GetSingleEvent(name);

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(name));
        }

        [TestMethod]
        public void WhenThereIsNoEventWithName_Throw()
        {
            //Arrange

            //Act
            var action = () => typeof(Garbage).GetSingleEvent(Dummy.Create<string>());

            //Assert
            action.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public void WhenThereIsEventWithName_ReturnEvent()
        {
            //Arrange

            //Act
            var result = typeof(Garbage).GetSingleEvent("OnInternal");

            //Assert
            result.Should().NotBeNull();
        }

        [TestMethod]
        public void WhenGettingSoleProtectedEvent_ReturnIt()
        {
            //Arrange

            //Act
            var result = typeof(Garbage).GetSingleEvent(x => x.IsProtected());

            //Assert
            result.Should().NotBeNull();
        }
    }

    [TestClass]
    public class GetSingleEventOrDefault : Tester
    {
        [TestMethod]
        public void WhenTypeIsNull_Throw()
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
        public void WhenNameIsNullOrEmpty_Throw(string name)
        {
            //Arrange

            //Act
            var action = () => typeof(Garbage).GetSingleEventOrDefault(name);

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(name));
        }

        [TestMethod]
        public void WhenLookingForPropertyByNameButItDoesntExist_ReturnNull()
        {
            //Arrange

            //Act
            var result = typeof(Garbage).GetSingleEventOrDefault("Les Meubles Alexandra");

            //Assert
            result.Should().BeNull();
        }

        [TestMethod]
        public void WhenThereIsNoEventWithName_ReturnNull()
        {
            //Arrange

            //Act
            var result = typeof(Garbage).GetSingleEventOrDefault(Dummy.Create<string>());

            //Assert
            result.Should().BeNull();
        }

        [TestMethod]
        public void WhenThereIsEventWithName_ReturnEvent()
        {
            //Arrange

            //Act
            var result = typeof(Garbage).GetSingleEventOrDefault("OnPrivate");

            //Assert
            result.Should().NotBeNull();
        }

        [TestMethod]
        public void WhenGettingSoleProtectedEvent_ReturnIt()
        {
            //Arrange

            //Act
            var result = typeof(Garbage).GetSingleEventOrDefault(x => x.IsProtected());

            //Assert
            result.Should().NotBeNull();
        }
    }
}