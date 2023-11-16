using System.Reflection.Emit;
using System.Runtime.CompilerServices;

namespace ToolBX.Reflection4Humans.TypeGenerator;

public static class TypeGenerator
{
    public static Type From<T>(TypeGenerationOptions? options = null) => From(typeof(T), options);

    public static Type From(Type type, TypeGenerationOptions? options = null)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));
        if (type.IsStatic()) throw new ArgumentException(string.Format(Exceptions.CannotGenerateFromStaticType, type.Name), nameof(type));
        if (type.IsSealed) throw new ArgumentException(string.Format(Exceptions.CannotGenerateFromSealedType, type.Name), nameof(type));

        options ??= new TypeGenerationOptions();

        var assemblyName = new AssemblyName(options.AssemblyName);
        var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
        var moduleBuilder = assemblyBuilder.DefineDynamicModule(options.ModuleName);

        var typeBuilder = moduleBuilder.DefineType(options.TypeName, TypeAttributes.Public | TypeAttributes.Class, type.IsInterface ? null : type, type.IsInterface ? new[] { type } : null);

        foreach (var member in type.GetAllMembers(x => x.IsPublic() && x.IsInstance()))
        {
            if (member is MethodInfo method)
            {
                if (!method.IsVirtual || method.IsPropertyAccessor())
                    continue;

                var parameters = method.GetParameters();

                var methodBuilder = typeBuilder.DefineMethod(member.Name, MethodAttributes.Public | MethodAttributes.Virtual, method.ReturnType, parameters.Any() ? parameters.Select(x => x.ParameterType).ToArray() : Type.EmptyTypes);
                var ilGenerator = methodBuilder.GetILGenerator();
                ilGenerator.Emit(OpCodes.Ret);
            }
            else if (member is PropertyInfo property)
            {
                var propertyBuilder = typeBuilder.DefineProperty(property.Name, PropertyAttributes.None, property.PropertyType, Type.EmptyTypes);
                var backingField = typeBuilder.DefineField($"<{property.Name}>k__BackingField", property.PropertyType, FieldAttributes.Private);

                if (property.IsGet())
                {
                    var propertyGetter = property.IsIndexer() ?
                        typeBuilder.DefineMethod("get_Item", MethodAttributes.Public | MethodAttributes.Virtual, property.PropertyType, property.GetIndexParameters().Select(x => x.ParameterType).ToArray()) :
                        typeBuilder.DefineMethod($"get_{property.Name}", MethodAttributes.Public | MethodAttributes.Virtual, property.PropertyType, Type.EmptyTypes);
                    var ilGenerator = propertyGetter.GetILGenerator();
                    ilGenerator.Emit(OpCodes.Ldarg_0);
                    ilGenerator.Emit(OpCodes.Ldfld, backingField);
                    ilGenerator.Emit(OpCodes.Ret);
                    propertyBuilder.SetGetMethod(propertyGetter);
                }

                if (property.IsSet())
                {
                    var propertySetter = property.IsIndexer() ?
                        typeBuilder.DefineMethod("set_Item", MethodAttributes.Public | MethodAttributes.Virtual, typeof(void), property.GetIndexParameters().Select(x => x.ParameterType).Concat(new[] { property.PropertyType }).ToArray()) :
                        typeBuilder.DefineMethod($"set_{property.Name}", MethodAttributes.Public | MethodAttributes.Virtual, typeof(void), new[] { property.PropertyType });
                    
                    var ilGenerator = propertySetter.GetILGenerator();
                    ilGenerator.Emit(OpCodes.Ldarg_0);
                    ilGenerator.Emit(OpCodes.Ldarg_1);
                    ilGenerator.Emit(OpCodes.Stfld, backingField);
                    ilGenerator.Emit(OpCodes.Ret);
                    propertyBuilder.SetSetMethod(propertySetter);
                }
            }
            else if (member is EventInfo eventInfo)
            {
                var eventBuilder = typeBuilder.DefineEvent(eventInfo.Name, EventAttributes.None, eventInfo.EventHandlerType!);

                if (eventInfo.GetAddMethod() != null)
                {
                    var addMethod = typeBuilder.DefineMethod($"add_{eventInfo.Name}", MethodAttributes.Public | MethodAttributes.Virtual, typeof(void), new[] { eventInfo.EventHandlerType! });
                    var ilGenerator = addMethod.GetILGenerator();
                    ilGenerator.Emit(OpCodes.Ret);
                    eventBuilder.SetAddOnMethod(addMethod);
                }

                if (eventInfo.GetRemoveMethod() != null)
                {
                    var removeMethod = typeBuilder.DefineMethod($"remove_{eventInfo.Name}", MethodAttributes.Public | MethodAttributes.Virtual, typeof(void), new[] { eventInfo.EventHandlerType! });
                    var ilGenerator = removeMethod.GetILGenerator();
                    ilGenerator.Emit(OpCodes.Ret);
                    eventBuilder.SetRemoveOnMethod(removeMethod);
                }
            }
            else if (member is ConstructorInfo constructorInfo)
            {
                var constructorBuilder = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, Type.EmptyTypes);
                var ilGenerator = constructorBuilder.GetILGenerator();
                ilGenerator.Emit(OpCodes.Ldarg_0);
                ilGenerator.Emit(OpCodes.Call, constructorInfo);
                ilGenerator.Emit(OpCodes.Ret);
            }
            else if (member is FieldInfo)
                continue;
            else throw new NotSupportedException(string.Format(Exceptions.MemberTypeNotSupported, member.GetType().Name));
        }

        return typeBuilder.CreateType();
    }
}