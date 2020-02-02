using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PopupGameManager : MonoBehaviour
{
    public GameObject[] popupGameArray;
    public static PopupGameManager Instance { get; private set; }
    public int rewardValue = 1;
    public Transform shakeTarget;
    public float shakeDuration;
    public Vector3 shakeStrength;
    public float disableTime;
    private GameObject currentPopup;
    private float shakeTimeRemaining;

    int gameIndex = 0;
    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        popupGameArray.Shuffle();
        CreatePopup();
    }

    public void CreatePopup()
    {
        var popup = popupGameArray[gameIndex];
        currentPopup = Instantiate(popup, transform, false);
        var renderer = currentPopup.GetComponent<SpriteRenderer>();
        renderer.enabled = true;
        gameIndex++;
        if (gameIndex == popupGameArray.Length)
            gameIndex = 0;
    }

    public void ClosePopup()
    {
        Destroy(currentPopup);
        CreatePopup();
    }

    // Update is called once per frame
    void Update()
    {
        shakeTimeRemaining -= Time.deltaTime;

    }
    public void ShakeCamera()
    {
        if (shakeTimeRemaining < 0)
        {
            shakeTarget.DOShakePosition(shakeDuration, shakeStrength, 20, 200f);
            shakeTimeRemaining = shakeDuration;
        }
    }
}
