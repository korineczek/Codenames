using UnityEditor;
using UnityEngine;
using System.Collections;

public class Board : Singleton<Board>
{
    public Transform Card;

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
                switch(grid[i,j])
                {
                    case 0:
                        break;
                    case 1:
                        currentCard.GetComponent<Renderer>().material.color = new Color(1, 0, 0);
                        break;
                    case 2:
                        currentCard.GetComponent<Renderer>().material.color = new Color(0, 0, 1);
                        break;
                    case 3 :
                        currentCard.GetComponent<Renderer>().material.color = new Color(0, 1, 1);
                        break;
                    case 4 :
                        currentCard.GetComponent<Renderer>().material.color = new Color(0, 0, 0);
                        break;
                }
            }
        }   
    }
}
