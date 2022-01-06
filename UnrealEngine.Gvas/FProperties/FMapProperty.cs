using System.Xml.Linq;
using UnrealEngine.Gvas.Exceptions;

namespace UnrealEngine.Gvas.FProperties;

public class FMapProperty : FProperty
{
    public Type? KeyType { get; private set; }
    public Type? ValueType { get; private set; }
    public Dictionary<FProperty, FProperty> KeyValuePairs { get; } = new();

    internal override void Read(BinaryReader reader, string? propertyName, long fieldLength, bool bodyOnly = false)
    {
        var keyTypeName = reader.ReadFString();
        var valueTypeName = reader.ReadFString();

        KeyType = FindPropertyTypeByName(keyTypeName);
        ValueType = FindPropertyTypeByName(valueTypeName);

        if (KeyType == null)
            throw new SaveGameException($"The {nameof(FMapProperty)}'s key type cannot be null.");
        if (ValueType == null)
            throw new SaveGameException($"The {nameof(FMapProperty)}'s key type cannot be null.");

        reader.ReadBytes(5);
        int elementCount = reader.ReadInt32();
        for (int i = 0; i < elementCount; i++)
        {
            var keyInstance = InstantiateType(KeyType);
            var valueInstance = InstantiateType(ValueType);

            // TODO handle if value is Struct

            keyInstance.Read(reader, null, 0);
            valueInstance.Read(reader, null, 0, true);

            KeyValuePairs.Add(keyInstance, valueInstance);
        }
    }

    internal override void Write(BinaryWriter writer, bool skipHeader)
    {
        writer.WriteFString(KeyType!.Name.Remove(0, 1));
        writer.WriteFString(ValueType!.Name.Remove(0, 1));

        writer.Write(new byte[5]);
        writer.Write(KeyValuePairs.Count);
        
        foreach (var (key, value) in KeyValuePairs)
        {
            key.Write(writer, true);
            value.Write(writer, true);
        }
    }

    protected override IEnumerable<object> SerializeContent()
    {
        foreach (var (key, value) in KeyValuePairs)
        {
            var element = new XElement("KeyValuePair");
            element.Add(new XElement("Key", key.SerializeProperty()));
            element.Add(new XElement("Value", value.SerializeProperty()));
            yield return element;
        }
    }

    protected override void ModifyXmlNode(XElement element)
    {
        element.SetAttributeValue("KeyType", KeyType?.Name);
        element.SetAttributeValue("ValueType", ValueType?.Name);
    }
}