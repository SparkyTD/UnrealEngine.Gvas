namespace FNAFGameData.SaveData;

public class ChicaFeedingFrenzySaveData : FNAFSaveStructure
{
    public virtual int HighScore { get; set; }
    public virtual bool Played { get; set; }
    public virtual bool Glitched { get; set; }
}