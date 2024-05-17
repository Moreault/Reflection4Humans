namespace Reflection4Humans.Extensions.Tests.Garbage;

public class GarbageMemberInfo : MemberInfo
{
    public override Type? DeclaringType => typeof(GarbageMemberInfo);
    public override MemberTypes MemberType => MemberTypes.Custom;
    public override string Name => nameof(GarbageMemberInfo);
    public override Type? ReflectedType => typeof(object);

    public override object[] GetCustomAttributes(bool inherit) => Array.Empty<object>();

    public override object[] GetCustomAttributes(Type attributeType, bool inherit) => Array.Empty<object>();

    public override bool IsDefined(Type attributeType, bool inherit) => false;
}