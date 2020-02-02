using UnityEngine;

public enum DebuffType
{
    NormalWork,
    BigWork,
    NormalStudy,
    BigStudy,
    NormalSleep,
    BigSleep
}

[System.Serializable]
public class GameEventObject
{
    public string Description;
    public string EffectDescription;
    public GameObject[] Visuals;

    public DebuffType[] Debuffs;
    public GameConfig Config;
}