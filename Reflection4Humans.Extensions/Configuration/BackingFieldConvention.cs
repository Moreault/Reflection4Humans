namespace ToolBX.Reflection4Humans.Extensions.Configuration;

public sealed record BackingFieldConvention(string Prefix, string Suffix)
{
    public static BackingFieldConvention Csharp = new("_", string.Empty);

    public BackingFieldConvention() : this(string.Empty, string.Empty) { }
}