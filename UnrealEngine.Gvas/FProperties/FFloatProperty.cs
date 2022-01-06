namespace UnrealEngine.Gvas.FProperties;

[OptionalGuid]
public class FFloatProperty : FProperty
{
    public float Value { get; set; }

    internal override void Read(BinaryReader reader, string? propertyName, long fieldLength, bool bodyOnly = false)
    {
        Value = reader.ReadSingle();
    }

    internal override void Write(BinaryWriter writer, bool skipHeader)
    {
        writer.Write(Value);
    }

    protected override IEnumerable<object> SerializeContent()
    {
        yield return Value;
    }
    
    public override object? AsPrimitive() => Value;
    
    public override void SetValue(object? val) => Value = (float) val;
}