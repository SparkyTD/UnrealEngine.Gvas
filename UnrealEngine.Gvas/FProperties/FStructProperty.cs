using System.Xml.Linq;

namespace UnrealEngine.Gvas.FProperties;

public class FStructProperty : FProperty
{
    private Guid structureGuid = System.Guid.Empty;

    public string? TypeName { get; set; }
    public Dictionary<string, FProperty> Fields { get; } = new();

    internal override void Read(BinaryReader reader, string? propertyName, long fieldLength, bool bodyOnly = false)
    {
        if (!bodyOnly)
        {
            TypeName = reader.ReadFString();
            structureGuid = new Guid(reader.ReadBytes(16));
            reader.ReadBytes(1);
            if (fieldLength == 0)
              return;
        }

        if (TypeName == "Vector")
        {
            Fields.Add("X", new FFloatProperty {Name = "X", Value = reader.ReadSingle()});
            Fields.Add("Y", new FFloatProperty {Name = "Y", Value = reader.ReadSingle()});
            Fields.Add("Z", new FFloatProperty {Name = "Z", Value = reader.ReadSingle()});
        }
        else if (TypeName == "Rotator")
        {
            Fields.Add("Pitch", new FFloatProperty {Name = "Pitch", Value = reader.ReadSingle()});
            Fields.Add("Yaw", new FFloatProperty {Name = "Yaw", Value = reader.ReadSingle()});
            Fields.Add("Roll", new FFloatProperty {Name = "Roll", Value = reader.ReadSingle()});
        }
        else if (TypeName == "Quat")
        {
            Fields.Add("X", new FFloatProperty {Name = "X", Value = reader.ReadSingle()});
            Fields.Add("Y", new FFloatProperty {Name = "Y", Value = reader.ReadSingle()});
            Fields.Add("Z", new FFloatProperty {Name = "Z", Value = reader.ReadSingle()});
            Fields.Add("W", new FFloatProperty {Name = "W", Value = reader.ReadSingle()});
        }
        else if (TypeName == "DateTime")
            Fields.Add("Ticks", new FInt64Property {Name = "Ticks", Value = reader.ReadInt64()});
        else if (TypeName == "IntPoint")
        {
            Fields.Add("X", new FIntProperty {Name = "X", Value = reader.ReadInt32()});
            Fields.Add("Y", new FIntProperty {Name = "Y", Value = reader.ReadInt32()});
        }
        else if (TypeName == "Guid")
        {
          if (!bodyOnly)
          {
            Fields.Add("A", new FIntProperty {Name = "A", Value = reader.ReadInt32()});
            Fields.Add("B", new FIntProperty {Name = "B", Value = reader.ReadInt32()});
            Fields.Add("C", new FIntProperty {Name = "C", Value = reader.ReadInt32()});
            Fields.Add("D", new FIntProperty {Name = "D", Value = reader.ReadInt32()});
          }
        }
        else
        {
            FProperty? field;
            while ((field = ReadFrom(reader).First()) != NoneProperty)
                Fields.Add(field.Name!, field);
        }
    }

    internal override void Write(BinaryWriter writer, bool skipHeader)
    {
        if (!skipHeader)
        {
            writer.WriteFString(TypeName);
            writer.Write(structureGuid.ToByteArray());
            writer.Write((byte) 0);
        }

        if (TypeName == "Vector")
        {
            writer.Write((Fields["X"] as FFloatProperty)!.Value);
            writer.Write((Fields["Y"] as FFloatProperty)!.Value);
            writer.Write((Fields["Z"] as FFloatProperty)!.Value);
        }
        else if (TypeName == "Rotator")
        {
            writer.Write((Fields["Pitch"] as FFloatProperty)!.Value);
            writer.Write((Fields["Yaw"] as FFloatProperty)!.Value);
            writer.Write((Fields["Roll"] as FFloatProperty)!.Value);
        }
        else if (TypeName == "Quat")
        {
            writer.Write((Fields["X"] as FFloatProperty)!.Value);
            writer.Write((Fields["Y"] as FFloatProperty)!.Value);
            writer.Write((Fields["Z"] as FFloatProperty)!.Value);
            writer.Write((Fields["W"] as FFloatProperty)!.Value);
        }
        else if (TypeName == "DateTime")
            writer.Write((Fields["Ticks"] as FInt64Property)!.Value);
        else if (TypeName == "IntPoint")
        {
            writer.Write((Fields["X"] as FIntProperty)!.Value);
            writer.Write((Fields["Y"] as FIntProperty)!.Value);
        }
        else if (TypeName == "Guid")
        {
            writer.Write((Fields["A"] as FIntProperty)!.Value);
            writer.Write((Fields["B"] as FIntProperty)!.Value);
            writer.Write((Fields["C"] as FIntProperty)!.Value);
            writer.Write((Fields["D"] as FIntProperty)!.Value);
        }
        else
        {
            foreach (var (_, field) in Fields)
                field.WriteTo(writer);
            writer.WriteFString("None");
        }
    }

    protected override IEnumerable<object> SerializeContent()
    {
        foreach (var (_, field) in Fields)
            yield return field.SerializeProperty();
    }

    protected override void ModifyXmlNode(XElement element)
    {
        element.SetAttributeValue("Type", TypeName);
    }
}