using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameType
{
    Work,
    Study,
    Sleep,
    Count
}

[System.Serializable]
public struct PairGameTypeSceneIndex
{
    public int scene;
    public GameType game;
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public PairGameTypeSceneIndex[] scenes;

    GameType game;
    List<int>[] gameLists;

    GameType currentType;
    int currentScene = -1;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        int gametypeCount = (int)GameType.Count;
        gameLists = new List<int>[gametypeCount];
        for (int i = 0; i < gametypeCount; i++)
            gameLists[i] = new List<int>();

        for (int i = 0; i < scenes.Length; i++)
        {
            int type = (int)scenes[i].game;
            gameLists[type].Add(scenes[i].scene);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            LoadGame(GameType.Work);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            LoadGame(GameType.Study);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            LoadGame(GameType.Sleep);
        }
    }

    public void EndGame(bool success)
    {
        LoadGame(currentType);
    }

    void LoadGame(GameType type)
    {
        UnloadCurrentScene();

        currentType = type;
        LoadTargetScene(GetRandomGame(type));
    }

    void LoadTargetScene(int scene)
    {
        currentScene = scene;
        SceneManager.LoadScene(scene, LoadSceneMode.Additive);
        //SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(scene));
    }

    void UnloadCurrentScene()
    {
        if (currentScene > 0)
        {
            SceneManager.UnloadSceneAsync(currentScene);
        }
    }

    int GetRandomGame(GameType type)
    {
        var list = gameLists[(int)type];
        return list[Random.Range(0, list.Count)];
    }
}


