using UnrealEngine.Gvas;

namespace SavToXml;

public static class Program
{
    public static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.Out.WriteLine("Usage: SavToXml savFile [xmlFile]");
            return;
        }

        string sourceFile = args[0];
        if (!File.Exists(sourceFile))
        {
            Console.Out.WriteLine("Source file does not exist.");
            return;
        }

        string targetFile = Path.Combine(Path.GetDirectoryName(sourceFile)!, Path.GetFileNameWithoutExtension(sourceFile) + ".xml");
        if (args.Length >= 2)
            targetFile = args[1];

        var saveData = SaveGameFile.LoadFrom(sourceFile);
        File.WriteAllText(targetFile, saveData.Serialize().ToString());
        Console.Out.WriteLine("Save file was converted successfully");
    }
}