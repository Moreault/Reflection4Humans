namespace ToolBX.Reflection4Humans.Extensions;

/// <summary>
/// Wrapper over properties and fields.
/// </summary>
public interface IPropertyOrField : ICustomAttributeProvider, IEquatable<IPropertyOrField>, IEquatable<PropertyInfo>, IEquatable<FieldInfo>, IEquatable<MemberInfo>
{
    bool CanRead { get; }
    bool CanWrite { get; }
    bool CanReadAndWrite { get; }

    MemberTypes MemberType { get; }
    string Name { get; }
    Type? DeclaringType { get; }
    Type? ReflectedType { get; }
    Module Module { get; }

    bool HasSameMetadataDefinitionAs(MemberInfo other);
    IEnumerable<CustomAttributeData> CustomAttributes { get; }
    IList<CustomAttributeData> GetCustomAttributesData();
    bool IsCollectible { get; }
    int MetadataToken { get; }

    bool IsStatic { get; }
    bool IsInstance { get; }
    bool IsPrivate { get; }
    bool IsPublic { get; }
    bool IsProtected { get; }
    bool IsInternal { get; }
    bool IsProperty { get; }
    bool IsField { get; }

    bool IsAutomaticBackingField { get; }

    /// <summary>
    /// True if a field name has the same name as a property (ignoring casing) and follows your naming conventions. Also validates whether it's an automatic backing field.
    /// </summary>
    bool IsBackingField(params BackingFieldConvention[] conventions);

    /// <summary>
    /// Attempts to return the value of member. Returns failure in case of write-only properties.
    /// </summary>
    Result<object?> TryGetValue(object? instance);
    object? GetValue(object? instance);

    /// <summary>
    /// Attempts to set the value of member. Does not throw in case of read-only properties.
    /// </summary>
    void TrySetValue(object? instance, object? value);

    void SetValue(object? instance, object? value);

    /// <summary>
    /// Returns a <see cref="PropertyInfo"/> if the instance is a property.
    /// </summary>
    Result<PropertyInfo> TryAsProperty();

    /// <summary>
    /// Returns a <see cref="PropertyInfo"/> if the instance is a property.
    /// </summary>
    /// <exception cref="InvalidCastException"></exception>
    PropertyInfo AsProperty();

    /// <summary>
    /// Returns a <see cref="FieldInfo"/> if the instance is a field.
    /// </summary>
    Result<FieldInfo> TryAsField();

    /// <summary>
    /// Returns a <see cref="FieldInfo"/> if the instance is a field.
    /// </summary>
    /// <exception cref="InvalidCastException"></exception>
    FieldInfo AsField();
}

internal sealed class PropertyOrField : MemberInfo, IPropertyOrField
{
    private readonly MemberInfo _unwrapped;

    public override Type? DeclaringType => _unwrapped.DeclaringType;
    public bool CanRead { get; }
    public bool CanWrite { get; }
    public bool CanReadAndWrite => CanRead && CanWrite;
    public override MemberTypes MemberType => _unwrapped.MemberType;
    public override string Name => _unwrapped.Name;
    public override Type? ReflectedType => _unwrapped.ReflectedType;

    public override bool IsCollectible => _unwrapped.IsCollectible;

    public override IEnumerable<CustomAttributeData> CustomAttributes => _unwrapped.CustomAttributes;

    public override Module Module => _unwrapped.Module;

    public override int MetadataToken => _unwrapped.MetadataToken;

    public bool IsStatic { get; }
    public bool IsInstance => !IsStatic;
    public bool IsPrivate { get; }
    public bool IsPublic { get; }
    public bool IsProtected { get; }
    public bool IsInternal { get; }
    public bool IsProperty { get; }
    public bool IsField { get; }
    public bool IsAutomaticBackingField { get; }

    public Result<object?> TryGetValue(object? instance)
    {
        try
        {
            return Result<object?>.Success(GetValue(instance));
        }
        catch
        {
            return Result<object?>.Failure();
        }
    }

    public object? GetValue(object? instance) => _getValue.Invoke(instance);
    private readonly Func<object?, object?> _getValue;

    public void TrySetValue(object? instance, object? value)
    {
        try
        {
            SetValue(instance, value);
        }
        catch
        {
            // ignore
        }
    }

    public void SetValue(object? instance, object? value) => _setValue.Invoke(instance, value);
    private readonly Action<object?, object?> _setValue;

