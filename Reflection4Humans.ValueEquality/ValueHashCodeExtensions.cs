namespace ToolBX.Reflection4Humans.ValueEquality;

public static class ValueHashCodeExtensions
{
    /// <summary>
    /// Returns consistent value hash code.
    /// </summary>
    public static int GetValueHashCode<T>(this T? value, Depth depth = Depth.Recursive)
    {
        if (value is null) return 0;

        if (value is string str)
            return str.GetHashCode();

        if (value is IEnumerable enumerable)
        {
            unchecked
            {
                var hash = 17;

                foreach (var obj in enumerable)
                {
                    var hashcode = depth == Depth.Shallow ? obj?.GetHashCode() ?? 0 : obj?.GetValueHashCode(depth) ?? 0;
                    hash = hash * 31 + hashcode;
                }

                return hash;
            }
        }

        var derived = value.GetType();

        var fields = derived.GetAllFields(x => x.IsInstance() && x.IsPublic);
        var properties = derived.GetAllProperties(x => x.IsInstance() && x.IsPublic());

        unchecked
        {
            var hash = 17;

            foreach (var field in fields)
            {
                var fieldValue = field.GetValue(value);
                var fieldHash = depth == Depth.Shallow ? fieldValue?.GetHashCode() ?? 0 : fieldValue?.GetValueHashCode(depth) ?? 0;
                hash = hash * 31 + fieldHash;
            }

            foreach (var property in properties)
            {
                var propertyValue = property.GetValue(value);
                var propertyHash = depth == Depth.Shallow ? propertyValue?.GetHashCode() ?? 0 : propertyValue?.GetValueHashCode(depth) ?? 0;
                hash = hash * 31 + propertyHash;
            }

            return hash;
        }
    }
}