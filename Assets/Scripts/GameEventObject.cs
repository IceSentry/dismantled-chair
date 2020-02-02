using UnityEngine;

[CreateAssetMenu(fileName = "Config", menuName = "Data/Config", order = 1)]
public class GameEventObject : ScriptableObject
{
    public string Description;
    public GameConfig Config;
}