using System.Text;
using UnrealEngine.Gvas.Exceptions;

namespace UnrealEngine.Gvas;

public class SaveGameHeader
{
    private const string FileTypeTag = "GVAS";

    public int SaveGameFileVersion { get; set; }
    public int PackageFileUE4Version { get; set; }
    public EngineVersion? SavedEngineVersion { get; set; }
    public CustomVersionSerializationFormat CustomVersionFormat { get; set; }
    public List<CustomVersion> CustomVersions { get; set; } = new();
    public string? SaveGameClassName { get; set; }
    public long Unknown1 { get; set; }

    public static SaveGameHeader ReadFrom(BinaryReader reader)
    {
        var magic = reader.ReadChars(4);
        if (!magic.SequenceEqual(FileTypeTag))
            throw new SaveGameException("Invalid magic number in file header, expected: 'GVAS'.");

        var header = new SaveGameHeader
        {
            SaveGameFileVersion = reader.ReadInt32(),
            PackageFileUE4Version = reader.ReadInt32(),
            SavedEngineVersion = EngineVersion.ReadFrom(reader),
            CustomVersionFormat = (CustomVersionSerializationFormat) reader.ReadInt32(),
            CustomVersions = Enumerable.Range(0, reader.ReadInt32()).Select(_ => CustomVersion.ReadFrom(reader)).ToList(),
            SaveGameClassName = reader.ReadFString(),
            Unknown1 = 0
        };
        if (header.SaveGameFileVersion >= 3)
            header.Unknown1 = reader.ReadInt64();
        return header;
    }

    public void WriteTo(BinaryWriter writer)
    {
        writer.Write(Encoding.ASCII.GetBytes(FileTypeTag));
        writer.Write(SaveGameFileVersion);
        writer.Write(PackageFileUE4Version);
        SavedEngineVersion!.WriteTo(writer);
        writer.Write((int) CustomVersionFormat);
        writer.Write(CustomVersions.Count);
        foreach (var customVersion in CustomVersions)
            customVersion.WriteTo(writer);
        writer.WriteFString(SaveGameClassName);
        if (SaveGameFileVersion >= 3)
            writer.Write(Unknown1);
    }
}