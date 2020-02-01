using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Config", menuName = "Data/Config", order = 1)]
public class GameConfig : ScriptableObject
{
    public float GlobalSpeed = 0.1f;

    public float WorkTimer = 2f;
    public float StudyTimer = 2f;
    public float SleepTimer = 2f;

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
