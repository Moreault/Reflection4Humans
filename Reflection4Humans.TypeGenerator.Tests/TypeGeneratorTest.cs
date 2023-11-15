namespace Reflection4Humans.TypeGenerator.Tests;

[TestClass]
public class TypeGeneratorTest
{
    public sealed record BogusEventArgs(string Name);

    public delegate void BogusEventHandler(object sender, BogusEventArgs args);

    public interface IBogus
    {
        int this[string a] { get; set; }

        void VoidMethod();
        void VoidMethod(string a, int b);

        string StringMethod();
        string StringMethod(int a, long b, decimal c);

        int GetSetProperty { get; set; }
        char GetProperty { get; }
        string SetProperty { set; }

        event BogusEventHandler OnBogus;

        public static void StaticMethod() => throw new NotImplementedException();

        public void DefaultMethod() => throw new NotImplementedException();

        public string DefaultProperty
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }
    }

    public abstract class AbstractBogus
    {
        public void DoStuff() { }

        public abstract int GetId();

        protected int ProtectedField;

        public int PublicField;

        protected AbstractBogus(string a)
        {

        }

        protected AbstractBogus(int a)
        {

        }

        protected AbstractBogus(string a, int b)
        {

        }
    }

    public sealed class SealedBogus
    {

    }

    public static class StaticBogus
    {

    }

    [TestClass]
    public class From : Tester
    {
        //TODO Test
        [TestMethod]
        public void WhenTypeIsNull_Throw()
        {
            //Arrange
            Type type = null!;

            //Act
            var action = () => ToolBX.Reflection4Humans.TypeGenerator.TypeGenerator.From(type);

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(type));
        }

        [TestMethod]
        public void WhenTypeIsInterface_CreateDummyDynamicallyFromInterface()
        {
            //Arrange

            //Act
            var result = ToolBX.Reflection4Humans.TypeGenerator.TypeGenerator.From<IBogus>();

            //Assert
            result.GetSinglePropertyOrDefault(x => x.Name == "Item" && x.IsGet() && x.IsSet()).Should().NotBeNull();
            result.GetSingleMethodOrDefault(x => x.Name == "VoidMethod" && x.HasNoParameter()).Should().NotBeNull();
            result.GetSingleMethodOrDefault(x => x.Name == "VoidMethod" && x.HasParameters<string, int>()).Should().NotBeNull();
            result.GetSingleMethodOrDefault(x => x.Name == "StringMethod" && x.HasNoParameter()).Should().NotBeNull();
            result.GetSingleMethodOrDefault(x => x.Name == "StringMethod" && x.HasParameters<int, long, decimal>()).Should().NotBeNull();
            result.GetSinglePropertyOrDefault(x => x.Name == "GetSetProperty").Should().NotBeNull();
            result.GetSinglePropertyOrDefault(x => x.Name == "GetProperty" && !x.IsSet()).Should().NotBeNull();
            result.GetSinglePropertyOrDefault(x => x.Name == "SetProperty" && !x.IsGet()).Should().NotBeNull();
            result.GetSingleEventOrDefault(x => x.Name == "OnBogus").Should().NotBeNull();
            result.GetSingleMethodOrDefault(x => x.Name == "StaticMethod").Should().BeNull();
            result.GetSingleMethodOrDefault(x => x.Name == "DefaultMethod").Should().NotBeNull();
            result.GetSinglePropertyOrDefault(x => x.Name == "DefaultProperty").Should().NotBeNull();
            result.Should().Implement<IBogus>();
        }

        [TestMethod]
        public void WhenTypeIsAbstractClass_CreateDynamically()
        {
            //Arrange

            //Act
            var result = ToolBX.Reflection4Humans.TypeGenerator.TypeGenerator.From<AbstractBogus>();

            //Assert
            result.Should().BeDerivedFrom<AbstractBogus>();
            result.GetSingleMethodOrDefault(x => x.Name == "DoStuff").Should().NotBeNull();
            result.GetSingleMethodOrDefault(x => x.Name == "GetId" && !x.IsAbstract).Should().NotBeNull();
            result.GetSingleFieldOrDefault(x => x.Name == "ProtectedField").Should().NotBeNull();
            result.GetSingleFieldOrDefault(x => x.Name == "PublicField").Should().NotBeNull();
        }

        [TestMethod]
        public void WhenTypeIsSealed_Throw()
        {
            //Arrange

            //Act
            var action = () => ToolBX.Reflection4Humans.TypeGenerator.TypeGenerator.From<SealedBogus>();

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.CannotGenerateFromSealedType, nameof(SealedBogus)) + "*");
        }

        [TestMethod]
        public void WhenTypeIsStatic_Throw()
        {
            //Arrange

            //Act
            var action = () => ToolBX.Reflection4Humans.TypeGenerator.TypeGenerator.From(typeof(StaticBogus));

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.CannotGenerateFromStaticType, nameof(StaticBogus)) + "*");
        }

        [TestMethod]
        public void WhenTypeIsInterface_GetSetPropertiesHaveBasicFunctionality()
        {
            //Arrange
            var type = ToolBX.Reflection4Humans.TypeGenerator.TypeGenerator.From<IBogus>();
            var instance = (IBogus)Activator.CreateInstance(type)!;

            var value = Fixture.Create<int>();
            instance.GetSetProperty = value;

            //Act
            var result = instance.GetSetProperty;

            //Assert
            result.Should().Be(value);
        }
    }
}