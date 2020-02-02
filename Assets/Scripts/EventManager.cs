using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EventManager : MonoBehaviour
{
    public TextMeshProUGUI eventText;
    public GameEventObject[] gameEvents;

    float timer = 0;
    float spawnTime = 30f;
    int index = 0;

    private void Start()
    {
        eventText.text = "";
        gameEvents.Shuffle();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnTime)
        {
            var gameEvent = gameEvents[index];
            eventText.text = gameEvent.Description;
            GameManager.Instance.DebuffConfig(gameEvent.Config);

            timer -= spawnTime;
            spawnTime = Random.Range(30f, 45f);
        }
    }
}
