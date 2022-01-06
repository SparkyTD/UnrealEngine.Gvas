using System.Text;
using System.Xml.Linq;

namespace UnrealEngine.Gvas.FProperties;

public class FByteProperty : FProperty
{
    public string? EnumName { get; set; }
    public byte[]? Payload { get; set; }

    public string? TextPayload { get; set; }

    internal override void Read(BinaryReader reader, string? propertyName, long fieldLength, bool bodyOnly = false)
    {
        EnumName = reader.ReadFString();
        if (!bodyOnly)
            reader.ReadByte(); // 0x00 separator
        Payload = reader.ReadBytes((int) fieldLength);

        if (Payload.Length > 4 && BitConverter.ToInt32(Payload, 0) == Payload.Length - 4)
            TextPayload = Encoding.ASCII.GetString(Payload.Skip(4).ToArray());
    }

    internal override void Write(BinaryWriter writer, bool skipHeader)
    {
        writer.WriteFString(EnumName);
        if (!skipHeader)
            writer.Write((byte) 0);
        writer.Write(Payload!);
    }

    protected override IEnumerable<object> SerializeContent()
    {
        if (TextPayload != null)
            yield return TextPayload.TrimEnd('\0');
        else
            yield return string.Join(null, Payload!.Select(b => $"{b:X2}"));
    }

    protected override void ModifyXmlNode(XElement element)
    {
        element.SetAttributeValue("EnumType", EnumName);
    }
}