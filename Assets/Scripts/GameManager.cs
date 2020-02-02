using DG.Tweening;
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

public enum GameState
{
    Start,
    During,
    GameOver
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
    public EventManager eventManager;
    public GameObject[] visualGameStates;

    [Header("Data")]
    public float globalSpeed = 1f;
    public GameConfig gameConfig;
    public PairGameTypeSceneIndex[] scenes;

    [Header("UI")]
    public RectTransform rightPanel;
    public RectTransform titleText;

    GameType game;
    List<int>[] gameLists;
    int difficulty;

    GameState gameState;
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
        gameState = GameState.Start;

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
        switch (gameState)
        {
            case GameState.Start:
                ScreenPlay();
                break;
            case GameState.During:
                DuringPlay();
                break;
            case GameState.GameOver:
                break;
        }   
    }

    public void DebuffConfig(GameConfig config)
    {
        gameConfig.WorkTimer -= config.WorkTimer;
        gameConfig.SleepTimer -= config.SleepTimer;
        gameConfig.StudyTimer -= config.StudyTimer;
    }

    public void SendReward(GameType type, int reward)
    {
        sliders[(int)type].value += reward;
    }

    public void EndGame(GameType type, int reward)
    {
        SendReward(type, reward);
        LoadGame(type);
    }

    void ScreenPlay()
    {
        if (Input.anyKeyDown)
        {
            rightPanel.DOAnchorPos3DX(-360, 0.5f);
            titleText.DOAnchorPos3DX(-400, 0.5f);
            StartCoroutine(WaitToStart());
        }
    }

    IEnumerator WaitToStart()
    {
        yield return new WaitForSeconds(0.5f);

        gameState = GameState.During;
        eventManager.enabled = true;
    }

    void DuringPlay()
    {
        for (int i = 0; i < gameTimers.Length; i++)
        {
            gameTimers[i] += Time.deltaTime * globalSpeed;
            float timer = gameConfig.GetTimer((GameType)i);
            if (gameTimers[i] >= timer)
            {
                var slider = sliders[i];
                if (slider.value > 0)
                {
                    slider.value -= 1;
                }
                else
                {
                    GameOver();
                    return;
                }
                gameTimers[i] -= timer;
            }
        }

        if (Input.GetButtonDown("Button_A"))
        {
            EnterGame(GameType.Work);
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            EnterGame(GameType.Study);
        }
        else if (Input.GetMouseButtonDown(0))
        {
            EnterGame(GameType.Sleep);
        }
    }

    void GameOver()
    {
        if (gameState != GameState.GameOver)
        {
            Debug.Log("GameOver");
            UnloadCurrentScene();
            gameState = GameState.GameOver;
        }
    }

    void EnterGame(GameType type)
    {
        if (type == currentType && currentScene > 0)
            return;

        LoadGame(type);
    }

    void LoadGame(GameType type)
    {
        UnloadCurrentScene();

        if (visualGameStates[(int)currentType] != null)
        {
            visualGameStates[(int)currentType].SetActive(false);
        }

        if (visualGameStates[(int)type] != null)
        {
            visualGameStates[(int)type].SetActive(true);
        }

        currentType = type;
        int scene = GetRandomGame(type);
        if (scene > 0)
        {
            LoadTargetScene(scene);

        }
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
        if(list.Count > 0)
            return list[Random.Range(0, list.Count)];

        return -1;
    }
}


