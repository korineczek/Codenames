using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Interface : MonoBehaviour
{

    private Text redCounter;
    private Text blueCounter;
    private Text statusText;
    private Gameplay gameplay;

    void Start()
    {
        //Grab references
        redCounter = this.transform.GetChild(0).GetComponent<Text>();
        blueCounter = this.transform.GetChild(1).GetComponent<Text>();
        statusText = this.transform.GetChild(2).GetComponent<Text>();

    }

    public IEnumerator UiRefresh()
    {
        gameplay = GameObject.Find("BoardGenerator").GetComponent<Gameplay>();
        int maxRed = gameplay.StartingTeam == 0 ? 9 : 8;
        int maxBlue = gameplay.StartingTeam == 1 ? 9 : 8;

        while (true)
        {
            statusText.text = gameplay.StartingTeam == 0 ? "RED STARTS" : "BLUE STARTS";
            redCounter.text = gameplay.ScoreRed+"/"+ maxRed;
            blueCounter.text = gameplay.ScoreBlue+"/"+maxBlue;
            yield return new WaitForEndOfFrame();
        }
    }
}
