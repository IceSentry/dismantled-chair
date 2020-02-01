using System.Collections;
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

    int Difficulty = 1;
    bool Finish = false;

    enum SequenceButtons
    {
        Button_A,
        Button_B,
        Button_X,
        Button_Y
    }

    Queue<SequenceButtons> ButtonQueue = new Queue<SequenceButtons>();
    int SequenceLength = 0;
    int SequenceCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        this.SequenceLength = Difficulty * 5;
        GenerateSequence();

        PlaySequence();
        //if (Random.Range(0, 2) == 0)
        //{
        //    PlaySequence();
        //}
        //else
        //{
        //    PlaySimonSays();
        //}
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
                if (ButtonQueue.Count == 0 && SequenceCount < Difficulty)
                {
                    SequenceCount++;
                    Debug.Log("Round #" + SequenceCount + " finish!");
                    if (SequenceCount == Difficulty) {
                        Finish = true;
                    }
                        Debug.Log("Game Finish");

                    GenerateSequence();
                }
            }
            else if (buttonMask != 0)
            {
                Debug.Log("Wrong Button");
            }
        }
    }

    void GenerateSequence()
    {
        ButtonQueue.Clear();
        for (int i = 0; i < this.SequenceLength; i++)
        {
            SequenceButtons tmpButton = (SequenceButtons)Random.Range(0, ButtonCount);
            ButtonQueue.Enqueue(tmpButton);
        }
    }

    void PlaySimonSays()
    {

        Debug.Log("Play SimonSays");
        SequenceButtons[] ButtonSequenceArray = ButtonQueue.ToArray();
        for (int i = 0; i < ButtonSequenceArray.Length; i++)
        {
            // Show button
            Debug.Log(ButtonSequenceArray[i]);
            // Wait 1 sec 
            // Hide button
        }
    }
    void PlaySequence()
    {
        Debug.Log("Play Button Sequence");
        int index = 0;
        foreach (var button in ButtonQueue)
        {
            Debug.Log(button);
            Sprite sprite = buttonSprites[(int)button];
            GameObject obj = Instantiate(buttonSpritePrefab, new Vector3(index * offset, 0f), Quaternion.identity, transform);
            SpriteRenderer renderer = obj.GetComponent<SpriteRenderer>();
            renderer.sprite = sprite;
            index++;
        }
    }
}
