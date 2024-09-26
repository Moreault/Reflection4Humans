namespace Reflection4Humans.Extensions.Tests.Customizations;

[AutoCustomization]
public sealed class PropertyOrFieldCustomization : CustomizationBase<IPropertyOrField>
{
    protected override IEnumerable<Type> AdditionalTypes => [typeof(PropertyOrField)];

    private string Property { get; set; }
    private string _field;

    public override IDummyBuilder<IPropertyOrField> Build(IDummy dummy) => dummy.Build<IPropertyOrField>().FromFactory(() => Coin.Flip()
        ? typeof(PropertyOrFieldCustomization).GetSinglePropertyOrField(nameof(Property))
        : typeof(PropertyOrFieldCustomization).GetSinglePropertyOrField(nameof(_field)));
}