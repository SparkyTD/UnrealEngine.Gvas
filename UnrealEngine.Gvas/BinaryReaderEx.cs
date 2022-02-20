using System.Text;
using UnrealEngine.Gvas.Exceptions;

namespace UnrealEngine.Gvas;

public static class BinaryReaderEx
{
    public static string? ReadFString(this BinaryReader reader)
    {
        int length = reader.ReadInt32();
        if (length is > 512 or < 0)
            throw new SaveGameException($"Sanity check failed: FString length is {length}");

        return Encoding.ASCII.GetString(reader.ReadBytes(length)).TrimEnd('\0');
    }

    public static void WriteFString(this BinaryWriter writer, string? str)
    {
        if (str is {Length: > 0})
        {
            writer.Write(str!.Length + 1);
            writer.Write(str.ToCharArray());
            writer.Write((byte) 0);
        }
        else
        {
            writer.Write(0);
        }
    }
}