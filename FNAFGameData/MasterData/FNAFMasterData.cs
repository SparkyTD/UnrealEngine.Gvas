using FNAFGameData.SaveData;
using UnrealEngine.Gvas;

namespace FNAFGameData.MasterData;

public class FNAFMasterData : FNAFSaveStructure
{
    public virtual int newSaveSlotNumber { get; set; }
    public virtual string lastSavedSlotName { get; set; }
    public virtual string lastLoadedSlotName { get; set; }
    public virtual string ActivitySaveSlot { get; set; }
    public virtual Dictionary<string, string> SaveGameSlotNames_Map { get; set; }
    public virtual bool InvertedGamepad { get; set; }
    public virtual bool bLastSaveWasAuto { get; set; }
    public virtual Dictionary<string, string> ActivityIdSaveSlotNamesMap { get; set; }

    public static FNAFMasterData Load(SaveGameFile saveGame) => (FNAFMasterData) Wrap(typeof(FNAFMasterData), saveGame.Root!);
}