using System.Reflection;
using System.Xml.Linq;
using Microsoft.VisualBasic.CompilerServices;
using UnrealEngine.Gvas.Exceptions;

namespace UnrealEngine.Gvas.FProperties;

public abstract class FProperty
{
    private static readonly Dictionary<string, Type> PropertyTypes = new();
    public static readonly FProperty NoneProperty = new FNoneProperty();

    public string? Name { get; set; }
    public long FieldLength { get; set; }
    public Guid? Guid { get; set; }

    static FProperty()
    {
        var propertyTypes = typeof(FProperty).Assembly.GetTypes()
            .Where(t => t.IsAssignableTo(typeof(FProperty)))
            .Where(t => t != typeof(FProperty));
        foreach (var propertyType in propertyTypes)
            PropertyTypes.Add(propertyType.Name.Remove(0, 1), propertyType);
    }

    public static IEnumerable<FProperty> ReadFrom(BinaryReader reader, int count = -1)
    {
        var propertyName = reader.ReadFString();
        if (propertyName!.TrimStart('\0') == "None")
            yield return NoneProperty;

        var propertyTypeName = reader.ReadFString()!;
        var fieldLength = reader.ReadInt64();

        var propertyType = FindPropertyTypeByName(propertyTypeName)!;
        if (propertyType == null)
            throw new SaveGameException($"Unknown {nameof(FProperty)} type: '{propertyTypeName}'.");

        for (int i = 0; i < (count == -1 ? 1 : count); i++)
        {
            var propertyInstance = (FProperty) Activator.CreateInstance(propertyType)!;
            propertyInstance.Name = propertyName;
            propertyInstance.FieldLength = fieldLength;

            if (propertyType.GetCustomAttribute<OptionalGuidAttribute>() != null)
            {
                bool hasGuid = reader.ReadBoolean();
                if (hasGuid)
                    propertyInstance.Guid = new Guid(reader.ReadBytes(16));
            }

            propertyInstance.Read(reader, propertyName, fieldLength, count != -1 && i != 0);
            yield return propertyInstance;
        }
    }

    public void WriteTo(BinaryWriter writer, bool writeHeader = true)
    {
        if (writeHeader)
        {
            writer.WriteFString(Name);

            var typeName = GetType().Name.Remove(0, 1);
            writer.WriteFString(typeName);
            writer.Write(FieldLength);

            if (GetType().GetCustomAttribute<OptionalGuidAttribute>() != null)
            {
                writer.Write(Guid != null);
                if (Guid != null)
                    writer.Write(Guid.Value.ToByteArray());
            }
        }

        Write(writer, !writeHeader);
    }

    internal abstract void Read(BinaryReader reader, string? propertyName, long fieldLength, bool bodyOnly = false);

    internal virtual void Write(BinaryWriter writer, bool skipHeader)
    {
        throw new Exception(GetType().ToString(), new NotImplementedException());
    }

    public XElement SerializeProperty()
    {
        var element = new XElement(GetType().Name);
        element.SetAttributeValue("Name", Name);
        if (Guid != null)
            element.SetAttributeValue("Guid", Guid.Value.ToString("D"));
        foreach (var o in SerializeContent())
            element.Add(o);
        ModifyXmlNode(element);
        return element;
    }

    protected abstract IEnumerable<object> SerializeContent();

    protected virtual void ModifyXmlNode(XElement element)
    {
    }

    protected static Type? FindPropertyTypeByName(string? typeName, Type? defaultType = null)
    {
        if (typeName == null)
            return defaultType;

        if (PropertyTypes.ContainsKey(typeName))
            return PropertyTypes[typeName];

        return defaultType;
    }

    protected static FProperty InstantiateType(Type type) => (FProperty) Activator.CreateInstance(type)!;

    public virtual object? AsPrimitive() => null;

    public virtual void SetValue(object? val)
    {
        throw new NotImplementedException();
    }
}