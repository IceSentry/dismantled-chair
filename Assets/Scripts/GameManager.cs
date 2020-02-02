using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum GameType
{
    Work,
    Study,
    Sleep
}

[System.Serializable]
public class PairGameTypeSceneIndex
{
    public int scene;
    public GameType game;
}

public class GameManager : MonoBehaviour
{
    const int GAMETYPE_COUNT = 3;

    public static GameManager Instance { get; private set; }

    [Header("GameObject")]
    public Slider[] sliders;

    [Header("Data")]
    public GameConfig[] gameConfigs;
    public PairGameTypeSceneIndex[] scenes;

    GameType game;
    List<int>[] gameLists;
    int difficulty;

    GameType currentType;
    int currentScene = -1;

    float[] gameTimers;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        gameTimers = new float[GAMETYPE_COUNT];
        gameLists = new List<int>[GAMETYPE_COUNT];
        for (int i = 0; i < GAMETYPE_COUNT; i++)
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
        GameConfig config = gameConfigs[difficulty];
        for (int i = 0; i < gameTimers.Length; i++)
        {
            float timer = config.GetTimer((GameType)i);
            gameTimers[i] += Time.deltaTime * config.GlobalSpeed;
            if (gameTimers[i] >= timer)
            {
                sliders[i].value += 1;
                gameTimers[i] -= timer;
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            LoadGame(GameType.Work);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            LoadGame(GameType.Study);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            LoadGame(GameType.Sleep);
        }
    }

    public void SendReward(GameType type, int reward)
    {
        sliders[(int)type].value -= reward;
    }

    public void EndGame(GameType type, int reward)
    {
        SendReward(type, reward);
        LoadGame(type);
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


