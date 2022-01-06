namespace FNAFGameData.SaveData;

public class FNAFAISaveData : FNAFSaveStructure
{
    public virtual bool bShatteredChica { get; set; }
    public virtual bool bShatteredRoxy { get; set; }
    public virtual bool bShatteredMonty { get; set; }
    public virtual bool bWorldSpawnEnabled { get; set; }
    public virtual bool bAITeleportEnabled { get; set; }
    public virtual Object AnimatronicStates { get; set; }
    public virtual List<EndoSaveData> Endos { get; set; }
}