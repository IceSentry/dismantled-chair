using Assets.Scripts.TypeRacer;
using UnityEngine;
using UnityEngine.UI;

public class TypeRacerSystem : MonoBehaviour
{
    public Text textComponent;
    public Text WPMText;
    public string stringToRace;
    public int wordCountForPoints;

    private ColoredText coloredStringToRace;
    private int index;
    private int wordCompletedCount;
    private float time;

    void Start()
    {
        coloredStringToRace = new ColoredText(stringToRace);
        UpdateColoredText();
    }

    void Update()
    {
        time += Time.deltaTime;
        if (index > coloredStringToRace.list.Count)
            return; //TODO load next phrase

        var currentData = coloredStringToRace.list[index];

        var currentCode = currentData.code;
        var startIndex = index;

        if (currentData.color == "black")
        {
            coloredStringToRace.SetColorAt(index, "yellow");
            UpdateColoredText();
        }

        if (currentCode == KeyCode.Space)
        {
            wordCompletedCount++;
            UpdateWPM();
            if (wordCompletedCount % wordCountForPoints == 0)
            {
                Debug.Log($"{wordCountForPoints} have been written corectly");
                GameManager.Instance.SendReward(GameType.Study, 1);
            }
            index++;
            return;
        }

        if (Input.GetKeyDown(currentCode))
        {
            coloredStringToRace.SetColorAt(index, "green");
            index++;
            UpdateColoredText();

        }
        else if (Input.anyKeyDown && startIndex == index)
        {
            if (Input.GetKeyDown(KeyCode.Space))
                return;

            coloredStringToRace.SetColorAt(index, "red");
            UpdateColoredText();
        }
    }

    void UpdateColoredText()
    {
        textComponent.text = coloredStringToRace.ComputeColoredText();
    }

    void UpdateWPM()
    {
        var WPM = (int)(wordCompletedCount / (time / 60.0));
        WPMText.text = $"{WPM} WPM";
    }
}
