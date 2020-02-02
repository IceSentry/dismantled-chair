using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EventManager : MonoBehaviour
{
    public TextMeshProUGUI eventText;
    public GameEventObject[] gameEvents;

    public float spawnTime = 3f;

    float timer = 0;
    int index = 0;

    private void Awake()
    {
        eventText.text = "";
        gameEvents.Shuffle();
    }

    // Update is called once per frame
    void Update()
    {
        if (index >= gameEvents.Length)
        {
            enabled = false;
            return;
        }

        timer += Time.deltaTime;
        if (timer >= spawnTime)
        {
            var gameEvent = gameEvents[index++];
            eventText.text = gameEvent.Description;
            GameManager.Instance.DebuffConfig(gameEvent.Config);

            for (int i = 0; i < gameEvent.Visuals.Length; i++)
            {
                gameEvent.Visuals[i].SetActive(true);
            }

            timer -= spawnTime;
        }
    }
}
