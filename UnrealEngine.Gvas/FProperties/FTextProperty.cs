namespace UnrealEngine.Gvas.FProperties;

[OptionalGuid]
public class FTextProperty : FProperty
{
    public string? Value { get; set; }
    public int Unknown1 { get; set; }
    public byte Unknown2 { get; set; }
    public byte[]? Unknown3 { get; set; }
    
    internal override void Read(BinaryReader reader, string? propertyName, long fieldLength, bool bodyOnly = false)
    {
        long startPos = reader.BaseStream.Position;
        Unknown1 = reader.ReadInt32();
        Unknown2 = reader.ReadByte();
        if (Unknown1 != 0)
        {
            reader.ReadInt32();
            Value = reader.ReadFString();
            int bytesLeft = (int) (fieldLength - (reader.BaseStream.Position - startPos));
            Unknown3 = reader.ReadBytes(bytesLeft);
        }
    }

    protected override IEnumerable<object> SerializeContent()
    {
        throw new NotImplementedException();
    }
}