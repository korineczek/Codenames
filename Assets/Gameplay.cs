using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Gameplay : NetworkBehaviour {

    private int[,] board = new int[5,5];

	// Use this for initialization
	void Start ()
	{

	}
	
	// Update is called once per frame
	void Update () {

	    if (Input.GetKeyUp(KeyCode.G))
	    {
            Debug.Log("generating shit");
            board = GetComponent<Board>().GenerateGrid(board, 10);
            GetComponent<Board>().SpawnBoard(board, this.transform);
	        List<string> testlist = GetComponent<Board>().ProcessWordList();
	    }
	}


}
