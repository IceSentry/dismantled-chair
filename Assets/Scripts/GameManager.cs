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
    EndGame
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("GameObject")]
    public Slider[] sliders;
    public EventManager eventManager;
    public GameObject[] visualGameStates;

    [Header("Data")]
    public float endGameTime = 120f;
    public float globalSpeed = 1f;
    public int workScene;
    public int studyScene;
    public int[] sleepScenes;
    public GameConfig gameConfig;

    [Header("UI")]

    public RectTransform rightPanel;
    public RectTransform leftPanel;
    public RectTransform titleText;
    public float UIAnimationSpeed;
    public GameObject introGroup;
    public GameObject gameOverGroup;
    public GameObject victoryGroup;
    public Text gameOverStatusText;

    GameConfig debuffEvent;
    GameType currentType;
    GameState gameState = GameState.Start;
    int currentScene = -1;
    int currentSleepScene = 0;

    float endGameTimer;
    float[] gameTimers;

    bool gameStarted;
    Coroutine transition;

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
            case GameState.EndGame:
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
        if (Input.anyKeyDown && transition == null)
        {
            transition = StartCoroutine(WaitToStart());
        }
    }

    IEnumerator WaitToStart()
    {
        rightPanel.DOAnchorPos3DX(-360, UIAnimationSpeed);
        leftPanel.DOAnchorPos3DX(600, UIAnimationSpeed);
        titleText.DOAnchorPos3DX(-400, UIAnimationSpeed);

        yield return new WaitForSeconds(UIAnimationSpeed);

        yield return new WaitUntil(() =>
        {
            CheckEnterGameInput();
            return gameStarted;
        });

        leftPanel.DOAnchorPos3DX(-600, UIAnimationSpeed);
        yield return new WaitForSeconds(UIAnimationSpeed);

        introGroup.SetActive(false);

        gameState = GameState.During;
        eventManager.enabled = true;
        for (int i = 0; i < sliders.Length; i++)
        {
            sliders[i].value--;
        }

        transition = null;
    }

    void DuringPlay()
    {
        endGameTimer += Time.deltaTime;
        if (endGameTimer >= endGameTime)
        {
            Finish();
            return;
        }

        for (int i = 0; i < gameTimers.Length; i++)
        {
            var gameType = (GameType)i;
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

        CheckEnterGameInput();
    }

    void CheckEnterGameInput()
    {
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

    void Finish()
    {
        Debug.Log("Finish");
        gameState = GameState.EndGame;
        transition = StartCoroutine(ShowVictoryPanel());
    }

    IEnumerator ShowVictoryPanel()
    {
        victoryGroup.SetActive(true);
        leftPanel.DOAnchorPos3DX(600, UIAnimationSpeed);

        yield return new WaitForSeconds(UIAnimationSpeed);

        DisableGameplay();

        StartCoroutine(WaitForRestart());

        transition = null;
    }

    void GameOver()
    {
        if (gameState != GameState.EndGame)
        {
            gameState = GameState.EndGame;
            transition = StartCoroutine(ShowGameOverPanel());
        }
    }

    void DisableGameplay()
    {
        UnloadCurrentScene();
        eventManager.enabled = false;
    }

    IEnumerator ShowGameOverPanel()
    {
        if (eventManager.index > 0)
        {
            var text = "You failed at fixing you life because:\n";
            for (int i = 0; i <= eventManager.index; i++)
            {
                text += $" - {eventManager.gameEvents[i].EndStatusDesc}\n";
            }
            gameOverStatusText.text = text;
        }
        else
        {
            gameOverStatusText.enabled = false;
        }

        gameOverGroup.SetActive(true);
        leftPanel.DOAnchorPos3DX(600, UIAnimationSpeed);

        yield return new WaitForSeconds(UIAnimationSpeed);

        DisableGameplay();

        StartCoroutine(WaitForRestart());

        transition = null;
    }

    IEnumerator WaitForRestart()
    {
        yield return new WaitUntil(() => Input.anyKey);

        SceneManager.LoadScene(0);
    }

    void EnterGame(GameType type)
    {
        if (!gameStarted)
            gameStarted = true;

        if (type == currentType && currentScene > 0)
            return;

        LoadGame(type);
    }

    void LoadGame(GameType type)
    {
        UnloadCurrentScene();

        visualGameStates[(int)currentType]?.SetActive(false);
        visualGameStates[(int)type]?.SetActive(true);

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


