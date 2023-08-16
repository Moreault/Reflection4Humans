namespace ToolBX.Reflection4Humans.Extensions;

public static class EventSearchExtensions
{
    public static IReadOnlyList<EventInfo> GetAllEvents(this Type type, Func<EventInfo, bool>? predicate = null) => type.GetAllEventsInternal(predicate).ToList();

    private static IEnumerable<EventInfo> GetAllEventsInternal(this Type type, Func<EventInfo, bool>? predicate = null)
    {
        if (type is null) throw new ArgumentNullException(nameof(type));

        IEnumerable<EventInfo> events = Array.Empty<EventInfo>();

        var currentType = type;
        do
        {
            events = events.Concat(currentType.GetEvents(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance));
            currentType = currentType.BaseType;
        } while (currentType != null);

        events = events.Distinct(new MemberInfoEqualityComparer<EventInfo>());
        return predicate is null ? events : events.Where(predicate);
    }

    public static EventInfo GetSingleEvent(this Type type, string name, StringComparison stringComparison = StringComparison.Ordinal)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
        return type.GetAllEventsInternal(x => x.Name.Equals(name, stringComparison)).Single();
    }

    public static EventInfo? GetSingleEventOrDefault(this Type type, string name, StringComparison stringComparison = StringComparison.Ordinal)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
        return type.GetAllEventsInternal(x => x.Name.Equals(name, stringComparison)).SingleOrDefault();
    }

    public static EventInfo GetSingleEvent(this Type type, Func<EventInfo, bool>? predicate = null) => type.GetAllEventsInternal(predicate).Single();

    public static EventInfo? GetSingleEventOrDefault(this Type type, Func<EventInfo, bool>? predicate = null) => type.GetAllEventsInternal(predicate).SingleOrDefault();
}