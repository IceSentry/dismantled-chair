using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupGame : MonoBehaviour
{
    const string Button_A_string = "Button_A";
    const string Button_B_string = "Button_B";
    const string Button_X_string = "Button_X";
    const string Button_Y_string = "Button_Y";
    const string Left_Bumper_string = "Left_Bumper";
    const string Right_Bumper_string = "Right_Bumper";

    const int ButtonCount = 6;

    const int SpriteCount = 5;

    const int MaxGameCount = 3;

    int Difficulty = 1;

    int GameCount = 0;

    enum SequenceButtons
    {
        Button_A,
        Button_B,
        Button_X,
        Button_Y,
        Left_Bumper,
        Right_Bumper
    }

    public SpriteRenderer[] buttonSprites;
    Queue<SequenceButtons> ButtonQueue = new Queue<SequenceButtons>();
    List<SpriteRenderer> ButtonSpriteList = new List<SpriteRenderer>();

    int SpriteListIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        Shuffle(buttonSprites);
        AddButtons();

    }

    // Update is called once per frame
    void Update()
    {
        //while (GameCount < MaxGameCount) {
            PlaySequence();
        //}
    }

    public static void Shuffle<T>(T[] array)
    {
        int n = array.Length;
        while (n > 1)
        {
            int k = Random.Range(0,n--);
            T temp = array[n];
            array[n] = array[k];
            array[k] = temp;
        }
    }
    void PlaySequence()
    {
        if (ButtonQueue.Count == 0)
        {
            foreach (var sprite in ButtonSpriteList)
            {
                Destroy(sprite.gameObject);
            }
            GameCount++;
            AddButtons();
        }


        if (ButtonQueue.Count != 0)
        {
            int buttonMask = 0;
            buttonMask |= Input.GetButtonDown(Button_A_string) ? 1 << (int)SequenceButtons.Button_A : 0;
            buttonMask |= Input.GetButtonDown(Button_B_string) ? 1 << (int)SequenceButtons.Button_B : 0;
            buttonMask |= Input.GetButtonDown(Button_X_string) ? 1 << (int)SequenceButtons.Button_X : 0;
            buttonMask |= Input.GetButtonDown(Button_Y_string) ? 1 << (int)SequenceButtons.Button_Y : 0;
            buttonMask |= Input.GetButtonDown(Left_Bumper_string) ? 1 << (int)SequenceButtons.Left_Bumper : 0;
            buttonMask |= Input.GetButtonDown(Right_Bumper_string) ? 1 << (int)SequenceButtons.Right_Bumper : 0;

            SequenceButtons peeked = ButtonQueue.Peek();

            if (buttonMask == 1 << (int)peeked)
            {
                Debug.Log("Dequeued " + peeked);
                ButtonQueue.Dequeue();
                ButtonSpriteList[SpriteListIndex].color = new Color(ButtonSpriteList[SpriteListIndex].color.r / 2,
                                                                    ButtonSpriteList[SpriteListIndex].color.g / 2,
                                                                    ButtonSpriteList[SpriteListIndex].color.b / 2,
                                                                    ButtonSpriteList[SpriteListIndex].color.a / 2);
                SpriteListIndex++;
            }
            else if (buttonMask != 0)
            {
                Debug.Log("Wrong Button");
            }
        }
    }

    void AddButtons() {
        for (int i = 0; i < SpriteCount; i++)
        {
            SequenceButtons tmpButton = (SequenceButtons)Random.Range(0, ButtonCount);
            ButtonQueue.Enqueue(tmpButton);
            Sprite sprite = buttonSprites[(int)tmpButton];
            renderer.sprite = sprite;
            ButtonSpriteList.Add(renderer);
            //buttonSprites[]
            //buttonSprites[i].enabled = true;

        }
    }
}
