using UnityEngine;
using System.Collections;

public class Gameplay : MonoBehaviour {

    private int[,] board = new int[5,5];

	// Use this for initialization
	void Start ()
	{
	    board = Board.Instance.GenerateGrid(board, 10);
        Board.Instance.SpawnBoard(board,this.transform);
	}
	
	// Update is called once per frame
	void Update () {
	
	}


}
