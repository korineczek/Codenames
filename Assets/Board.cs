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
    /// <param name="seed"></param>
    /// <param name="startingTeam">starting team influences the amounts of agents for each team. The team that begins gets one agent extra</param>
    /// <returns></returns>
    public int[,] GenerateGrid(int[,] input, int seed, int startingTeam)
    {
        //set seed to required one only enable for development purposes
        //Random.seed = seed;

        //set counts for all variables
        int redCount = startingTeam == 0 ? 9 : 8;
        int blueCount = startingTeam == 1 ? 9 : 8;
        const int assassinCount = 1;
        const int bystanderCount = 7;

        //create array
        int[] indexList = new int[bystanderCount + redCount + blueCount + assassinCount];

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
                currentCard.GetComponent<Cards>().CardType = grid[i, j];
                //assign word to card
                currentCard.GetComponent<Cards>().Word = finalWords[i * 5 + j];
                currentCard.GetComponent<Cards>().CardID = i * 5 + j;
                //spawn shit on clients as well
                NetworkServer.Spawn(currentCard.gameObject);
            }
        }   
    }
}
