using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Interface : MonoBehaviour
{

    private Text redCounter;
    private Text blueCounter;
    private Text statusText;

    private Transform startGame;

    private Gameplay gameplay;

    void Start()
    {
        //Grab references
        redCounter = this.transform.GetChild(0).GetComponent<Text>();
        blueCounter = this.transform.GetChild(1).GetComponent<Text>();
        statusText = this.transform.GetChild(2).GetComponent<Text>();
        startGame = this.transform.GetChild(4);

        //turn off unnecessary UI for the client
        if (!transform.GetComponent<NetworkIdentity>().isServer)
        {
            startGame.gameObject.SetActive(false);
        }

    }

    /// <summary>
    /// Custom update function that can be turned on and off. Had to be done this way instead of a regular update due to unity player-spawning in networking.
    /// </summary>
    /// <returns></returns>
    public IEnumerator UiRefresh()
    {
        //grab reference and set max amounts of cards for both teams
        gameplay = GameObject.Find("BoardGenerator").GetComponent<Gameplay>();
        int maxRed = gameplay.StartingTeam == 0 ? 9 : 8;
        int maxBlue = gameplay.StartingTeam == 1 ? 9 : 8;

        //Display which team starts + their current amount of cards
        while (true)
        {
            statusText.text = gameplay.StartingTeam == 0 ? "RED STARTS" : "BLUE STARTS";
            redCounter.text = gameplay.ScoreRed+"/"+ maxRed;
            blueCounter.text = gameplay.ScoreBlue+"/"+maxBlue;
            yield return new WaitForEndOfFrame();
        }
    }
}