    internal PropertyOrField(MemberInfo unwrapped)
    {
        if (unwrapped is PropertyInfo property)
        {
            CanRead = property.CanRead;
            CanWrite = property.CanWrite;
            _getValue = x =>
            {
                if (!CanRead) throw new InvalidOperationException(string.Format(Exceptions.UsingGetOnWriteOnlyProperty, unwrapped.Name));
                return property.GetValue(x);
            };
            _setValue = (instance, value) =>
            {
                if (!CanWrite) throw new InvalidOperationException(string.Format(Exceptions.UsingSetOnReadOnlyProperty, unwrapped.Name));
                property.SetValue(instance, value);
            };
            IsStatic = property.IsStatic();
            IsPrivate = property.IsPrivate();
            IsPublic = property.IsPublic();
            IsProtected = property.IsProtected();
            IsInternal = property.IsInternal();
            IsProperty = true;
            IsAutomaticBackingField = false;
        }
        else if (unwrapped is FieldInfo field)
        {
            CanRead = true;
            CanWrite = true;
            _getValue = x => field.GetValue(x);
            _setValue = (instance, value) => field.SetValue(instance, value);
            IsStatic = field.IsStatic;
            IsPrivate = field.IsPrivate;
            IsPublic = field.IsPublic;
            IsProtected = field.IsFamily;
            IsInternal = field.IsAssembly;
            IsField = true;
            IsAutomaticBackingField = field.IsAutomaticBackingField();
        }
        else
            throw new InvalidOperationException(string.Format(Exceptions.MemberIsNeitherPropertyNorField, unwrapped.Name));

        _unwrapped = unwrapped ?? throw new ArgumentNullException(nameof(unwrapped));
    }

    public override IList<CustomAttributeData> GetCustomAttributesData() => _unwrapped.GetCustomAttributesData();

    public bool IsBackingField(params BackingFieldConvention[] conventions)
    {
        if (IsProperty)
            return false;
        return ((FieldInfo)_unwrapped).IsBackingField(conventions);
    }

    public override object[] GetCustomAttributes(bool inherit) => _unwrapped.GetCustomAttributes(inherit);

    public override object[] GetCustomAttributes(Type attributeType, bool inherit) => _unwrapped.GetCustomAttributes(attributeType, inherit);

    public override bool IsDefined(Type attributeType, bool inherit) => _unwrapped.IsDefined(attributeType, inherit);

    public Result<PropertyInfo> TryAsProperty()
    {
        return _unwrapped is not PropertyInfo value ? Result<PropertyInfo>.Failure() : Result<PropertyInfo>.Success(value);
    }

    public PropertyInfo AsProperty() => _unwrapped as PropertyInfo ?? throw new InvalidCastException(string.Format(Exceptions.MemberCannotBeCastToProperty, Name));

    public Result<FieldInfo> TryAsField()
    {
        return _unwrapped is not FieldInfo value ? Result<FieldInfo>.Failure() : Result<FieldInfo>.Success(value);
    }

    public FieldInfo AsField() => _unwrapped as FieldInfo ?? throw new InvalidCastException(string.Format(Exceptions.MemberCannotBeCastToField, Name));

    public bool Equals(IPropertyOrField? other)
    {
        var concrete = other as PropertyOrField;
        if (concrete is null) return false;
        return _unwrapped.Equals(concrete._unwrapped);
    }

    public bool Equals(PropertyInfo? other) => _unwrapped.Equals(other);

    public bool Equals(FieldInfo? other) => _unwrapped.Equals(other);

    public bool Equals(MemberInfo? other) => _unwrapped.Equals(other);

    public override bool Equals(object? obj)
    {
        if (obj is IPropertyOrField propertyOrField) return Equals(propertyOrField);
        if (obj is PropertyInfo propertyInfo) return Equals(propertyInfo);
        if (obj is FieldInfo fieldInfo) return Equals(fieldInfo);
        if (obj is MemberInfo memberInfo) return Equals(memberInfo);
        return false;
    }

    public override int GetHashCode() => _unwrapped.GetHashCode();

    public override string? ToString() => _unwrapped.ToString();

    public static bool operator ==(PropertyOrField? a, IPropertyOrField? b)
    {
        if (a is not null) return a.Equals(b);
        return b is null;
    }

    public static bool operator !=(PropertyOrField? a, IPropertyOrField? b)
    {
        return !(a == b);
    }

    public static bool operator ==(PropertyOrField? a, MemberInfo? b)
    {
        if (a is not null) return a.Equals(b);
        return b is null;
    }

    public static bool operator !=(PropertyOrField? a, MemberInfo? b) => !(a == b);

    public static bool operator ==(PropertyOrField? a, PropertyInfo? b)
    {
        if (a is not null) return a.Equals(b);
        return b is null;
    }

    public static bool operator !=(PropertyOrField? a, PropertyInfo? b) => !(a == b);

    public static bool operator ==(PropertyOrField? a, FieldInfo? b)
    {
        if (a is not null) return a.Equals(b);
        return b is null;
    }

    public static bool operator !=(PropertyOrField? a, FieldInfo? b) => !(a == b);
}