namespace FNAFGameData.SaveData;

public class FNAFItemCollectInfo : FNAFSaveStructure
{
    public virtual string ItemName { get; set; }
    public virtual bool HasViewed { get; set; }
    public virtual long CollectionTime { get; set; }
    public virtual int GameHour { get; set; }
    public virtual int GameMinute { get; set; }
    public virtual int PlayIteration { get; set; }
}