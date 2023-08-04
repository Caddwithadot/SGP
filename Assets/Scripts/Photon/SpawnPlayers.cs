using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine.SceneManagement;

public class SpawnPlayers : MonoBehaviourPunCallbacks
{
    public GameObject PlayerPrefab;

    public void Awake()
    {
        PhotonNetwork.IsMessageQueueRunning = true;
    }

    public void Start()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            Vector3 spawnPoint = new Vector3(0, 3.15f, 0);
            PhotonNetwork.Instantiate(PlayerPrefab.name, spawnPoint, Quaternion.identity);
        }
    }
}