using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Unity.VisualScripting;
using System;

public class ChatterSpawner : MonoBehaviour
{
    private GameObject chatManager;
    public Transform chatterParent;
    public GameObject chatter;
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
        chatManager = GameObject.FindGameObjectWithTag("ChatManager");
    }

    void Update()
    {
        if (Variables.Object(chatManager).Get<string>("newGuy") != null || Input.GetKeyDown(KeyCode.E))
        {
            InstantiateNextChatter(Variables.Object(chatManager).Get<string>("newGuy"));

            Variables.Object(chatManager).Set("newGuy", null);
        }
    }

    private void InstantiateNextChatter(string chatterName)
    {
        var seatList = Variables.Object(this).Get("seatList") as IList<int>;
        var destroyedPosList = Variables.Object(this).Get("destroyedPositionList") as IList<Vector3>;

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
            Vector3 startPos = destroyedPosList[smallestIndex];
            Quaternion rotation = Quaternion.Euler(0, 180, 0);
            GameObject chatterAvatar = PhotonNetwork.Instantiate(chatter.name, startPos, rotation);
            chatterAvatar.transform.SetParent(chatterParent);
            chatterAvatar.name = chatterName;
            Variables.Object(chatterAvatar).Set("SeatNum", smallest);

            seatList.RemoveAt(smallestIndex);
            destroyedPosList.RemoveAt(smallestIndex);
        }
        else
        {
            if (currentRow < numberOfRows && currentCol < objectsPerRow)
            {
                float xOffset = (currentRow % 2 == 0) ? 0f : rowOffset;
                float rowStep = startingY + (currentRow * yOffset);

                Vector3 startingPos = new Vector3(startPos.position.x + currentCol * spacing + xOffset, startPos.position.y + rowStep, startPos.position.z + currentRow * spacing);
                Quaternion rotation = Quaternion.Euler(0, 180, 0);
                GameObject chatterAvatar = PhotonNetwork.Instantiate(chatter.name, startingPos, rotation);
                chatterAvatar.transform.SetParent(chatterParent);
                chatterAvatar.name = chatterName;
                Variables.Object(chatterAvatar).Set("SeatNum", seatNum);
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
}