using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Gameplay : NetworkBehaviour {

    private int[,] board = new int[5,5];

    [SyncVar] public int ScoreRed = 0;
    [SyncVar] public int ScoreBlue = 0;
    [SyncVar] public bool GameStarted = false;
    [SyncVar] public int StartingTeam = 3;
    private Interface gameUI;

    public void StartGame()
    {
        //Turn on UI monitoring for server and client
        gameUI = GameObject.Find("UI").GetComponent<Interface>();
        gameUI.StartCoroutine("UiRefresh");
        RpcUiRefresh();

        //Generate Board
        Debug.Log("generating shit");
        //Set starting teams
        StartingTeam = Random.Range(0, 2);

        board = GetComponent<Board>().GenerateGrid(board, 10, StartingTeam);
        GetComponent<Board>().SpawnBoard(board, this.transform);

        List<string> testlist = GetComponent<Board>().ProcessWordList();
    }


    [ClientRpc]
    public void RpcUiRefresh()
    {
        GameObject.Find("UI").GetComponent<Interface>().StartCoroutine("UiRefresh");
    }

}
