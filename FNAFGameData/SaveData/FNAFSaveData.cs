using UnrealEngine.Gvas;

namespace FNAFGameData.SaveData;

public class FNAFSaveData : FNAFSaveStructure
{
    public virtual FNAFAISaveData AISaveData { get; set; }
    public virtual FNAFWorldStateSaveData WorldStateData { get; set; }
    public virtual ArcadeSaveData ArcadeSaveData { get; set; }
    public virtual FNAFInventorySaveData InventorySaveData { get; set; }
    public virtual FNAFPowerSaveData FazwatchPowerSaveData { get; set; }
    public virtual FNAFPowerSaveData FreddyPowerSaveData { get; set; }
    public virtual List<FNAFMissionState> MissionState { get; set; }
    public virtual FreddyUpgradeState FreddyUpgrades { get; set; }
    public virtual LightScenarioManagerData LightScenarioManagerData { get; set; }
    public virtual int Hour { get; set; }
    public virtual int Minute { get; set; }
    public virtual int GameIteration { get; set; }
    public virtual int TotalTimePlayedInSeconds { get; set; }
    public virtual DateTime RealtimeSaveTime { get; set; }
    public virtual Vector PlayerLocation { get; set; }
    public virtual Rotator PlayerRotation { get; set; }
    public virtual bool bInPowerStation { get; set; }
    public virtual int PowerStationID { get; set; }
    public virtual List<PowerStationSaveInfo> PowerStationsVisited { get; set; }
    public virtual string ActivityId { get; set; }

    public static FNAFSaveData Load(SaveGameFile saveGame) => (FNAFSaveData) Wrap(typeof(FNAFSaveData), saveGame.Root!);
}