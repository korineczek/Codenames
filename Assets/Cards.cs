using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Cards : NetworkBehaviour
{

    [SyncVar] public int CardType;
    [SyncVar] public string Word;
    [SyncVar] public int CardID;

    private Gameplay gameplay;

	void Start () {

        //grab references
	    gameplay = GameObject.Find("BoardGenerator").GetComponent<Gameplay>();

        //set renderer color for master spy
	    if (!transform.GetComponent<NetworkIdentity>().isServer)
	    {
	        switch (CardType)
	        {
	            case 0:
	                break;
	            case 1:
	                GetComponent<Renderer>().material.color = new Color(1, 0, 0);
	                break;
	            case 2:
	                GetComponent<Renderer>().material.color = new Color(0, 0, 1);
	                break;
	            case 4:
	                GetComponent<Renderer>().material.color = new Color(0, 0, 0);
	                break;
	        }
	    }
	    //set text
	    transform.GetChild(0).GetChild(0).GetComponentInChildren<Text>().text = Word;

	}


    /// <summary>
    /// Function for card selection. Reveals its true identity on the server and disables controls of the card on the client
    /// </summary>
    [ServerCallback]
    public void TurnCard()
    {
        //set card color to reveal true identity
        switch (CardType)
        {
            case 0:
                break;
            case 1:
                GetComponent<Renderer>().material.color = new Color(1, 0, 0);
                break;
            case 2:
                GetComponent<Renderer>().material.color = new Color(0, 0, 1);
                break;
            case 3:
                GetComponent<Renderer>().material.color = new Color(0, 1, 1);
                break;
            case 4:
                GetComponent<Renderer>().material.color = new Color(0, 0, 0);
                break;
        }

        //add up scores
        if (CardType == 1)
        {
            gameplay.ScoreRed++;
        }
        else if (CardType == 2)
        {
            gameplay.ScoreBlue++;
        }

        //delete button
        this.transform.GetChild(0).gameObject.SetActive(false);
        RpcSelectCard();
    }

    /// <summary>
    /// RPC call to disable the button on client to indicate card selection
    /// </summary>
    [ClientRpc]
    public void RpcSelectCard()
    {
       this.transform.GetChild(0).gameObject.SetActive(false);  
    }

}
