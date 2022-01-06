using System.Collections;
using System.Reflection;
using UnrealEngine.Gvas.FProperties;

namespace FNAFGameData.SaveData;

public class FNAFSaveStructure
{
    private FStructProperty? structure;

    protected void SetValue(string propertyName, object value, Type type)
    {
        Console.Out.WriteLine($"Setting '{propertyName}' to '{value}' as {value.GetType()}");
    }

    protected object? GetValue(string propertyName, Type type)
    {
        var property = structure!.Fields.ContainsKey(propertyName)
            ? structure.Fields[propertyName]
            : null;

        if (property == null)
            return DefaultValue(type);

        return ReadPropertyValue(property, type);
    }

    private object? ReadPropertyValue(FProperty property, Type type)
    {
        var primitiveValue = property.AsPrimitive();
        if ((type.IsPrimitive || type == typeof(string)) && primitiveValue != null)
            return primitiveValue;

        if (type.IsValueType && !type.IsPrimitive && !type.IsEnum)
        {
            if (type == typeof(DateTime))
            {
                long ticks = (long) (property as FStructProperty)!.Fields["Ticks"].AsPrimitive()!;
                return new DateTime(1, 1, 1).AddMilliseconds(ticks / 10000);
            }

            throw new NotImplementedException();
        }

        if (type.IsEnum && property is FEnumProperty enumProperty && Enum.TryParse(type, enumProperty.Value, out var enumValue))
            return enumValue;

        if (type.IsClass)
        {
            if (property is FStructProperty structProperty)
                return Wrap(type, structProperty);
            if (type.IsGenericType && type.GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>)))
            {
                var listInstance = (IList) Activator.CreateInstance(type)!;
                if (property is FArrayProperty arrayProperty)
                {
                    foreach (var element in arrayProperty.Elements)
                        listInstance.Add(ReadPropertyValue(element, type.GetGenericArguments()[0]));
                }
                else if (property is FSetProperty setProperty)
                {
                    foreach (var element in setProperty.Elements)
                        listInstance.Add(ReadPropertyValue(element, type.GetGenericArguments()[0]));
                }
                else
                    throw new NotImplementedException();

                return listInstance;
            }

            if (type.IsGenericType && type.GetGenericTypeDefinition().IsAssignableFrom(typeof(Dictionary<,>)))
            {
                var mapInstance = (IDictionary) Activator.CreateInstance(type)!;
                if (property is FMapProperty mapProperty)
                {
                    var keyType = type.GetGenericArguments()[0];
                    var valueType = type.GetGenericArguments()[1];
                    foreach (var (key, value) in mapProperty.KeyValuePairs)
                        mapInstance[ReadPropertyValue(key, keyType)!] = ReadPropertyValue(value, valueType);
                }
                else
                    throw new NotImplementedException();

                return mapInstance;
            }
        }

        return DefaultValue(type);
    }

    private static object? DefaultValue(Type type)
    {
        if (type.IsValueType)
            return Activator.CreateInstance(type);
        return null;
    }

    protected static object Wrap(Type type, FStructProperty dataSource)
    {
        var baseSetterMethod = typeof(FNAFSaveStructure).GetMethod(nameof(SetValue), BindingFlags.Instance | BindingFlags.NonPublic)!;
        var baseGetterMethod = typeof(FNAFSaveStructure).GetMethod(nameof(GetValue), BindingFlags.Instance | BindingFlags.NonPublic)!;
        var newType = MyTypeBuilder.CreateDynamicType(type, baseGetterMethod, baseSetterMethod);
        var saveStructure = Activator.CreateInstance(newType) as FNAFSaveStructure;
        saveStructure!.structure = dataSource;
        return saveStructure;
    }
}