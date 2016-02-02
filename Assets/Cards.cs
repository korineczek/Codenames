using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Cards : NetworkBehaviour
{

    [SyncVar] public int CardType;
    [SyncVar] public string Word;

	void Start () {
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
	}
}
