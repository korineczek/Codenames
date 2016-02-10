using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.IO;

public class Board : NetworkBehaviour
{
    public Transform Card;
    private string[] finalWords;
    public TextAsset Wordlist;

    [SyncVar]
    public GameObject CommandTest;

    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.L))
        {
            Debug.Log("please delete");
            CommandTest = GameObject.Find("cmdtest(Clone)");
            Cmd_DestroyThis(CommandTest);
        }
        if(Input.GetKeyUp(KeyCode.K))
        {
            Instantiate(CommandTest);
            NetworkServer.Spawn(CommandTest);
        }
        if (Input.GetKeyUp(KeyCode.P))
        {
            RpcTest();
        }
        if (Input.GetKeyUp(KeyCode.O))
        {
            CmdPostTest();
        }
        if (Input.GetKeyUp(KeyCode.I))
        {
            CmdPostTest();
        }
    }

    [ClientRpc]
    public void RpcTest()
    {
        Debug.Log("Penis");
    }

    [Command]
    public void CmdPostTest()
    {
        Debug.Log("PenisFromClient");
    }


    [Command]
    void Cmd_DestroyThis(GameObject please)
    {
        GameObject test = Instantiate(Card).gameObject;
        NetworkServer.Spawn(test);
        NetworkServer.Destroy(please);
    }

    /// <summary>   
    /// 
    /// </summary>
    /// <returns></returns>
    public List<string> ProcessWordList()
    {
        List<string> words = new List<string>();
        TextAsset f = Resources.Load("words") as TextAsset;
        using (StringReader r = new StringReader(f.text))
        {

            string line;
            while ((line = r.ReadLine()) != null)
            {
                words.Add(line);
            }
        }
        return words;
    }

    /// <summary>
    /// 5x5 grid
    /// 8 red agents
    /// 8 blue agents
    /// 1 double agent
    /// 1 assassin
    /// 7 bystanders
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public int[,] GenerateGrid(int[,] input, int seed)
    {
        //set seed to required one
        //Random.seed = seed;

        //set counts for all variables
        const int redCount = 8;
        const int blueCount = 8;
        const int doubleCount = 1;
        const int assassinCount = 1;
        const int bystanderCount = 7;

        //create array
        int[] indexList = new int[bystanderCount + redCount + blueCount + doubleCount + assassinCount];

        //fill array
        for (int i = 0; i < indexList.Length; i++)
        {
            if (i < bystanderCount)
            {
                indexList[i] = 0;
            }
            else if (i < bystanderCount + redCount)
            {
                indexList[i] = 1;
            }
            else if (i < bystanderCount + redCount + blueCount)
            {
                indexList[i] = 2;
            }
            else if (i < bystanderCount + redCount + blueCount + doubleCount)
            {
                indexList[i] = 3;
            }
            else
            {
                indexList[i] = 4;
            }
        }

        //shuffle array (fisher-yates shuffle https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle)
        for (int i = 0; i < indexList.Length; i++)
        {
            //select index from array
            int index = Random.Range(0, indexList.Length - i);
            int number = indexList[index];
            //swap numbers
            indexList[index] = indexList[indexList.Length - 1 - i];
            indexList[indexList.Length - 1 - i] = number;
        }

        //create list of words
        List<string> wordList = ProcessWordList();
        string[] wordArray = wordList.ToArray();
        finalWords = new string[indexList.Length];
        //shuffle and pick 25 words
        for (int i = 0; i < indexList.Length; i++)
        {
            //select index from array
            int index = Random.Range(0, wordArray.Length - i);
            string word = wordArray[index];
            //swap words
            wordArray[index] = wordArray[wordArray.Length - 1 - i];
            wordArray[wordArray.Length - 1 - i] = word;
            //pick final words
            finalWords[i] = word;
        } 
        


        //run position assignment
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                input[i, j] = indexList[i*5 + j];
            }
        }
        return input;
    }

    public void SpawnBoard(int[,] grid, Transform parent)
    {
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                Transform currentCard = Instantiate(Card, new Vector3(i, 0, j), Quaternion.identity) as Transform;
                currentCard.SetParent(parent);
                //give card an index to identify the block
                switch(grid[i,j])
                {
                    case 0:
                        break;
                    case 1:
                        //currentCard.GetComponent<Renderer>().material.color = new Color(1, 0, 0);
                        currentCard.GetComponent<Cards>().CardType = grid[i, j];
                        break;
                    case 2:
                        //currentCard.GetComponent<Renderer>().material.color = new Color(0, 0, 1);
                        currentCard.GetComponent<Cards>().CardType = grid[i, j];
                        break;
                    case 3 :
                        //currentCard.GetComponent<Renderer>().material.color = new Color(0, 1, 1);
                        currentCard.GetComponent<Cards>().CardType = grid[i, j];
                        break;
                    case 4 :
                        //currentCard.GetComponent<Renderer>().material.color = new Color(0, 0, 0);
                        currentCard.GetComponent<Cards>().CardType = grid[i, j];
                        break;
                }
                //assign word to card
                currentCard.GetComponent<Cards>().Word = finalWords[i * 5 + j];
                currentCard.GetComponent<Cards>().CardID = i * 5 + j;
                //spawn shit on clients as well
                NetworkServer.Spawn(currentCard.gameObject);
            }
        }   
    }
}
