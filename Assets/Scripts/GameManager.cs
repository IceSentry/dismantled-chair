using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum GameType
{
    Work,
    Study,
    Sleep,
    Count
}

[System.Serializable]
public class PairGameTypeSceneIndex
{
    public int scene;
    public GameType game;
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("GameObject")]
    public Slider[] sliders;

    [Header("Data")]
    public GameConfig[] difficulty;
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
        GameConfig config = difficulty[0];
        sliders[(int)GameType.Work].normalizedValue += Time.deltaTime * config.WorkSpeed;
        sliders[(int)GameType.Study].normalizedValue += Time.deltaTime * config.StudySpeed;
        sliders[(int)GameType.Sleep].normalizedValue += Time.deltaTime * config.SleepSpeed;

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


