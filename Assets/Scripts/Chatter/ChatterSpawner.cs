/*******************************************************************************
Author: Taylor
State: Working, will have to change when making a more official stadium.
Description:
Handles chatter instantiation
*******************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ChatterSpawner : MonoBehaviour
{
    public Dictionary<string, Chatter> chatterDictionary = new Dictionary<string, Chatter>();

    private ChatterManager chatterManager;

    public GameObject chatterPrefab;
    public Transform chatterParent;
    public Transform startPos;

    //variables for current stadium-style placement
    public int numberOfRows;
    public int objectsPerRow;
    public float spacing;
    public float startingY;
    public float rowOffset;
    public float yOffset;
    private int currentRow = 0;
    private int currentCol = 0;

    //position and rotation for the chatter
    private Vector3 chatterPos;
    private Quaternion chatterRot;

    //gives the chatter their seat number
    private int seatNum;

    private void Awake()
    {
        chatterManager = FindObjectOfType<ChatterManager>();
    }

    public void InstantiateNextChatter(string chatterName)
    {
        List<int> seatList = chatterManager.seatList;
        List<Vector3> posList = chatterManager.positionList;

        if (seatList.Count > 0)
        {
            int smallest = seatList[0];
            int smallestIndex = 0;

            for (int i = 1; i < seatList.Count; i++)
            {
                if (seatList[i] < smallest)
                {
                    smallest = seatList[i];
                    smallestIndex = i;
                }
            }

            //sets the position and rotation of the chatter
            chatterPos = posList[smallestIndex];
            chatterRot = Quaternion.Euler(0, 180, 0);

            //creates a new chatter and assigns it's seat
            Chatter newChatter = CreateChatterObject(chatterName);
            newChatter.seatNum = smallest;

            //removes occupied seat from list
            seatList.RemoveAt(smallestIndex);
            posList.RemoveAt(smallestIndex);
        }
        else
        {
            if (currentRow < numberOfRows && currentCol < objectsPerRow)
            {
                //offsets based on stadium positions
                float xOffset = (currentRow % 2 == 0) ? 0f : rowOffset;
                float rowStep = startingY + (currentRow * yOffset);

                //sets the starting position and rotation
                chatterPos = new Vector3(startPos.position.x + currentCol * spacing + xOffset, startPos.position.y + rowStep, startPos.position.z + currentRow * spacing);
                chatterRot = Quaternion.Euler(0, 180, 0);

                //creates a new chatter and assigns it's seat
                Chatter newChatter = CreateChatterObject(chatterName);
                newChatter.seatNum = seatNum;
                seatNum++;

                //keeps track of the columns of the stadium that are filled
                currentCol++;
                if (currentCol >= objectsPerRow)
                {
                    currentCol = 0;
                    currentRow++;
                }
            }
        }
    }

    public Chatter CreateChatterObject(string chatter)
    {
        // Instantiate a new Chatter GameObject dynamically
        GameObject chatterObject = Instantiate(chatterPrefab, chatterPos, chatterRot, chatterParent);
        chatterObject.name = chatter;

        //gets the chatters script component
        Chatter chatterComponent = chatterObject.GetComponent<Chatter>();

        //adds the chatter to the dictionary
        chatterDictionary[chatter] = chatterComponent;

        return chatterComponent;
    }

    public void DestroyChatter(string chatter)
    {
        if (chatterDictionary.ContainsKey(chatter))
        {
            chatterDictionary.Remove(chatter);
            Destroy(GameObject.Find(chatter));
        }
    }
}