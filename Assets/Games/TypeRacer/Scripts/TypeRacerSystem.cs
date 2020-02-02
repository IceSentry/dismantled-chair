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
        "have had a significant influence on popular culture.",

        "But first, we need to talk about parallel universes. Now, you are probably what I am going " +
        "to need all this speed for. After all, I do build up speed for 12 hours. But to answer that, " +
        "we need to talk about parallel universes. And if you thought my other tangents were complicated, just you wait.",

        "History also includes the academic discipline which uses a narrative to examine and analyse a sequence " +
        "of past events, and objectively determine the patterns of cause and effect that determine them.",

        "According to all known laws of aviation, there is no way a bee should be able to fly. " +
        "Its wings are too small to get its fat little body off the ground. The bee, of course, " +
        "flies anyway because bees don't care what humans think is impossible. " +
        "Yellow, black. Yellow, black. Yellow, black. Yellow, black.",

        "It deos not mttaer in waht oredr the ltteers in a wrod are, the olny iprmoetnt tihng is " +
        "taht the frist and lsat ltteer be at the rghit pclae. The rset can be a toatl mses and you can " +
        "sitll raed it wouthit porbelm. Tihs is bcuseae the huamn mnid deos not raed ervey lteter by istlef, " +
        "but the wrod as a wlohe.",

        "Is this the real life\nIs this just fantasy\nCaught in a landslide,\nNo escape from reality.\n" + 
        "Open your eyes,\nLook up to the skies and see,\nI m just a poor boy, I need no sympathy,\n" +
        "Because I'm easy come, easy go,\nLittle high, little low,"
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
        var startIndex = index;

        if (currentData.c == ' ' || currentData.c == '\n')
        {
            wordCompletedCount++;
            if (wordCompletedCount % wordCountForPoints == 0)
            {
                GameManager.Instance?.SendReward(GameType.Study, 1);
            }
            index++;
            return;
        }

        if (currentData.color == TextColor.NORMAL)
        {
            coloredStringToRace.SetColorAt(index, TextColor.CURSOR);
            UpdateColoredText();
        }

        if (Input.GetKeyDown(currentData.code))
        {
            coloredStringToRace.SetColorAt(index, TextColor.SUCCESS);
            index++;
            UpdateColoredText();
        }
        else if (Input.anyKeyDown && startIndex == index)
        {
            if (Input.GetKeyDown(KeyCode.Space))
                return;

            //coloredStringToRace.SetColorAt(index, TextColor.FAILURE);
            //UpdateColoredText();
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
