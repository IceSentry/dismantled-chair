using Assets.Scripts.TypeRacer;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TypeRacerSystem : MonoBehaviour
{
    public Text textComponent;
    public Text WPMText;
    public int wordCountForPoints;
    public bool useDebug;

    private List<string> dictionary = new List<string>() {
        "Gender is pertinent to many disciplines, such as literary theory, drama studies, " +
        "film theory, performance theory, contemporary art history, anthropology, sociology, " +
        "sociolinguistics and psychology.",

        "By design, a blockchain is resistant to modification of the data. It is an open, " +
        "distributed ledger that can record transactions between two parties efficiently and" +
        " in a verifiable and permanent way.",

        "Calculus is the mathematical study of continuous change, in the same way that geometry " +
        "is the study of shape and algebra is the study of generalizations of arithmetic operations.",

        "In Puritan Boston, Massachusetts, a crowd gathers to witness the punishment of Hester Prynne, " +
        "a young woman who has given birth to a baby of unknown parentage. She is required to wear a " +
        "scarlet A on her dress when she is in front of the townspeople to shame her.",

        "The FitnessGram Pacer Test is a multistage aerobic capacity test that progressively gets more " +
        "difficult as it continues. The twenty meter pacer test will begin in thirty seconds. " +
        "Line up at the start. The running speed starts slowly but gets faster each minute after " +
        "you hear this signal bodeboop.",

        "Video game culture is a worldwide new media subculture formed around video games and " +
        "game playing. As computer and video games have increased in popularity over time, they " +
        "have had a significant influence on popular culture."
    };

    private ColoredText coloredStringToRace;
    private int stringToRaceIndex = -1;
    private int index;
    private int wordCompletedCount;
    private float time;

    private List<string> debugDictionary = new List<string>()
    {
        "A B.",
        "A B BBA C",
        "hello world",
    };

    void Start()
    {
        if (useDebug)
        {
            dictionary = debugDictionary;
        }
        stringToRaceIndex = GetNextStringToRaceIndex();
        coloredStringToRace = GetNextColoredText();
        UpdateColoredText();
    }

    void Update()
    {
        time += Time.deltaTime;
        if (index >= coloredStringToRace.list.Count)
        {
            ChangeText();
            return;
        }

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
            if (wordCompletedCount % wordCountForPoints == 0)
            {
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

        UpdateWPM();
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

    int GetNextStringToRaceIndex()
    {
        int nextStringToRaceIndex;
        do
        {
            nextStringToRaceIndex = Random.Range(0, dictionary.Count);
        } while (nextStringToRaceIndex == stringToRaceIndex);
        return nextStringToRaceIndex;
    }

    ColoredText GetNextColoredText()
    {
        return new ColoredText(dictionary[stringToRaceIndex]);
    }

    void ChangeText()
    {
        stringToRaceIndex = GetNextStringToRaceIndex();
        coloredStringToRace = GetNextColoredText();
        index = 0;
        UpdateColoredText();
    }
}
