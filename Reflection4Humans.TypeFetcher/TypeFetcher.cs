namespace ToolBX.Reflection4Humans.TypeFetcher;

[Obsolete("Use the 'Types' class instead. TypeFetcher will be removed in 3.0.0")]
public static class TypeFetcher
{
    [Obsolete("Use Types.Where() instead. TypeFetcher will be removed in 3.0.0")]
    public static ITypeQuery Query() => new TypeQuery();
}