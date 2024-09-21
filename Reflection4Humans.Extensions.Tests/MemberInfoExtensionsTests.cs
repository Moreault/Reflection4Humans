using MemberInfoExtensions = ToolBX.Reflection4Humans.Extensions.MemberInfoExtensions;

namespace Reflection4Humans.Extensions.Tests;

[TestClass]
public class MemberInfoExtensionsTests
{
    [TestClass]
    public class IsStatic : Tester
    {
        public delegate void TestEventHandler(object sender, EventArgs e);

        public record Garbage
        {
            private static int _staticField;
            protected char _instanceField;

            public static long StaticProperty { get; set; }
            public short InstanceProperty { get; }

            public static event TestEventHandler OnStaticTest;
            public event TestEventHandler OnTest;

            public static void StaticMethod()
            {

            }

            internal void InstanceMethod()
            {

            }

            public Garbage() { }

            static Garbage() { }
        }

        public sealed class SealedGarbage { }

        public abstract class AbstractGarbage { }

        public static class StaticGarbage { }

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
            MemberInfo memberInfo = typeof(Garbage).GetSingleField("_staticField");

            //Act
            var result = memberInfo.IsStatic();

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenIsInstanceField_ReturnFalse()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Garbage).GetSingleField("_instanceField");

            //Act
            var result = memberInfo.IsStatic();

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenIsStaticProperty_ReturnTrue()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Garbage).GetSingleProperty("StaticProperty");

            //Act
            var result = memberInfo.IsStatic();

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenIsInstanceProperty_ReturnFalse()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Garbage).GetSingleProperty("InstanceProperty");

            //Act
            var result = memberInfo.IsStatic();

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenIsStaticMethod_ReturnTrue()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Garbage).GetSingleMethod("StaticMethod");

            //Act
            var result = memberInfo.IsStatic();

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenIsInstanceMethod_ReturnFalse()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Garbage).GetSingleMethod("InstanceMethod");

            //Act
            var result = memberInfo.IsStatic();

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenIsStaticConstructor_ReturnTrue()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Garbage).GetSingleConstructor(x => x.IsStatic);

            //Act
            var result = memberInfo.IsStatic();

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenIsInstanceConstructor_ReturnFalse()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Garbage).GetAllConstructors(x => x.IsInstance() && x.HasNoParameter()).First();

            //Act
            var result = memberInfo.IsStatic();

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenIsGarbageMemberInfo_Throw()
        {
            //Arrange

            //Act
            var action = () => new GarbageMemberInfo().IsStatic();

            //Assert
            action.Should().Throw<NotSupportedException>().WithMessage(string.Format(Exceptions.MemberKindUnsupported, nameof(MemberInfoExtensions.IsStatic), nameof(GarbageMemberInfo)));
        }

        [TestMethod]
        public void WhenIsTypeAndIsStatic_ReturnTrue()
        {
            //Arrange

            //Act
            var result = typeof(StaticGarbage).IsStatic();

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenIsTypeAndIsNotStaticButAbstract_ReturnFalse()
        {
            //Arrange

            //Act
            var result = typeof(AbstractGarbage).IsStatic();

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenIsTypeAndIsNotStaticButSealed_ReturnFalse()
        {
            //Arrange

            //Act
            var result = typeof(SealedGarbage).IsStatic();

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenEventIsStatic_ReturnTrue()
        {
            //Arrange

            //Act
            var result = typeof(Garbage).GetSingleEvent(nameof(Garbage.OnStaticTest)).IsStatic();

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenEventIsNotStatic_ReturnFalse()
        {
            //Arrange

            //Act
            var result = typeof(Garbage).GetSingleEvent(nameof(Garbage.OnTest)).IsStatic();

            //Assert
            result.Should().BeFalse();
        }
    }

    [TestClass]
    public class IsPrivate : Tester
    {
        public delegate void TestEventHandler(object sender, EventArgs e);

        public record Garbage
        {
            private int _privateField;
            protected long NonPrivateField;

            private string PrivateProperty
            {
                set => _privatePropertyField = value;
            }
            private string _privatePropertyField;
            internal char NonPrivateProperty { get; }

            public event TestEventHandler OnPublic;
            private event TestEventHandler OnPrivate;

            private void PrivateMethod()
            {

            }

            public void NonPrivateMethod()
            {

            }

            public Garbage(int privateField)
            {
                _privateField = privateField;
            }

            private Garbage()
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
            MemberInfo memberInfo = typeof(Garbage).GetSingleField("_privateField");

            //Act
            var result = memberInfo.IsPrivate();

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenIsNonPrivateField_ReturnFalse()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Garbage).GetSingleField("NonPrivateField");

            //Act
            var result = memberInfo.IsPrivate();

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenIsPrivateProperty_ReturnTrue()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Garbage).GetSingleProperty("PrivateProperty");

            //Act
            var result = memberInfo.IsPrivate();

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenIsNonPrivateProperty_ReturnFalse()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Garbage).GetSingleProperty("NonPrivateProperty");

            //Act
            var result = memberInfo.IsPrivate();

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenIsPrivateMethod_ReturnTrue()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Garbage).GetSingleMethod("PrivateMethod");

            //Act
            var result = memberInfo.IsPrivate();

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenIsNonPrivateMethod_ReturnFalse()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Garbage).GetSingleMethod("NonPrivateMethod");

            //Act
            var result = memberInfo.IsPrivate();

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenIsPrivateConstructor_ReturnTrue()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Garbage).GetSingleConstructor(x => x.IsPrivate);

            //Act
            var result = memberInfo.IsPrivate();

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenIsNonPrivateConstructor_ReturnFalse()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Garbage).GetSingleConstructor(x => !x.IsPrivate && x.HasParameters<int>());

            //Act
            var result = memberInfo.IsPrivate();

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenIsGarbageMemberInfo_Throw()
        {
            //Arrange

            //Act
            var action = () => new GarbageMemberInfo().IsPrivate();

            //Assert
            action.Should().Throw<NotSupportedException>().WithMessage(string.Format(Exceptions.MemberKindUnsupported, nameof(MemberInfoExtensions.IsPrivate), nameof(GarbageMemberInfo)));
        }

        [TestMethod]
        public void WhenEventIsPrivate_ReturnTrue()
        {
            //Arrange

            //Act
            var result = typeof(Garbage).GetSingleEvent("OnPrivate").IsPrivate();

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenEventIsNotPrivate_ReturnFalse()
        {
            //Arrange

            //Act
            var result = typeof(Garbage).GetSingleEvent("OnPublic").IsPrivate();

            //Assert
            result.Should().BeFalse();
        }
    }

    [TestClass]
    public class IsProtected : Tester
    {
        public delegate void TestEventHandler(object sender, EventArgs e);

        public record Garbage
        {
            protected int _protectedField;
            private long NonProtectedField;

            protected string ProtectedProperty
            {
                set => _protectedPropertyField = value;
            }
            protected string _protectedPropertyField;
            internal char NonProtectedProperty { get; }

            public event TestEventHandler OnPublic;
            protected event TestEventHandler OnProtected;

            protected void ProtectedMethod()
            {

            }

            public void NonProtectedMethod()
            {

            }

            public Garbage(int protectedField)
            {
                _protectedField = protectedField;
            }

            protected Garbage()
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
            MemberInfo memberInfo = typeof(Garbage).GetSingleField("_protectedField");

            //Act
            var result = memberInfo.IsProtected();

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenIsNonProtectedField_ReturnFalse()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Garbage).GetSingleField("NonProtectedField");

            //Act
            var result = memberInfo.IsProtected();

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenIsProtectedProperty_ReturnTrue()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Garbage).GetSingleProperty("ProtectedProperty");

            //Act
            var result = memberInfo.IsProtected();

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenIsNonProtectedProperty_ReturnFalse()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Garbage).GetSingleProperty("NonProtectedProperty");

            //Act
            var result = memberInfo.IsProtected();

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenIsProtectedMethod_ReturnTrue()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Garbage).GetSingleMethod("ProtectedMethod");

            //Act
            var result = memberInfo.IsProtected();

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenIsNonProtectedMethod_ReturnFalse()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Garbage).GetSingleMethod("NonProtectedMethod");

            //Act
            var result = memberInfo.IsProtected();

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenIsProtectedConstructor_ReturnTrue()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Garbage).GetSingleConstructor(x => x.IsProtected() && x.HasNoParameter());

            //Act
            var result = memberInfo.IsProtected();

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenIsNonProtectedConstructor_ReturnFalse()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Garbage).GetSingleConstructor(x => !x.IsProtected() && x.HasParameters<int>());

            //Act
            var result = memberInfo.IsProtected();

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenIsGarbageMemberInfo_Throw()
        {
            //Arrange

            //Act
            var action = () => new GarbageMemberInfo().IsProtected();

            //Assert
            action.Should().Throw<NotSupportedException>().WithMessage(string.Format(Exceptions.MemberKindUnsupported, nameof(MemberInfoExtensions.IsProtected), nameof(GarbageMemberInfo)));
        }

        [TestMethod]
        public void WhenEventIsProtected_ReturnTrue()
        {
            //Arrange

            //Act
            var result = typeof(Garbage).GetSingleEvent("OnProtected").IsProtected();

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenEventIsNotProtected_ReturnFalse()
        {
            //Arrange

            //Act
            var result = typeof(Garbage).GetSingleEvent("OnPublic").IsProtected();

            //Assert
            result.Should().BeFalse();
        }
    }

    [TestClass]
    public class IsInternal : Tester
    {
        public delegate void TestEventHandler(object sender, EventArgs e);

        public record Garbage
        {
            internal int _internalField;
            protected long NonInternalField;

            internal string InternalProperty
            {
                set => _internalPropertyField = value;
            }
            internal string _internalPropertyField;
            private char NonInternalProperty { get; }

            public event TestEventHandler OnPublic;
            internal event TestEventHandler OnInternal;

            internal void InternalMethod()
            {

            }

            public void NonInternalMethod()
            {

            }

            public Garbage(int internalField)
            {
                _internalField = internalField;
            }

            internal Garbage()
            {

            }
        }

        internal record InternalGarbage { }

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
            MemberInfo memberInfo = typeof(Garbage).GetSingleField("_internalField");

            //Act
            var result = memberInfo.IsInternal();

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenIsNonInternalField_ReturnFalse()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Garbage).GetSingleField("NonInternalField");

            //Act
            var result = memberInfo.IsInternal();

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenIsInternalProperty_ReturnTrue()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Garbage).GetSingleProperty("InternalProperty");

            //Act
            var result = memberInfo.IsInternal();

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenIsNonInternalProperty_ReturnFalse()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Garbage).GetSingleProperty("NonInternalProperty");

            //Act
            var result = memberInfo.IsInternal();

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenIsInternalMethod_ReturnTrue()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Garbage).GetSingleMethod("InternalMethod");

            //Act
            var result = memberInfo.IsInternal();

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenIsNonInternalMethod_ReturnFalse()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Garbage).GetSingleMethod("NonInternalMethod");

            //Act
            var result = memberInfo.IsInternal();

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenIsInternalConstructor_ReturnTrue()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Garbage).GetSingleConstructor(x => x.IsInternal());

            //Act
            var result = memberInfo.IsInternal();

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenIsNonInternalConstructor_ReturnFalse()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Garbage).GetSingleConstructor(x => !x.IsInternal() && x.HasParameters<int>());

            //Act
            var result = memberInfo.IsInternal();

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenIsGarbageMemberInfo_Throw()
        {
            //Arrange

            //Act
            var action = () => new GarbageMemberInfo().IsInternal();

            //Assert
            action.Should().Throw<NotSupportedException>().WithMessage(string.Format(Exceptions.MemberKindUnsupported, nameof(MemberInfoExtensions.IsInternal), nameof(GarbageMemberInfo)));
        }

        [TestMethod]
        public void WhenIsInternalClass_ReturnTrue()
        {
            //Arrange

            //Act
            var result = typeof(InternalGarbage).IsInternal();

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenIsPublicClass_ReturnFalse()
        {
            //Arrange

            //Act
            var result = typeof(Garbage).IsInternal();

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenEventIsInternal_ReturnTrue()
        {
            //Arrange

            //Act
            var result = typeof(Garbage).GetSingleEvent("OnInternal").IsInternal();

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenEventIsNotInternal_ReturnFalse()
        {
            //Arrange

            //Act
            var result = typeof(Garbage).GetSingleEvent("OnPublic").IsInternal();

            //Assert
            result.Should().BeFalse();
        }
    }

    [TestClass]
    public class IsPublic : Tester
    {
        public delegate void TestEventHandler(object sender, EventArgs e);

        public record Garbage
        {
            public int _publicField;
            protected long NonPublicField;

            public string PublicProperty
            {
                set => _publicPropertyField = value;
            }
            private string _publicPropertyField;
            private char NonPublicProperty { get; }

            public event TestEventHandler OnPublic;
            internal event TestEventHandler OnInternal;

            public void PublicMethod()
            {

            }

            private void NonPublicMethod()
            {

            }

            private Garbage(int publicField)
            {
                _publicField = publicField;
            }

            public Garbage()
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
            MemberInfo memberInfo = typeof(Garbage).GetSingleField("_publicField");

            //Act
            var result = memberInfo.IsPublic();

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenIsNonPublicField_ReturnFalse()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Garbage).GetSingleField("NonPublicField");

            //Act
            var result = memberInfo.IsPublic();

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenIsPublicProperty_ReturnTrue()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Garbage).GetSingleProperty("PublicProperty");

            //Act
            var result = memberInfo.IsPublic();

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenIsNonPublicProperty_ReturnFalse()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Garbage).GetSingleProperty("NonPublicProperty");

            //Act
            var result = memberInfo.IsPublic();

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenIsPublicMethod_ReturnTrue()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Garbage).GetSingleMethod("PublicMethod");

            //Act
            var result = memberInfo.IsPublic();

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenIsNonPublicMethod_ReturnFalse()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Garbage).GetSingleMethod("NonPublicMethod");

            //Act
            var result = memberInfo.IsPublic();

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenIsPublicConstructor_ReturnTrue()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Garbage).GetAllConstructors(x => x.IsPublic).First();

            //Act
            var result = memberInfo.IsPublic();

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenIsNonPublicConstructor_ReturnFalse()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Garbage).GetSingleConstructor(x => !x.IsPublic && x.HasParameters<int>());

            //Act
            var result = memberInfo.IsPublic();

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenIsGarbageMemberInfo_Throw()
        {
            //Arrange

            //Act
            var action = () => new GarbageMemberInfo().IsPublic();

            //Assert
            action.Should().Throw<NotSupportedException>().WithMessage(string.Format(Exceptions.MemberKindUnsupported, nameof(MemberInfoExtensions.IsPublic), nameof(GarbageMemberInfo)));
        }

        [TestMethod]
        public void WhenEventIsPublic_ReturnTrue()
        {
            //Arrange

            //Act
            var result = typeof(Garbage).GetSingleEvent("OnPublic").IsPublic();

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenEventIsNotPublic_ReturnFalse()
        {
            //Arrange

            //Act
            var result = typeof(Garbage).GetSingleEvent("OnInternal").IsPublic();

            //Assert
            result.Should().BeFalse();
        }
    }

    [TestClass]
    public class IsConstructor : Tester
    {
        public record Garbage
        {
            public string Field;

            public int Property { get; set; }

            public void Method() { }

            private Garbage() { }
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
            MemberInfo memberInfo = typeof(Garbage).GetSingleConstructor(x => x.IsPrivate());

            //Act
            var result = memberInfo.IsConstructor();

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenIsMethod_ReturnFalse()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Garbage).GetSingleMethod("Method");

            //Act
            var result = memberInfo.IsConstructor();

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenIsField_ReturnFalse()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Garbage).GetSingleField("Field");

            //Act
            var result = memberInfo.IsConstructor();

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenIsProperty_ReturnFalse()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Garbage).GetSingleProperty("Property");

            //Act
            var result = memberInfo.IsConstructor();

            //Assert
            result.Should().BeFalse();
        }
    }

    [TestClass]
    public class IsMethod : Tester
    {
        public record Garbage
        {
            public string Field;

            public int Property { get; set; }

            public void Method() { }

            private Garbage() { }
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
            MemberInfo memberInfo = typeof(Garbage).GetSingleConstructor(x => x.IsPrivate());

            //Act
            var result = memberInfo.IsMethod();

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenIsMethod_ReturnTrue()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Garbage).GetSingleMethod("Method");

            //Act
            var result = memberInfo.IsMethod();

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenIsField_ReturnFalse()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Garbage).GetSingleField("Field");

            //Act
            var result = memberInfo.IsMethod();

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenIsProperty_ReturnFalse()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Garbage).GetSingleProperty("Property");

            //Act
            var result = memberInfo.IsMethod();

            //Assert
            result.Should().BeFalse();
        }
    }

    [TestClass]
    public class IsField : Tester
    {
        public record Garbage
        {
            public string Field;

            public int Property { get; set; }

            public void Method() { }

            private Garbage() { }
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
            MemberInfo memberInfo = typeof(Garbage).GetSingleConstructor(x => x.IsPrivate());

            //Act
            var result = memberInfo.IsField();

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenIsMethod_ReturnFalse()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Garbage).GetSingleMethod("Method");

            //Act
            var result = memberInfo.IsField();

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenIsField_ReturnTrue()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Garbage).GetSingleField("Field");

            //Act
            var result = memberInfo.IsField();

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenIsProperty_ReturnFalse()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Garbage).GetSingleProperty("Property");

            //Act
            var result = memberInfo.IsField();

            //Assert
            result.Should().BeFalse();
        }
    }

    [TestClass]
    public class IsProperty : Tester
    {
        public record Garbage
        {
            public string Field;

            public int Property { get; set; }

            public void Method() { }

            private Garbage() { }
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
            MemberInfo memberInfo = typeof(Garbage).GetSingleConstructor(x => x.IsPrivate());

            //Act
            var result = memberInfo.IsProperty();

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenIsMethod_ReturnFalse()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Garbage).GetSingleMethod("Method");

            //Act
            var result = memberInfo.IsProperty();

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenIsField_ReturnFalse()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Garbage).GetSingleField("Field");

            //Act
            var result = memberInfo.IsProperty();

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenIsProperty_ReturnTrue()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Garbage).GetSingleProperty("Property");

            //Act
            var result = memberInfo.IsProperty();

            //Assert
            result.Should().BeTrue();
        }
    }

    [TestClass]
    public class GetMemberType : Tester
    {
        private class Garbage
        {
            public delegate void OnSomething();

            public int Id { get; set; }

            public string Field;

            public event OnSomething SomethingHappened;

            public char Method()
            {
                throw new NotImplementedException();
            }

        }

        [TestMethod]
        public void WhenMemberInfoIsNull_Throw()
        {
            //Arrange
            MemberInfo memberInfo = null!;

            //Act
            var action = () => memberInfo.GetMemberType();

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(memberInfo));
        }

        [TestMethod]
        public void WhenIsProperty_ReturnPropertyType()
        {
            //Arrange
            var memberInfo = typeof(Garbage).GetSingleMember(nameof(Garbage.Id));

            //Act
            var result = memberInfo.GetMemberType();

            //Assert
            result.Should().Be(typeof(int));
        }

        [TestMethod]
        public void WhenIsField_ReturnFieldType()
        {
            //Arrange
            var memberInfo = typeof(Garbage).GetSingleMember(nameof(Garbage.Field));

            //Act
            var result = memberInfo.GetMemberType();

            //Assert
            result.Should().Be(typeof(string));
        }

        [TestMethod]
        public void WhenIsMethod_ReturnMethodType()
        {
            //Arrange
            var memberInfo = typeof(Garbage).GetSingleMember(nameof(Garbage.Method));

            //Act
            var result = memberInfo.GetMemberType();

            //Assert
            result.Should().Be(typeof(char));
        }

        [TestMethod]
        public void WhenIsType_ReturnType()
        {
            //Arrange
            MemberInfo memberInfo = typeof(Garbage);

            //Act
            var result = memberInfo.GetMemberType();

            //Assert
            result.Should().Be(typeof(Garbage));
        }

        [TestMethod]
        public void WhenIsEvent_ReturnEventHandlerType()
        {
            //Arrange
            var memberInfo = typeof(Garbage).GetSingleMember(nameof(Garbage.SomethingHappened));

            //Act
            var result = memberInfo.GetMemberType();

            //Assert
            result.Should().Be(typeof(Garbage.OnSomething));
        }

        [TestMethod]
        public void WhenIsSomethingElse_Throw()
        {
            //Arrange
            var memberInfo = Dummy.Create<GarbageMemberInfo>();

            //Act
            var action = () => memberInfo.GetMemberType();

            //Assert
            action.Should().Throw<NotSupportedException>().WithMessage(string.Format(Exceptions.MemberInfoTypeNotSupported, memberInfo.GetType()));
        }
    }

    [TestClass]
    public class HasAttribute_Generic
    {
        //TODO Test
    }

    [TestClass]
    public class HasAttribute_NonGeneric
    {
        //TODO Test
    }
}