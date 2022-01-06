namespace FNAFGameData.SaveData;

public class FNAFWorldStateSaveData : FNAFSaveStructure
{
    public virtual List<string> ActivatedObjects { get; set; }
    public virtual bool bFreddyInWorld { get; set; }
    public virtual Vector FreddyPosition { get; set; }
    public virtual Rotator FreddyRotation { get; set; }
    public virtual bool bCanCallFreddy { get; set; }
    public virtual bool bCanEnterExitFreddy { get; set; }
    public virtual bool bIsInFreddy { get; set; }
    public virtual bool bUseSickFreddy { get; set; }
    public virtual bool bPlayerUsedHidingSpace { get; set; }
    public virtual bool bCanUsePowerStation { get; set; }
    public virtual int FreddyPatrolPoint { get; set; }
    public virtual EFNAFGameState GameState { get; set; }
}