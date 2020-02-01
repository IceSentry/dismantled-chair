using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameType
{
    Work,
    Study,
    Sleep,
    Count
}

public class GameManager : MonoBehaviour
{
    public GameType[] scenes;

    GameType game;
    List<int>[] gameLists;

    // Start is called before the first frame update
    void Start()
    {
        int gametypeCount = (int)GameType.Count;
        gameLists = new List<int>[gametypeCount];
        for (int i = 0; i < gametypeCount; i++)
            gameLists[i] = new List<int>();

        for (int i = 0; i < scenes.Length; i++)
        {
            int type = (int)scenes[i];
            gameLists[type].Add(i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}


