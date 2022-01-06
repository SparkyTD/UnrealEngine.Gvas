namespace FNAFGameData.SaveData;

public class FNAFInventorySaveData : FNAFSaveStructure
{
    public virtual List<FNAFItemCollectInfo> InventoryItems { get; set; }
    public virtual List<FNAFItemCollectInfo> Messages { get; set; }
    public virtual int SecurityLevel { get; set; }
    public virtual int UsedPartyPassCount { get; set; }
    public virtual int NumFlashCharges { get; set; }
    public virtual int FlashlightInStationID { get; set; }
    public virtual Object TapesListenedTo { get; set; }
    public virtual int DishesBroken { get; set; }
}