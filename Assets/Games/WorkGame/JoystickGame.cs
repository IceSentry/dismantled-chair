using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickGame : MonoBehaviour
{
    int Difficulty = 1;

    const string Button_A_string = "Button_A";
    const string Button_B_string = "Button_B";
    const string Button_X_string = "Button_X";
    const string Button_Y_string = "Button_Y";
    int ButtonCount = 4;
    bool Finish = false;

    Dictionary<SequenceButtons, string> ButtonsMap = new Dictionary<SequenceButtons, string>();

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
        initButtonsMap();
        GenerateSequence();
        if (Random.Range(0, 2) == 0)
        {
            PlaySequence();
        }
        else
        {
            PlaySimonSays();
        }
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

    void initButtonsMap()
    {
        ButtonsMap.Add(SequenceButtons.Button_A, Button_A_string);
        ButtonsMap.Add(SequenceButtons.Button_B, Button_B_string);
        ButtonsMap.Add(SequenceButtons.Button_X, Button_X_string);
        ButtonsMap.Add(SequenceButtons.Button_Y, Button_Y_string);
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
        SequenceButtons[] ButtonSequenceArray = ButtonQueue.ToArray();
        for (int i = 0; i < ButtonSequenceArray.Length; i++)
        {
            Debug.Log(ButtonSequenceArray[i]);
        }

    }
}
