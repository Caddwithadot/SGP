using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using System;

public class ChatterSpawner : MonoBehaviour
{
    public Dictionary<string, Chatter> chatterDictionary = new Dictionary<string, Chatter>();

    private ChatManager chatManager;
    private ChatterManager chatterManager;

    public Transform chatterParent;
    public GameObject chatterPrefab;
    public Transform startPos;

    public int numberOfRows;
    public int objectsPerRow;
    public float spacing;
    public float startingY;
    public float rowOffset;
    public float yOffset;

    private int currentRow = 0;
    private int currentCol = 0;

    private int seatNum;

    private void Awake()
    {
        chatManager = FindObjectOfType<ChatManager>();
        chatterManager = FindObjectOfType<ChatterManager>();
    }

    void Update()
    {
        if (chatManager.newGuy != null)
        {
            InstantiateNextChatter(chatManager.newGuy);

            chatManager.newGuy = null;
        }
    }

    private void InstantiateNextChatter(string chatterName)
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
            Vector3 startPos = posList[smallestIndex];
            Quaternion rotation = Quaternion.Euler(0, 180, 0);

            GameObject chatterAvatar = Instantiate(chatterPrefab, startPos, rotation);
            chatterAvatar.transform.SetParent(chatterParent);
            chatterAvatar.name = chatterName;

            chatterAvatar.GetComponent<Chatter>().seatNum = smallest;

            seatList.RemoveAt(smallestIndex);
            posList.RemoveAt(smallestIndex);
        }
        else
        {
            if (currentRow < numberOfRows && currentCol < objectsPerRow)
            {
                float xOffset = (currentRow % 2 == 0) ? 0f : rowOffset;
                float rowStep = startingY + (currentRow * yOffset);

                Vector3 startingPos = new Vector3(startPos.position.x + currentCol * spacing + xOffset, startPos.position.y + rowStep, startPos.position.z + currentRow * spacing);
                Quaternion rotation = Quaternion.Euler(0, 180, 0);

                GameObject chatterAvatar = Instantiate(chatterPrefab, startingPos, rotation);
                chatterAvatar.transform.SetParent(chatterParent);
                chatterAvatar.name = chatterName;

                chatterAvatar.GetComponent<Chatter>().seatNum = seatNum;
                seatNum++;

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
        GameObject chatterObject = Instantiate(chatterPrefab);
        chatterObject.name = chatter;

        Chatter chatterComponent = chatterObject.GetComponent<Chatter>();

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