using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickGame : MonoBehaviour
{

    int difficulty = 1;

    const string Button_A_string = "Button_A";
    const string Button_B_string = "Button_B";
    const string Button_X_string = "Button_X";
    const string Button_Y_string = "Button_Y";
    int count = 4;

    Dictionary<SequenceButtons, string> ButtonsMap = new Dictionary<SequenceButtons, string>();

    enum SequenceButtons
    {
        Button_A,
        Button_B,
        Button_X,
        Button_Y
    }

    Queue<SequenceButtons> ButtonQueue = new Queue<SequenceButtons>();
    int numSequence = 0;

    // Start is called before the first frame update
    void Start()
    {
        this.numSequence = difficulty * 5;
        initButtonsMap();
        GenerateSequence();
    }

    // Update is called once per frame
    void Update()
    {
        if (ButtonQueue.Count != 0)
        {
            string sequence;
            ButtonsMap.TryGetValue(ButtonQueue.Peek(), out sequence);
            if (Input.GetButtonDown(sequence))
            {
                Debug.Log("Button " + sequence);
                ButtonQueue.Dequeue();
            }
        }
    }

    void GenerateSequence()
    {

        ButtonQueue.Clear();

        for (int i = 0; i < this.numSequence; i++)
        {
            SequenceButtons tmpButton = (SequenceButtons)Random.Range(0, count);
            ButtonQueue.Enqueue(tmpButton);
            Debug.Log(tmpButton);
        }
    }

    void initButtonsMap()
    {
        ButtonsMap.Add(SequenceButtons.Button_A, Button_A_string);
        ButtonsMap.Add(SequenceButtons.Button_B, Button_B_string);
        ButtonsMap.Add(SequenceButtons.Button_X, Button_X_string);
        ButtonsMap.Add(SequenceButtons.Button_Y, Button_Y_string);
    }
}
