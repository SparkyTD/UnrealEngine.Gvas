namespace UnrealEngine.Gvas.FProperties;

[OptionalGuid]
public class FStrProperty : FProperty
{
    public string? Value { get; set; }

    internal override void Read(BinaryReader reader, string? propertyName, long fieldLength, bool bodyOnly = false)
    {
        Value = reader.ReadFString();
    }

    internal override void Write(BinaryWriter writer, bool skipHeader)
    {
        writer.WriteFString(Value);
    }

    protected override IEnumerable<object> SerializeContent()
    {
        yield return Value ?? string.Empty;
    }

    public override object? AsPrimitive() => Value;

    public override void SetValue(object? val) => Value = (string?) val;
}