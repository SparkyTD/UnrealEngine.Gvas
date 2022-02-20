using System.Xml.Linq;
using UnrealEngine.Gvas.FProperties;

namespace UnrealEngine.Gvas;

public class SaveGameFile
{
    public SaveGameHeader? Header { get; set; }
    public FStructProperty? Root { get; set; }

    public static SaveGameFile LoadFrom(string path)
    {
        var fileStream = File.OpenRead(path);
        var reader = new DebugBinaryReader(fileStream);

        var saveGameFile = new SaveGameFile();
        saveGameFile.Header = SaveGameHeader.ReadFrom(reader);

        var root = new FStructProperty();
        FProperty property;
        while ((property = FProperty.ReadFrom(reader).First()) != FProperty.NoneProperty)
            root.Fields.Add(property.Name!, property);
        saveGameFile.Root = root;
        
        fileStream.Close();

        return saveGameFile;
    }

    public void Save(string path)
    {
        var fileStream = File.Open(path, FileMode.Create, FileAccess.ReadWrite, FileShare.Read);
        var writer = new BinaryWriter(fileStream);

        try
        {
            Header!.WriteTo(writer);
            foreach (var field in Root!.Fields)
                field.Value.WriteTo(writer);
            writer.WriteFString("None");
            writer.Write(new byte[4]);
        }
        catch (Exception ex)
        {
            Console.Out.WriteLine(ex);
        }
        finally
        {
            writer.Flush();
            writer.Close();
        }
    }

    public XElement Serialize()
    {
        var element = new XElement("SaveData");
        foreach (var property in Root!.Fields)
            element.Add(property.Value.SerializeProperty());
        return element;
    }
}