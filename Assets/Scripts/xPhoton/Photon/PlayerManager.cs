/*
using UnityEngine;
using Photon.Pun;
using Photon.Chat;
using Unity.VisualScripting;
using System.Collections;
using Photon.Realtime;

public class PlayerManager : MonoBehaviourPunCallbacks
{
    public GameObject PlayerPrefab;
    public Transform ChatterParent;
    private GameObject newGuy;

    public void Awake()
    {
        PhotonNetwork.IsMessageQueueRunning = true;
    }

    public void Start()
    {
        //clear chatterList on start
        if (PhotonNetwork.IsMasterClient)
        {
            AotList emptyList = new AotList();
            Variables.Application.Set("chatterList", emptyList);
            Variables.Application.Set("playerList", emptyList);
        }

        if (PhotonNetwork.IsConnectedAndReady)
        {
            Vector3 spawnPoint = new Vector3(0, -10, 0);
            newGuy = PhotonNetwork.Instantiate(PlayerPrefab.name, spawnPoint, Quaternion.identity);

            StartCoroutine(UpdateAndCheck());
        }
    }

    IEnumerator UpdateAndCheck()
    {
        yield return new WaitForSeconds(0.1f);
        
        //Adds the player into the player list when they spawn in
        AotList playerList = Variables.Application.Get<AotList>("playerList");
        playerList.Add(newGuy.name);
        Variables.Application.Set("playerList", playerList);

        //Checks if the player that gets instantiated has an existing chatter body
        AotList chatterList = Variables.Application.Get<AotList>("chatterList");
        if (chatterList.Contains(newGuy.name))
        {
            Transform chatter = ChatterParent.Find(newGuy.name);
            Debug.Log("Chatter: " + chatter);
            if(chatter != null)
            {
                newGuy.transform.position = chatter.position;
            }
        }
        else
        {
            newGuy.transform.position = new Vector3(0, 5, 0);
        }
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        PhotonNetwork.Disconnect();
        PhotonNetwork.LoadLevel("1");
    }
}
*/