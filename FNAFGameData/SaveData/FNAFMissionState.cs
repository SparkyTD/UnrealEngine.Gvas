namespace FNAFGameData.SaveData;

public class FNAFMissionState : FNAFSaveStructure
{
    public virtual string? Name { get; set; }
    public virtual EMissionStatus Status { get; set; }
    public virtual int InfoState { get; set; } // Number of tasks completed
    public virtual List<int>? CompletedTasks { get; set; } // Seems to be unused
}