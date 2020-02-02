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

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("GameObject")]
    public Slider[] sliders;
    public EventManager eventManager;
    public GameObject[] visualGameStates;

    [Header("Data")]
    public float globalSpeed = 1f;
    public int workScene;
    public int studyScene;
    public int[] sleepScenes;
    public GameConfig gameConfig;

    [Header("UI")]
    public RectTransform rightPanel;
    public RectTransform titleText;

    GameConfig debuffEvent;
    GameType currentType;
    GameState gameState = GameState.Start;
    int currentScene = -1;
    int currentSleepScene = 0;

    float[] gameTimers;

    private void Awake()
    {
        Instance = this;
        gameTimers = new float[] { 0, 0, 0 };
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
        debuffEvent = config;
    }

    public void SendReward(GameType type, int reward)
    {
        sliders[(int)type].value += reward;
    }

    public void EndGame(GameType type, int reward)
    {
        if (type == GameType.Sleep)
        {
            currentSleepScene++;
            if (currentSleepScene >= sleepScenes.Length)
                currentSleepScene = 0;
        }

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
        for (int i = 0; i < sliders.Length; i++)
        {
            sliders[i].value -= 1;
        }
    }

    void DuringPlay()
    {
        for (int i = 0; i < gameTimers.Length; i++)
        {
            GameType gameType = (GameType)i;
            gameTimers[i] += Time.deltaTime * globalSpeed;
            float timer = gameConfig.GetTimer(gameType) - debuffEvent.GetTimer(gameType);
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

            eventManager.enabled = false;
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
        int scene = GetGameScene(type);
        if (scene > 0)
        {
            LoadTargetScene(scene);

        }
    }

    void LoadTargetScene(int scene)
    {
        currentScene = scene;
        SceneManager.LoadScene(scene, LoadSceneMode.Additive);
    }

    void UnloadCurrentScene()
    {
        if (currentScene > 0)
        {
            SceneManager.UnloadSceneAsync(currentScene);
        }
    }

    int GetGameScene(GameType type)
    {
        switch (type)
        {
            case GameType.Work: return workScene;
            case GameType.Study: return studyScene;
        }

        return sleepScenes[currentSleepScene];
    }
}


