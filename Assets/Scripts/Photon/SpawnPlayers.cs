using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;

public class SpawnPlayers : MonoBehaviour
{
    public GameObject PlayerPrefab;

    private void Start()
    {
        Vector3 spawnPoint = new Vector3(0, 3.15f, 0);
        PhotonNetwork.Instantiate(PlayerPrefab.name, spawnPoint, Quaternion.identity);
    }
}
