namespace FNAFGameData.SaveData;

public class ArcadeSaveData : FNAFSaveStructure
{
    public virtual BalloonWorldSaveData BalloonWorld { get; set; }
    public virtual ChicaFeedingFrenzySaveData ChicaFeedingFrenzy { get; set; }
    public virtual MinigolfSaveData Minigolf { get; set; }
    public virtual MinigolfSaveData MinigolfTwo { get; set; }
    public virtual MinigolfSaveData MinigolfFull { get; set; }
    public virtual PrincessQuestSaveData PrincessQuestOne { get; set; }
    public virtual PrincessQuestSaveData PrincessQuestTwo { get; set; }
    public virtual PrincessQuestSaveData PrincessQuestThree { get; set; }
}