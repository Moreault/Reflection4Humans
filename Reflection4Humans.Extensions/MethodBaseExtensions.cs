namespace ToolBX.Reflection4Humans.Extensions;

public static class MethodBaseExtensions
{
    public static bool HasParameters<T>(this MethodBase methodInfo) => methodInfo.HasParameters(typeof(T));
    public static bool HasParameters<T1, T2>(this MethodBase methodInfo) => methodInfo.HasParameters(typeof(T1), typeof(T2));
    public static bool HasParameters<T1, T2, T3>(this MethodBase methodInfo) => methodInfo.HasParameters(typeof(T1), typeof(T2), typeof(T3));
    public static bool HasParameters<T1, T2, T3, T4>(this MethodBase methodInfo) => methodInfo.HasParameters(typeof(T1), typeof(T2), typeof(T3), typeof(T4));
    public static bool HasParameters<T1, T2, T3, T4, T5>(this MethodBase methodInfo) => methodInfo.HasParameters(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5));
    public static bool HasParameters<T1, T2, T3, T4, T5, T6>(this MethodBase methodInfo) => methodInfo.HasParameters(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6));
    public static bool HasParameters<T1, T2, T3, T4, T5, T6, T7>(this MethodBase methodInfo) => methodInfo.HasParameters(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7));
    public static bool HasParameters<T1, T2, T3, T4, T5, T6, T7, T8>(this MethodBase methodInfo) => methodInfo.HasParameters(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8));
    public static bool HasParameters<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this MethodBase methodInfo) => methodInfo.HasParameters(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9));
    public static bool HasParameters<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this MethodBase methodInfo) => methodInfo.HasParameters(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10));

    public static bool HasParameters(this MethodBase methodInfo, params Type[] parameters)
    {
        if (methodInfo is null) throw new ArgumentNullException(nameof(methodInfo));
        if (parameters is null) throw new ArgumentNullException(nameof(parameters));
        if (parameters.Length == 0) return methodInfo.GetParameters().Length == 0;
        return methodInfo.GetParameters().Select(x => x.ParameterType).SequenceEqual(parameters);
    }

    public static bool HasParameters(this MethodBase methodInfo, int count)
    {
        if (methodInfo is null) throw new ArgumentNullException(nameof(methodInfo));
        return methodInfo.GetParameters().Length == count;
    }

    public static bool HasNoParameter(this MethodBase methodInfo)
    {
        if (methodInfo is null) throw new ArgumentNullException(nameof(methodInfo));
        return methodInfo.GetParameters().Length == 0;
    }

    public static bool IsPropertyAccessor(this MethodBase method)
    {
        if (method is null) throw new ArgumentNullException(nameof(method));
        if (method.IsSpecialName && method.IsPublic && method.DeclaringType != null)
        {
            var property = method.DeclaringType.GetProperty(method.Name[4..]);
            if (property != null)
            {
                var accessors = property.GetAccessors();
                return accessors.Contains(method);
            }
        }

        return false;
    }
}