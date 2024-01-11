namespace ToolBX.Reflection4Humans.TypeFetcher;

[Obsolete("Use the 'Types' class instead. ITypeQuery will be removed in 3.0.0")]
public interface ITypeQuery
{
    ITypeQuery IsClass();
    ITypeQuery IsNotClass();
    ITypeQuery IsAbstract();
    ITypeQuery IsNotAbstract();
    ITypeQuery IsInterface();
    ITypeQuery IsNotInterface();
    ITypeQuery IsGeneric();
    ITypeQuery IsNotGeneric();
    ITypeQuery IsGenericTypeDefinition();
    ITypeQuery IsNotGenericTypeDefinition();
    ITypeQuery IsAttribute();
    ITypeQuery IsNotAttribute();
    ITypeQuery IsStruct();
    ITypeQuery IsNotStruct();
    ITypeQuery IsEnum();
    ITypeQuery IsNotEnum();
    ITypeQuery HasAttribute(Type attribute);
    ITypeQuery HasAttributes(params Type[] attributes);
    ITypeQuery Implements(params Type[] interfaces);
    IReadOnlyList<Type> ToList();
    Type Single();
    Type? SingleOrDefault();
}

internal record TypeQuery : ITypeQuery
{
    private bool? _isClass;
    private bool? _isAbstract;
    private bool? _isInterface;
    private bool? _isGenericType;
    private bool? _isGenericTypeDefinition;
    private bool? _isAttribute;
    private bool? _isStruct;
    private bool? _isEnum;

    private IReadOnlyList<Type> Attributes { get; init; } = new List<Type>();
    private IReadOnlyList<Type> Interfaces { get; init; } = new List<Type>();

    public ITypeQuery IsClass() => this with { _isClass = true };

    public ITypeQuery IsNotClass() => this with { _isClass = false };

    public ITypeQuery IsAbstract() => this with { _isAbstract = true };

    public ITypeQuery IsNotAbstract() => this with { _isAbstract = false };

    public ITypeQuery IsInterface() => this with { _isInterface = true };

    public ITypeQuery IsNotInterface() => this with { _isInterface = false };

    public ITypeQuery IsGeneric() => this with { _isGenericType = true };

    public ITypeQuery IsNotGeneric() => this with { _isGenericType = false };

    public ITypeQuery IsGenericTypeDefinition() => this with { _isGenericTypeDefinition = true };

    public ITypeQuery IsNotGenericTypeDefinition() => this with { _isGenericTypeDefinition = false };

    public ITypeQuery IsAttribute() => this with { _isAttribute = true };

    public ITypeQuery IsNotAttribute() => this with { _isAttribute = false };

    public ITypeQuery IsStruct() => this with { _isStruct = true };

    public ITypeQuery IsNotStruct() => this with { _isStruct = false };

    public ITypeQuery IsEnum() => this with { _isEnum = true };

    public ITypeQuery IsNotEnum() => this with { _isEnum = false };

    public ITypeQuery HasAttribute(Type attribute) => HasAttributes(attribute);

    public ITypeQuery HasAttributes(params Type[] attributes)
    {
        if (attributes == null || !attributes.Any()) throw new ArgumentNullException(nameof(attributes));

        return this with
        {
            Attributes = Attributes.Concat(attributes).ToList()
        };
    }

    public ITypeQuery Implements(params Type[] interfaces)
    {
        if (interfaces == null || !interfaces.Any()) throw new ArgumentNullException(nameof(interfaces));

        return this with
        {
            Interfaces = Interfaces.Concat(interfaces).ToList()
        };
    }

    internal TypeQuery()
    {

    }

    public IReadOnlyList<Type> ToList() => Query().ToList();

    public Type Single() => Query().Single();

    public Type? SingleOrDefault() => Query().SingleOrDefault();

    private IEnumerable<Type> Query()
    {
        AssemblyLoader.EnsureAllLoaded(true);
        return AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
            .Where(x => (!_isClass.HasValue || x.IsClass == _isClass.Value) &&
                        (!_isAbstract.HasValue || x.IsAbstract == _isAbstract.Value) &&
                        (!_isInterface.HasValue || x.IsInterface == _isInterface.Value) &&
                        (!_isStruct.HasValue || x.IsValueType == _isStruct.Value) &&
                        (!_isEnum.HasValue || x.IsEnum == _isEnum.Value) &&
                        (!_isGenericType.HasValue || x.IsGenericType == _isGenericType.Value) &&
                        (!_isGenericTypeDefinition.HasValue || x.IsGenericTypeDefinition == _isGenericTypeDefinition.Value) &&
                        (!_isAttribute.HasValue || x.IsAttribute() == _isAttribute.Value) &&
                        (!Attributes.Any() || x.GetCustomAttributes(true).Any(y => Attributes.Contains(y.GetType()))) &&
                        (!Interfaces.Any() || x.GetInterfaces().Any(y => Interfaces.Contains(y)))).DistinctBy(x => x.FullName);
    }

    public virtual bool Equals(TypeQuery? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return _isAbstract == other._isAbstract &&
               _isInterface == other._isInterface &&
               _isGenericType == other._isGenericType &&
               _isAttribute == other._isAttribute &&
               _isClass == other._isClass &&
               _isStruct == other._isStruct &&
               _isEnum == other._isEnum &&
               _isGenericTypeDefinition == other._isGenericTypeDefinition &&
               Attributes.SequenceEqual(other.Attributes) &&
               Interfaces.SequenceEqual(other.Interfaces);
    }

    public override int GetHashCode()
    {
        var hashCode = new HashCode();
        hashCode.Add(_isClass);
        hashCode.Add(_isAbstract);
        hashCode.Add(_isInterface);
        hashCode.Add(_isGenericType);
        hashCode.Add(_isGenericTypeDefinition);
        hashCode.Add(_isAttribute);
        hashCode.Add(_isStruct);
        hashCode.Add(_isEnum);
        hashCode.Add(Attributes);
        hashCode.Add(Interfaces);
        return hashCode.ToHashCode();
    }
}