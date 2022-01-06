namespace FNAFGameData.SaveData;

public class PowerStationSaveInfo : FNAFSaveStructure
{
    public virtual int PowerStationID { get; set; }
    public virtual int GameIteration { get; set; }
    public virtual int GameHour { get; set; }
    public virtual int GameMinute { get; set; }
}