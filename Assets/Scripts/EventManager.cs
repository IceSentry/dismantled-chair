using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class EventManager : MonoBehaviour
{
    public float spawnTime = 3f;
    public TextMeshProUGUI panelEventText;
    public RectTransform warningEvent;
    public TextMeshProUGUI warningTitle, warningEffect;
    public GameEventObject[] gameEvents;

    float timer = 0;
    int index = 0;

    private void Awake()
    {
        panelEventText.text = "";
        warningTitle.text = "";
        warningEffect.text = "";
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
            panelEventText.text = gameEvent.Description;
            GameManager.Instance.DebuffConfig(gameEvent.Config);

            for (int i = 0; i < gameEvent.Visuals.Length; i++)
            {
                gameEvent.Visuals[i].SetActive(true);
            }

            warningEvent.DOLocalMoveY(700, 3f, true).OnComplete(() => 
            {
                Vector2 pos = warningEvent.localPosition;
                pos.y = -700;
                warningEvent.localPosition = pos;
            });
            warningTitle.text = gameEvent.Description;

            timer -= spawnTime;
        }
    }
}