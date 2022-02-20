namespace UnrealEngine.Gvas.FProperties;

[OptionalGuid]
public class FObjectProperty : FProperty
{
    public string? ObjectName { get; set; }

    internal override void Read(BinaryReader reader, string? propertyName, long fieldLength, bool bodyOnly = false)
    {
        ObjectName = reader.ReadFString();
    }

    internal override void Write(BinaryWriter writer, bool skipHeader)
    {
        writer.WriteFString(ObjectName!);
    }

    protected override IEnumerable<object> SerializeContent()
    {
        yield return ObjectName ?? string.Empty;
    }

    public override object? AsPrimitive() => ObjectName;

    public override void SetValue(object? val) => ObjectName = (string?) val;
}