using System.Collections.Generic;
using UnityEngine;

public class JoystickGame : MonoBehaviour
{
    const string Button_A_string = "Button_A";
    const string Button_B_string = "Button_B";
    const string Button_X_string = "Button_X";
    const string Button_Y_string = "Button_Y";

    const int ButtonCount = 4;

    public float offset = 1f;
    public GameObject buttonSpritePrefab;
    public Sprite[] buttonSprites = new Sprite[ButtonCount];

    bool Finish = false;

    enum SequenceButtons
    {
        Button_A,
        Button_B,
        Button_X,
        Button_Y
    }

    Queue<SequenceButtons> ButtonQueue = new Queue<SequenceButtons>();
    List<SpriteRenderer> ButtonSpriteList = new List<SpriteRenderer>();
    int SpriteListIndex = 0;
    int InitSequenceLength = 5;
    int SequeneLength = 0;

    // Start is called before the first frame update
    void Start()
    {
        GenerateSequence(this.InitSequenceLength);
    }

    // Update is called once per frame
    void Update()
    {
        if (ButtonQueue.Count != 0 && !Finish)
        {
            int buttonMask = 0;
            buttonMask |= Input.GetButtonDown(Button_A_string) ? 1 << (int)SequenceButtons.Button_A : 0;
            buttonMask |= Input.GetButtonDown(Button_B_string) ? 1 << (int)SequenceButtons.Button_B : 0;
            buttonMask |= Input.GetButtonDown(Button_X_string) ? 1 << (int)SequenceButtons.Button_X : 0;
            buttonMask |= Input.GetButtonDown(Button_Y_string) ? 1 << (int)SequenceButtons.Button_Y : 0;

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
                GenerateSequence(1);
                transform.position = new Vector3(transform.position.x - 1, transform.position.y);
            }
            else if (buttonMask != 0)
            {
                Debug.Log("Wrong Button");
            }
        }
    }

    void GenerateSequence(int length)
    {
        for (int i = 0; i < length; i++)
        {
            SequenceButtons tmpButton = (SequenceButtons)Random.Range(0, ButtonCount);
            Debug.Log("Adding Button " + tmpButton);
            ButtonQueue.Enqueue(tmpButton);
            Sprite sprite = buttonSprites[(int)tmpButton];
            GameObject obj = Instantiate(buttonSpritePrefab, new Vector3(SequeneLength, 0f), Quaternion.identity, transform);
            SpriteRenderer renderer = obj.GetComponent<SpriteRenderer>();
            renderer.sprite = sprite;
            ButtonSpriteList.Add(renderer);
            if (SequeneLength < InitSequenceLength)
                SequeneLength++;
        }
    }
}
