using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PopupGame : MonoBehaviour
{
    const string Button_A_string = "Button_A";
    const string Button_B_string = "Button_B";
    const string Button_X_string = "Button_X";
    const string Button_Y_string = "Button_Y";
    const string Left_Bumper_string = "Left_Bumper";
    const string Right_Bumper_string = "Right_Bumper";

    const int ButtonCount = 6;

    const int MaxGameCount = 3;

    enum ButtonType
    {
        Button_A,
        Button_B,
        Button_X,
        Button_Y,
        Left_Bumper,
        Right_Bumper
    }

    public SpriteRenderer workLevelBG;
    public SpriteRenderer[] buttonSpawners;
    public Sprite[] buttonIcons = new Sprite[ButtonCount];
    public float breathingScalingX;
    public float breathingScalingY;
    public float breathingDuration;

    Queue<ButtonType> ButtonQueue = new Queue<ButtonType>();
    List<SpriteRenderer> ButtonSpriteList = new List<SpriteRenderer>();

    int SpriteListIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        GenerateButtonSequence();
        ButtonSpriteList[SpriteListIndex].enabled = true;
        ButtonSpriteList[SpriteListIndex].transform.DOPunchScale(new Vector3(breathingScalingX, breathingScalingY), breathingDuration);

    }

    // Update is called once per frame
    void Update()
    {
        PlaySequence();
    }

    void PlaySequence()
    {
        if (ButtonQueue.Count == 0)
        {
            PopupGameManager.Instance.ClosePopup();
        }


        if (ButtonQueue.Count != 0)
        {
            int buttonMask = 0;
            buttonMask |= Input.GetButtonDown(Button_A_string) ? 1 << (int)ButtonType.Button_A : 0;
            buttonMask |= Input.GetButtonDown(Button_B_string) ? 1 << (int)ButtonType.Button_B : 0;
            buttonMask |= Input.GetButtonDown(Button_X_string) ? 1 << (int)ButtonType.Button_X : 0;
            buttonMask |= Input.GetButtonDown(Button_Y_string) ? 1 << (int)ButtonType.Button_Y : 0;
            buttonMask |= Input.GetButtonDown(Left_Bumper_string) ? 1 << (int)ButtonType.Left_Bumper : 0;
            buttonMask |= Input.GetButtonDown(Right_Bumper_string) ? 1 << (int)ButtonType.Right_Bumper : 0;

            ButtonType peeked = ButtonQueue.Peek();

            if (buttonMask == 1 << (int)peeked)
            {
                ButtonQueue.Dequeue();
                ButtonSpriteList[SpriteListIndex].color = new Color(ButtonSpriteList[SpriteListIndex].color.r / 2,
                                                                    ButtonSpriteList[SpriteListIndex].color.g / 2,
                                                                    ButtonSpriteList[SpriteListIndex].color.b / 2,
                                                                    ButtonSpriteList[SpriteListIndex].color.a / 2);
                if (SpriteListIndex < ButtonSpriteList.Count-1)
                    SpriteListIndex++;

                ButtonSpriteList[SpriteListIndex].enabled = true;
                ButtonSpriteList[SpriteListIndex].transform.DOPunchScale(new Vector3(breathingScalingX, breathingScalingY), breathingDuration);
                GameManager.Instance.SendReward(GameType.Work, PopupGameManager.Instance.rewardValue);
            }
            else if (buttonMask != 0)
            {
                Debug.Log("Wrong Button");
                PopupGameManager.Instance.ShakeCamera();
            }
        }
    }

    void GenerateButtonSequence()
    {
        foreach (SpriteRenderer buttonSpanwer in buttonSpawners)
        {
            ButtonType tmpButton = (ButtonType)Random.Range(0, ButtonCount);
            ButtonQueue.Enqueue(tmpButton);
            Sprite sprite = buttonIcons[(int)tmpButton];
            buttonSpanwer.GetComponent<SpriteRenderer>().sprite = sprite;
            buttonSpanwer.enabled = false;
            ButtonSpriteList.Add(buttonSpanwer);

        }
    }
}
