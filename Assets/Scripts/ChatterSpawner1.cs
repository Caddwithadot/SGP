using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;

public class ChatterSpawner1 : MonoBehaviour
{
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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            InstantiateNextChatter();
        }
    }

    private void InstantiateNextChatter()
    {
        if (currentRow < numberOfRows && currentCol < objectsPerRow)
        {
            float xOffset = (currentRow % 2 == 0) ? 0f : rowOffset;
            float rowStep = startingY + (currentRow * yOffset);

            Vector3 startingPos = new Vector3(startPos.position.x + currentCol * spacing + xOffset, startPos.position.y + rowStep, startPos.position.z + currentRow * spacing);
            Quaternion rotation = Quaternion.Euler(0, 180, 0);
            GameObject chatterAvatar = PhotonNetwork.Instantiate(chatter.name, startingPos, rotation);
            chatterAvatar.transform.SetParent(chatterParent);

            currentCol++;
            if (currentCol >= objectsPerRow)
            {
                currentCol = 0;
                currentRow++;
            }
        }
    }
}