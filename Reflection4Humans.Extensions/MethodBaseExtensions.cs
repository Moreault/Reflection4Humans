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

    public static bool HasParameters(this MethodBase methodInfo, params Type[] parameters) => methodInfo.HasParameters((IEnumerable<Type>)parameters);
    public static bool HasParameters(this MethodBase methodInfo, IEnumerable<Type> parameters)
    {
        if (methodInfo is null) throw new ArgumentNullException(nameof(methodInfo));
        if (parameters is null) throw new ArgumentNullException(nameof(parameters));

        var parametersArray = parameters as IList<Type> ?? parameters.ToArray();
        if (parametersArray.Count == 0) return methodInfo.GetParameters().Length == 0;
        return methodInfo.GetParameters().Select(x => x.ParameterType).SequenceEqual(parametersArray);
    }

    public static bool HasParameters(this MethodBase methodInfo, int count)
    {
        if (methodInfo is null) throw new ArgumentNullException(nameof(methodInfo));
        return methodInfo.GetParameters().Length == count;
    }

    public static bool HasParameters(this MethodBase methodInfo, params Func<ParameterInfo, bool>[] predicates) => HasParameters(methodInfo, (IEnumerable<Func<ParameterInfo, bool>>)predicates);

    public static bool HasParameters(this MethodBase methodInfo, IEnumerable<Func<ParameterInfo, bool>> predicates)
    {
        if (methodInfo is null) throw new ArgumentNullException(nameof(methodInfo));
        if (predicates is null) throw new ArgumentNullException(nameof(predicates));

        var list = predicates as IList<Func<ParameterInfo, bool>> ?? predicates.ToList();

        var parameters = methodInfo.GetParameters();
        if (parameters.Length != list.Count) return false;

        for (var i = 0; i < parameters.Length; i++)
        {
            if (!list[i](parameters[i])) return false;
        }
        return true;
    }

    public static bool HasParametersAssignableFrom(this MethodBase methodInfo, params Type?[] parameters) => methodInfo.HasParametersAssignableFrom((IEnumerable<Type?>)parameters);

    public static bool HasParametersAssignableFrom(this MethodBase methodInfo, IEnumerable<Type?> parameters)
    {
        if (methodInfo is null) throw new ArgumentNullException(nameof(methodInfo));
        if (parameters is null) throw new ArgumentNullException(nameof(parameters));

        var list = parameters as IList<Type?> ?? parameters.ToList();
        if (list.Count != methodInfo.GetParameters().Length) return false;

        for (var i = 0; i < list.Count; i++)
        {
            if (list[i] != null && !list[i]!.IsAssignableFrom(methodInfo.GetParameters()[i].ParameterType)) return false;
        }

        return true;
    }

    public static bool HasParametersAssignableTo(this MethodBase methodInfo, params Type?[] parameters) => methodInfo.HasParametersAssignableTo((IEnumerable<Type?>)parameters);

    public static bool HasParametersAssignableTo(this MethodBase methodInfo, IEnumerable<Type?> parameters)
    {
        if (methodInfo is null) throw new ArgumentNullException(nameof(methodInfo));
        if (parameters is null) throw new ArgumentNullException(nameof(parameters));

        var list = parameters as IList<Type?> ?? parameters.ToList();
        if (list.Count != methodInfo.GetParameters().Length) return false;

        for (var i = 0; i < list.Count; i++)
        {
            if (list[i] != null && !list[i]!.IsAssignableTo(methodInfo.GetParameters()[i].ParameterType)) return false;
        }

        return true;
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

    public static Type GetMethodType(this MethodBase method)
    {
        if (method is null) throw new ArgumentNullException(nameof(method));
        if (method is ConstructorInfo)
            return method.DeclaringType!;

        return method is MethodInfo methodInfo ?
            methodInfo.ReturnType :
            throw new NotSupportedException($"MethodBase of type {method.GetType()} is not supported.");
    }
}