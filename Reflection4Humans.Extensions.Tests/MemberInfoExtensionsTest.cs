using MemberInfoExtensions = ToolBX.Reflection4Humans.Extensions.MemberInfoExtensions;

namespace Reflection4Humans.Extensions.Tests;

[TestClass]
public class MemberInfoExtensionsTest
{
    [TestClass]
    public class IsStatic : Tester
    {
        public record Dummy
        {
            private static int _staticField;
            protected char _instanceField;

            public static long StaticProperty { get; set; }
            public short InstanceProperty { get; }

            public static void StaticMethod()
            {

            }

            internal void InstanceMethod()
            {

            }

            public Dummy() { }

            static Dummy() { }
        }

        public sealed class SealedDummy { }

        public abstract class AbstractDummy { }

        public static class StaticDummy { }

        [TestMethod]
        public void WhenMemberInfoIsNull_Throw()
        {
            //Arrange
            MemberInfo memberInfo = null!;

            //Act
            var action = () => memberInfo.IsStatic();

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(memberInfo));
        }

        [TestMethod]
        public void WhenIsStaticField_ReturnTrue()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Dummy).GetSingleField("_staticField");

            //Act
            var result = memberInfo.IsStatic();

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenIsInstanceField_ReturnFalse()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Dummy).GetSingleField("_instanceField");

            //Act
            var result = memberInfo.IsStatic();

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenIsStaticProperty_ReturnTrue()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Dummy).GetSingleProperty("StaticProperty");

            //Act
            var result = memberInfo.IsStatic();

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenIsInstanceProperty_ReturnFalse()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Dummy).GetSingleProperty("InstanceProperty");

            //Act
            var result = memberInfo.IsStatic();

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenIsStaticMethod_ReturnTrue()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Dummy).GetSingleMethod("StaticMethod");

            //Act
            var result = memberInfo.IsStatic();

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenIsInstanceMethod_ReturnFalse()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Dummy).GetSingleMethod("InstanceMethod");

            //Act
            var result = memberInfo.IsStatic();

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenIsStaticConstructor_ReturnTrue()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Dummy).GetSingleConstructor(x => x.IsStatic);

            //Act
            var result = memberInfo.IsStatic();

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenIsInstanceConstructor_ReturnFalse()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Dummy).GetAllConstructors(x => x.IsInstance() && x.HasNoParameter()).First();

            //Act
            var result = memberInfo.IsStatic();

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenIsDummyMemberInfo_Throw()
        {
            //Arrange

            //Act
            var action = () => new DummyMemberInfo().IsStatic();

            //Assert
            action.Should().Throw<NotSupportedException>().WithMessage(string.Format(Exceptions.MemberKindUnsupported, nameof(MemberInfoExtensions.IsStatic), nameof(DummyMemberInfo)));
        }

        [TestMethod]
        public void WhenIsTypeAndIsStatic_ReturnTrue()
        {
            //Arrange

            //Act
            var result = typeof(StaticDummy).IsStatic();

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenIsTypeAndIsNotStaticButAbstract_ReturnFalse()
        {
            //Arrange

            //Act
            var result = typeof(AbstractDummy).IsStatic();

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenIsTypeAndIsNotStaticButSealed_ReturnFalse()
        {
            //Arrange

            //Act
            var result = typeof(SealedDummy).IsStatic();

            //Assert
            result.Should().BeFalse();
        }
    }

    [TestClass]
    public class IsPrivate : Tester
    {
        public record Dummy
        {
            private int _privateField;
            protected long NonPrivateField;

            private string PrivateProperty
            {
                set => _privatePropertyField = value;
            }
            private string _privatePropertyField;
            internal char NonPrivateProperty { get; }

            private void PrivateMethod()
            {

            }

            public void NonPrivateMethod()
            {

            }

            public Dummy(int privateField)
            {
                _privateField = privateField;
            }

            private Dummy()
            {

            }
        }

        [TestMethod]
        public void WhenMemberInfoIsNull_Throw()
        {
            //Arrange
            MemberInfo memberInfo = null!;

            //Act
            var action = () => memberInfo.IsPrivate();

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(memberInfo));
        }

        [TestMethod]
        public void WhenIsPrivateField_ReturnTrue()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Dummy).GetSingleField("_privateField");

            //Act
            var result = memberInfo.IsPrivate();

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenIsNonPrivateField_ReturnFalse()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Dummy).GetSingleField("NonPrivateField");

            //Act
            var result = memberInfo.IsPrivate();

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenIsPrivateProperty_ReturnTrue()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Dummy).GetSingleProperty("PrivateProperty");

            //Act
            var result = memberInfo.IsPrivate();

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenIsNonPrivateProperty_ReturnFalse()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Dummy).GetSingleProperty("NonPrivateProperty");

            //Act
            var result = memberInfo.IsPrivate();

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenIsPrivateMethod_ReturnTrue()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Dummy).GetSingleMethod("PrivateMethod");

            //Act
            var result = memberInfo.IsPrivate();

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenIsNonPrivateMethod_ReturnFalse()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Dummy).GetSingleMethod("NonPrivateMethod");

            //Act
            var result = memberInfo.IsPrivate();

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenIsPrivateConstructor_ReturnTrue()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Dummy).GetSingleConstructor(x => x.IsPrivate);

            //Act
            var result = memberInfo.IsPrivate();

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenIsNonPrivateConstructor_ReturnFalse()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Dummy).GetSingleConstructor(x => !x.IsPrivate && x.HasParameters<int>());

            //Act
            var result = memberInfo.IsPrivate();

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenIsDummyMemberInfo_Throw()
        {
            //Arrange

            //Act
            var action = () => new DummyMemberInfo().IsPrivate();

            //Assert
            action.Should().Throw<NotSupportedException>().WithMessage(string.Format(Exceptions.MemberKindUnsupported, nameof(MemberInfoExtensions.IsPrivate), nameof(DummyMemberInfo)));
        }
    }

    [TestClass]
    public class IsProtected : Tester
    {
        public record Dummy
        {
            protected int _protectedField;
            private long NonProtectedField;

            protected string ProtectedProperty
            {
                set => _protectedPropertyField = value;
            }
            protected string _protectedPropertyField;
            internal char NonProtectedProperty { get; }

            protected void ProtectedMethod()
            {

            }

            public void NonProtectedMethod()
            {

            }

            public Dummy(int protectedField)
            {
                _protectedField = protectedField;
            }

            protected Dummy()
            {

            }
        }

        [TestMethod]
        public void WhenMemberInfoIsNull_Throw()
        {
            //Arrange
            MemberInfo memberInfo = null!;

            //Act
            var action = () => memberInfo.IsProtected();

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(memberInfo));
        }

        [TestMethod]
        public void WhenIsProtectedField_ReturnTrue()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Dummy).GetSingleField("_protectedField");

            //Act
            var result = memberInfo.IsProtected();

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenIsNonProtectedField_ReturnFalse()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Dummy).GetSingleField("NonProtectedField");

            //Act
            var result = memberInfo.IsProtected();

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenIsProtectedProperty_ReturnTrue()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Dummy).GetSingleProperty("ProtectedProperty");

            //Act
            var result = memberInfo.IsProtected();

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenIsNonProtectedProperty_ReturnFalse()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Dummy).GetSingleProperty("NonProtectedProperty");

            //Act
            var result = memberInfo.IsProtected();

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenIsProtectedMethod_ReturnTrue()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Dummy).GetSingleMethod("ProtectedMethod");

            //Act
            var result = memberInfo.IsProtected();

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenIsNonProtectedMethod_ReturnFalse()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Dummy).GetSingleMethod("NonProtectedMethod");

            //Act
            var result = memberInfo.IsProtected();

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenIsProtectedConstructor_ReturnTrue()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Dummy).GetSingleConstructor(x => x.IsProtected() && x.HasNoParameter());

            //Act
            var result = memberInfo.IsProtected();

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenIsNonProtectedConstructor_ReturnFalse()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Dummy).GetSingleConstructor(x => !x.IsProtected() && x.HasParameters<int>());

            //Act
            var result = memberInfo.IsProtected();

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenIsDummyMemberInfo_Throw()
        {
            //Arrange

            //Act
            var action = () => new DummyMemberInfo().IsProtected();

            //Assert
            action.Should().Throw<NotSupportedException>().WithMessage(string.Format(Exceptions.MemberKindUnsupported, nameof(MemberInfoExtensions.IsProtected), nameof(DummyMemberInfo)));
        }
    }

    [TestClass]
    public class IsInternal : Tester
    {
        public record Dummy
        {
            internal int _internalField;
            protected long NonInternalField;

            internal string InternalProperty
            {
                set => _internalPropertyField = value;
            }
            internal string _internalPropertyField;
            private char NonInternalProperty { get; }

            internal void InternalMethod()
            {

            }

            public void NonInternalMethod()
            {

            }

            public Dummy(int internalField)
            {
                _internalField = internalField;
            }

            internal Dummy()
            {

            }
        }

        internal record InternalDummy{}

        [TestMethod]
        public void WhenMemberInfoIsNull_Throw()
        {
            //Arrange
            MemberInfo memberInfo = null!;

            //Act
            var action = () => memberInfo.IsInternal();

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(memberInfo));
        }

        [TestMethod]
        public void WhenIsInternalField_ReturnTrue()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Dummy).GetSingleField("_internalField");

            //Act
            var result = memberInfo.IsInternal();

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenIsNonInternalField_ReturnFalse()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Dummy).GetSingleField("NonInternalField");

            //Act
            var result = memberInfo.IsInternal();

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenIsInternalProperty_ReturnTrue()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Dummy).GetSingleProperty("InternalProperty");

            //Act
            var result = memberInfo.IsInternal();

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenIsNonInternalProperty_ReturnFalse()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Dummy).GetSingleProperty("NonInternalProperty");

            //Act
            var result = memberInfo.IsInternal();

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenIsInternalMethod_ReturnTrue()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Dummy).GetSingleMethod("InternalMethod");

            //Act
            var result = memberInfo.IsInternal();

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenIsNonInternalMethod_ReturnFalse()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Dummy).GetSingleMethod("NonInternalMethod");

            //Act
            var result = memberInfo.IsInternal();

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenIsInternalConstructor_ReturnTrue()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Dummy).GetSingleConstructor(x => x.IsInternal());

            //Act
            var result = memberInfo.IsInternal();

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenIsNonInternalConstructor_ReturnFalse()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Dummy).GetSingleConstructor(x => !x.IsInternal() && x.HasParameters<int>());

            //Act
            var result = memberInfo.IsInternal();

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenIsDummyMemberInfo_Throw()
        {
            //Arrange

            //Act
            var action = () => new DummyMemberInfo().IsInternal();

            //Assert
            action.Should().Throw<NotSupportedException>().WithMessage(string.Format(Exceptions.MemberKindUnsupported, nameof(MemberInfoExtensions.IsInternal), nameof(DummyMemberInfo)));
        }

        [TestMethod]
        public void WhenIsInternalClass_ReturnTrue()
        {
            //Arrange
            
            //Act
            var result = typeof(InternalDummy).IsInternal();

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenIsPublicClass_ReturnFalse()
        {
            //Arrange

            //Act
            var result = typeof(Dummy).IsInternal();

            //Assert
            result.Should().BeFalse();
        }
    }

    [TestClass]
    public class IsPublic : Tester
    {
        public record Dummy
        {
            public int _publicField;
            protected long NonPublicField;

            public string PublicProperty
            {
                set => _publicPropertyField = value;
            }
            private string _publicPropertyField;
            private char NonPublicProperty { get; }

            public void PublicMethod()
            {

            }

            private void NonPublicMethod()
            {

            }

            private Dummy(int publicField)
            {
                _publicField = publicField;
            }

            public Dummy()
            {

            }
        }

        [TestMethod]
        public void WhenMemberInfoIsNull_Throw()
        {
            //Arrange
            MemberInfo memberInfo = null!;

            //Act
            var action = () => memberInfo.IsPublic();

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(memberInfo));
        }

        [TestMethod]
        public void WhenIsPublicField_ReturnTrue()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Dummy).GetSingleField("_publicField");

            //Act
            var result = memberInfo.IsPublic();

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenIsNonPublicField_ReturnFalse()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Dummy).GetSingleField("NonPublicField");

            //Act
            var result = memberInfo.IsPublic();

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenIsPublicProperty_ReturnTrue()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Dummy).GetSingleProperty("PublicProperty");

            //Act
            var result = memberInfo.IsPublic();

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenIsNonPublicProperty_ReturnFalse()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Dummy).GetSingleProperty("NonPublicProperty");

            //Act
            var result = memberInfo.IsPublic();

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenIsPublicMethod_ReturnTrue()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Dummy).GetSingleMethod("PublicMethod");

            //Act
            var result = memberInfo.IsPublic();

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenIsNonPublicMethod_ReturnFalse()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Dummy).GetSingleMethod("NonPublicMethod");

            //Act
            var result = memberInfo.IsPublic();

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenIsPublicConstructor_ReturnTrue()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Dummy).GetAllConstructors(x => x.IsPublic).First();

            //Act
            var result = memberInfo.IsPublic();

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenIsNonPublicConstructor_ReturnFalse()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Dummy).GetSingleConstructor(x => !x.IsPublic && x.HasParameters<int>());

            //Act
            var result = memberInfo.IsPublic();

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenIsDummyMemberInfo_Throw()
        {
            //Arrange

            //Act
            var action = () => new DummyMemberInfo().IsPublic();

            //Assert
            action.Should().Throw<NotSupportedException>().WithMessage(string.Format(Exceptions.MemberKindUnsupported, nameof(MemberInfoExtensions.IsPublic), nameof(DummyMemberInfo)));
        }
    }

    [TestClass]
    public class IsConstructor : Tester
    {
        public record Dummy
        {
            public string Field;

            public int Property { get; set; }

            public void Method() { }

            private Dummy() { }
        }

        [TestMethod]
        public void WhenMemberInfoIsNull_Throw()
        {
            //Arrange
            MemberInfo memberInfo = null!;

            //Act
            var action = () => memberInfo.IsConstructor();

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(memberInfo));
        }

        [TestMethod]
        public void WhenIsConstructor_ReturnTrue()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Dummy).GetSingleConstructor(x => x.IsPrivate());

            //Act
            var result = memberInfo.IsConstructor();

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenIsMethod_ReturnFalse()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Dummy).GetSingleMethod("Method");

            //Act
            var result = memberInfo.IsConstructor();

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenIsField_ReturnFalse()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Dummy).GetSingleField("Field");

            //Act
            var result = memberInfo.IsConstructor();

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenIsProperty_ReturnFalse()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Dummy).GetSingleProperty("Property");

            //Act
            var result = memberInfo.IsConstructor();

            //Assert
            result.Should().BeFalse();
        }
    }

    [TestClass]
    public class IsMethod : Tester
    {
        public record Dummy
        {
            public string Field;

            public int Property { get; set; }

            public void Method() { }

            private Dummy() { }
        }

        [TestMethod]
        public void WhenMemberInfoIsNull_Throw()
        {
            //Arrange
            MemberInfo memberInfo = null!;

            //Act
            var action = () => memberInfo.IsMethod();

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(memberInfo));
        }

        [TestMethod]
        public void WhenIsConstructor_ReturnTrue()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Dummy).GetSingleConstructor(x => x.IsPrivate());

            //Act
            var result = memberInfo.IsMethod();

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenIsMethod_ReturnTrue()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Dummy).GetSingleMethod("Method");

            //Act
            var result = memberInfo.IsMethod();

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenIsField_ReturnFalse()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Dummy).GetSingleField("Field");

            //Act
            var result = memberInfo.IsMethod();

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenIsProperty_ReturnFalse()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Dummy).GetSingleProperty("Property");

            //Act
            var result = memberInfo.IsMethod();

            //Assert
            result.Should().BeFalse();
        }
    }

    [TestClass]
    public class IsField : Tester
    {
        public record Dummy
        {
            public string Field;

            public int Property { get; set; }

            public void Method() { }

            private Dummy() { }
        }

        [TestMethod]
        public void WhenMemberInfoIsNull_Throw()
        {
            //Arrange
            MemberInfo memberInfo = null!;

            //Act
            var action = () => memberInfo.IsField();

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(memberInfo));
        }

        [TestMethod]
        public void WhenIsConstructor_ReturnFalse()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Dummy).GetSingleConstructor(x => x.IsPrivate());

            //Act
            var result = memberInfo.IsField();

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenIsMethod_ReturnFalse()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Dummy).GetSingleMethod("Method");

            //Act
            var result = memberInfo.IsField();

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenIsField_ReturnTrue()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Dummy).GetSingleField("Field");

            //Act
            var result = memberInfo.IsField();

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenIsProperty_ReturnFalse()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Dummy).GetSingleProperty("Property");

            //Act
            var result = memberInfo.IsField();

            //Assert
            result.Should().BeFalse();
        }
    }

    [TestClass]
    public class IsProperty : Tester
    {
        public record Dummy
        {
            public string Field;

            public int Property { get; set; }

            public void Method() { }

            private Dummy() { }
        }

        //TODO Test
        [TestMethod]
        public void WhenMemberInfoIsNull_Throw()
        {
            //Arrange
            MemberInfo memberInfo = null!;

            //Act
            var action = () => memberInfo.IsProperty();

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(memberInfo));
        }

        [TestMethod]
        public void WhenIsConstructor_ReturnFalse()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Dummy).GetSingleConstructor(x => x.IsPrivate());

            //Act
            var result = memberInfo.IsProperty();

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenIsMethod_ReturnFalse()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Dummy).GetSingleMethod("Method");

            //Act
            var result = memberInfo.IsProperty();

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenIsField_ReturnFalse()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Dummy).GetSingleField("Field");

            //Act
            var result = memberInfo.IsProperty();

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenIsProperty_ReturnTrue()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Dummy).GetSingleProperty("Property");

            //Act
            var result = memberInfo.IsProperty();

            //Assert
            result.Should().BeTrue();
        }
    }
}