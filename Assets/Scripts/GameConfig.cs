using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct GameConfig
{
    public float WorkTimer;
    public float StudyTimer;
    public float SleepTimer;

    public float GetTimer(GameType type)
    {
        switch (type)
        {
            case GameType.Work: return WorkTimer;
            case GameType.Study: return StudyTimer;
            case GameType.Sleep: return SleepTimer;
        }

        throw new System.ArgumentException($"GameType: {type} is not valid.");
    }


}
