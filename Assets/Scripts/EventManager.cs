using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

[System.Serializable]
public class DebuffRenderer
{
    public GameObject debuff;
    public GameObject bigDebuff;
}

public class EventManager : MonoBehaviour
{
    public float spawnTime = 3f;
    public TextMeshProUGUI panelEventText;
    public RectTransform warningEvent;
    public TextMeshProUGUI warningTitle, warningEffect;
    public DebuffRenderer[] debuffRenderers;

    [Header("Event")]
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
            warningEffect.text = gameEvent.EffectDescription;

            HideAllDebuffs();
            ShowDebuff(gameEvent.Debuffs);

            if (index >= gameEvents.Length)
                index = 0;

            timer -= spawnTime;
        }

        void ShowDebuff(DebuffType[] debuffs)
        {
            for (int i = 0; i < debuffs.Length; i++)
            {
                switch (debuffs[i])
                {
                    case DebuffType.NormalWork:
                        debuffRenderers[0].debuff.SetActive(true);
                        break;
                    case DebuffType.BigWork:
                        debuffRenderers[0].bigDebuff.SetActive(true);
                        break;
                    case DebuffType.NormalStudy:
                        debuffRenderers[1].debuff.SetActive(true);
                        break;
                    case DebuffType.BigStudy:
                        debuffRenderers[1].bigDebuff.SetActive(true);
                        break;
                    case DebuffType.NormalSleep:
                        debuffRenderers[2].debuff.SetActive(true);
                        break;
                    case DebuffType.BigSleep:
                        debuffRenderers[2].bigDebuff.SetActive(true);
                        break;
                }
            }
        }

        void HideAllDebuffs()
        {
            for (int i = 0; i < debuffRenderers.Length; i++)
            {
                debuffRenderers[i].debuff.SetActive(false);
                debuffRenderers[i].bigDebuff.SetActive(false);
            }
        }
    }
}