﻿using System.Collections.Generic;
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
        //Set starting teams
        StartingTeam = Random.Range(0, 2);
        //generate board and spawnit
        board = GetComponent<Board>().GenerateGrid(board, 10, StartingTeam);
        GetComponent<Board>().SpawnBoard(board, this.transform);
    }

    /// <summary>
    /// RPC call to client that starts the monititoring of the game UI
    /// </summary>
    [ClientRpc]
    public void RpcUiRefresh()
    {
        GameObject.Find("UI").GetComponent<Interface>().StartCoroutine("UiRefresh");
    }

}
