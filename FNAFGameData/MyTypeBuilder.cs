using System.Reflection;
using System.Reflection.Emit;

namespace FNAFGameData;

public static class MyTypeBuilder
{
    public static Type CreateDynamicType(Type baseType, MethodInfo baseGetterMethod, MethodInfo baseSetterMethod)
    {
        var builder = GetTypeBuilder(baseType.Name + "Wrapper");
        builder.SetParent(baseType);

        foreach (var propertyInfo in baseType.GetProperties())
        {
            var property = builder.DefineProperty(propertyInfo.Name, PropertyAttributes.None, propertyInfo.PropertyType, null);

            var setMethodBuilder = builder.DefineMethod(
                "set_" + propertyInfo.Name,
                MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig | MethodAttributes.Virtual,
                null, new[] {propertyInfo.PropertyType});
            var setMethodGenerator = setMethodBuilder.GetILGenerator();
            setMethodGenerator.Emit(OpCodes.Ldarg_0);
            setMethodGenerator.Emit(OpCodes.Ldstr, propertyInfo.Name);
            setMethodGenerator.Emit(OpCodes.Ldarg_1);
            setMethodGenerator.Emit(OpCodes.Box, propertyInfo.PropertyType);
            setMethodGenerator.Emit(OpCodes.Ldtoken, propertyInfo.PropertyType);
            setMethodGenerator.EmitCall(OpCodes.Call, typeof(Type).GetMethod(nameof(Type.GetTypeFromHandle))!, Type.EmptyTypes);
            setMethodGenerator.EmitCall(OpCodes.Call, baseSetterMethod, Type.EmptyTypes);
            setMethodGenerator.Emit(OpCodes.Ret);

            var getMethodBuilder = builder.DefineMethod(
                "get_" + propertyInfo.Name,
                MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig | MethodAttributes.Virtual,
                propertyInfo.PropertyType, Type.EmptyTypes);
            var getMethodGenerator = getMethodBuilder.GetILGenerator();
            getMethodGenerator.Emit(OpCodes.Ldarg_0);
            getMethodGenerator.Emit(OpCodes.Ldstr, propertyInfo.Name);

            getMethodGenerator.Emit(OpCodes.Ldtoken, propertyInfo.PropertyType);
            getMethodGenerator.EmitCall(OpCodes.Call, typeof(Type).GetMethod(nameof(Type.GetTypeFromHandle))!, Type.EmptyTypes);

            getMethodGenerator.EmitCall(OpCodes.Call, baseGetterMethod, Type.EmptyTypes);
            getMethodGenerator.Emit(propertyInfo.PropertyType.IsValueType ? OpCodes.Unbox_Any : OpCodes.Castclass, propertyInfo.PropertyType);
            getMethodGenerator.Emit(OpCodes.Ret);

            property.SetSetMethod(setMethodBuilder);
            property.SetGetMethod(getMethodBuilder);
        }


        return builder.CreateType()!;
    }

    public static TypeBuilder GetTypeBuilder(string typeName)
    {
        var an = new AssemblyName(typeName);
        var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(an, AssemblyBuilderAccess.Run);
        var moduleBuilder = assemblyBuilder.DefineDynamicModule("MainModule");
        var tb = moduleBuilder.DefineType(typeName,
            TypeAttributes.Public |
            TypeAttributes.Class |
            TypeAttributes.AutoClass |
            TypeAttributes.AnsiClass |
            TypeAttributes.BeforeFieldInit |
            TypeAttributes.AutoLayout,
            null);
        return tb;
    }
}