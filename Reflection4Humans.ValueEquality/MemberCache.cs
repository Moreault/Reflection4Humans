using System.Collections.Concurrent;
using System.Reflection;

namespace ToolBX.Reflection4Humans.ValueEquality;

internal static class MemberCache
{
    private static readonly ConcurrentDictionary<Type, (IReadOnlyList<FieldInfo> Fields, IReadOnlyList<PropertyInfo> Properties)> Cache = new();

    public static (IReadOnlyList<FieldInfo> Fields, IReadOnlyList<PropertyInfo> Properties) GetPublicInstanceMembers(Type type)
    {
        return Cache.GetOrAdd(type, static t => (
            t.GetAllFields(x => x.IsInstance() && x.IsPublic),
            t.GetAllProperties(x => x.IsInstance() && x.IsPublic() && x.CanRead && !x.IsIndexer())
        ));
    }
}
