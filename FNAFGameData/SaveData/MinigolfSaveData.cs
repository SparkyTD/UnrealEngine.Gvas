namespace FNAFGameData.SaveData;

public class MinigolfSaveData : FNAFSaveStructure
{
    public virtual int HighScore { get; set; }
    public virtual bool Played { get; set; }
}