using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Config", menuName = "Data/Config", order = 1)]
public class GameConfig : ScriptableObject
{
    public float WorkSpeed;
    public float StudySpeed;
    public float SleepSpeed;
}
