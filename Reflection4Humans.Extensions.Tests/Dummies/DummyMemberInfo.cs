using System.Globalization;

namespace Reflection4Humans.Extensions.Tests.Dummies;

public class DummyMemberInfo : MemberInfo
{
    public override Type? DeclaringType => typeof(DummyMemberInfo);
    public override MemberTypes MemberType => MemberTypes.Custom;
    public override string Name => nameof(DummyMemberInfo);
    public override Type? ReflectedType => typeof(object);

    public override object[] GetCustomAttributes(bool inherit) => Array.Empty<object>();

    public override object[] GetCustomAttributes(Type attributeType, bool inherit) => Array.Empty<object>();

    public override bool IsDefined(Type attributeType, bool inherit) => false;
}