﻿using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Cards : NetworkBehaviour
{

    [SyncVar] public int CardType;
    [SyncVar] public string Word;

	void Start () {
        //set renderer color for master spy
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
        //set text
	    transform.GetChild(0).GetChild(0).GetComponentInChildren<Text>().text = Word;

	}

    public void SelectCard()
    {
       Debug.Log(Network.peerType);
            Destroy(this.gameObject);
        
    }

}
