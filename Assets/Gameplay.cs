using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Gameplay : NetworkBehaviour {

    private int[,] board = new int[5,5];

    [SyncVar] public int ScoreRed = 0;
    [SyncVar] public int ScoreBlue = 0;

	// Use this for initialization
	void Start ()
	{
        //get IPv4 address for client joining purposes
        Debug.Log(Dns.GetHostEntry(Dns.GetHostName()).AddressList.FirstOrDefault(a => a.AddressFamily == AddressFamily.InterNetwork));
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log(ScoreRed+ "  "+ ScoreBlue);

	    if (Input.GetKeyUp(KeyCode.G))
	    {
            Debug.Log("generating shit");
            board = GetComponent<Board>().GenerateGrid(board, 10);
            GetComponent<Board>().SpawnBoard(board, this.transform);
	        List<string> testlist = GetComponent<Board>().ProcessWordList();
	    }
	}

    [ClientRpc]
    public void RpcDelete(GameObject obj)
    {
        NetworkIdentity.Destroy(obj);
    }


}
